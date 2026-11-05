using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace FichaUsuario
{
    public partial class MainWindow : Window
    {
        // Cadena de conexión. Si fichas.db no existe, SQLite lo crea al abrir.
        private const string CadenaConexion = "Data Source=fichas.db";

        public MainWindow()
        {
            InitializeComponent(); // construye los controles definidos en el XAML
        }

        // =====================================================
        // EVENTO 1: la ventana se acaba de abrir (Loaded)
        // =====================================================
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CrearBaseDeDatos();                  // crea tablas y ciudades si faltan
                CargarCiudades();                    // rellena el ComboBox desde la BD
                dpFecha.SelectedDate = DateTime.Today; // fecha de hoy por defecto
            }
            catch (SqliteException)
            {
                MessageBox.Show("No se pudo abrir la base de datos.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // =====================================================
        // EVENTO 2: se marca o desmarca la casilla
        // =====================================================
        private void chkCondiciones_Changed(object sender, RoutedEventArgs e)
        {
            // IsChecked es bool? (puede ser true, false o null).
            // "== true" da true solo si está marcada.
            // Así, el botón queda habilitado si y solo si la casilla está marcada.
            btnGuardar.IsEnabled = chkCondiciones.IsChecked == true;
        }

        // =====================================================
        // EVENTO 3: se pulsa Guardar (Click)
        // =====================================================
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // ---- a) VALIDAR: si algo falla, avisamos, ponemos el foco y salimos (return) ----
            string nombre = txtNombre.Text.Trim(); // Trim quita espacios al principio y final
            string correo = txtCorreo.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre)) // vacío o solo espacios
            {
                Avisar("Escribe el nombre del usuario.", txtNombre);
                return;
            }

            if (!CorreoValido(correo))
            {
                Avisar("El correo no tiene un formato válido (ejemplo: nombre@dominio.es).", txtCorreo);
                return;
            }

            // SelectedItem devuelve el objeto elegido. "as CiudadItem" da null si no hay selección.
            CiudadItem ciudad = cmbCiudad.SelectedItem as CiudadItem;
            if (ciudad == null)
            {
                Avisar("Selecciona una ciudad.", cmbCiudad);
                return;
            }

            // SelectedDate es DateTime? (puede ser null si no hay fecha)
            if (!dpFecha.SelectedDate.HasValue)
            {
                Avisar("Selecciona una fecha.", dpFecha);
                return;
            }
            DateTime fecha = dpFecha.SelectedDate.Value;

            // ---- b) INSERTAR en la base de datos ----
            try
            {
                // using cierra y libera la conexión automáticamente al salir del bloque
                using (var conexion = new SqliteConnection(CadenaConexion))
                {
                    conexion.Open();

                    using (var comando = conexion.CreateCommand())
                    {
                        // $nombre, $correo... son PARÁMETROS: evitan la inyección SQL.
                        // Nunca se concatena el texto del usuario dentro del SQL.
                        comando.CommandText =
                            @"INSERT INTO Usuario (Nombre, Correo, CiudadId, Fecha, Acepta)
                              VALUES ($nombre, $correo, $ciudadId, $fecha, 1)";

                        comando.Parameters.AddWithValue("$nombre", nombre);
                        comando.Parameters.AddWithValue("$correo", correo);
                        comando.Parameters.AddWithValue("$ciudadId", ciudad.Id);
                        // La BD guarda la fecha como texto con formato yyyy-MM-dd
                        comando.Parameters.AddWithValue("$fecha",
                            fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                        comando.ExecuteNonQuery(); // ejecuta el INSERT
                    }
                }
            }
            // Código 19 = violación de restricción (aquí, el UNIQUE del correo)
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                Avisar("Ya existe un usuario con el correo " + correo + ".", txtCorreo);
                return;
            }
            // Cualquier otro error de BD: mensaje genérico y la aplicación no se cierra
            catch (SqliteException)
            {
                MessageBox.Show("No se ha podido guardar el usuario. Inténtalo de nuevo.",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // ---- c) MOSTRAR EL RESUMEN con el formato: Marta Ruiz · correo · Sevilla · 12/11/2026 ----
            txtResumen.Text = nombre + " · " + correo + " · " + ciudad.Nombre + " · " +
                              fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            // ---- d) LIMPIAR el formulario (el resumen NO se borra) ----
            txtNombre.Clear();
            txtCorreo.Clear();
            cmbCiudad.SelectedIndex = -1;            // -1 = ninguna ciudad seleccionada
            dpFecha.SelectedDate = DateTime.Today;
            chkCondiciones.IsChecked = false;        // dispara Unchecked, así Guardar se deshabilita
            txtNombre.Focus();                       // cursor listo para el siguiente usuario
        }

        // =====================================================
        // MÉTODOS AUXILIARES
        // =====================================================

        // Muestra un mensaje de aviso y manda el foco al campo erróneo
        private void Avisar(string mensaje, UIElement campo)
        {
            MessageBox.Show(mensaje, "Datos incorrectos",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
            campo.Focus();
        }

        // Comprueba el formato del correo usando MailAddress
        private static bool CorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return false;
            try
            {
                var direccion = new MailAddress(correo); // lanza FormatException si es inválido
                // Address == correo evita casos como "Juan <juan@x.es>".
                // Exigimos un punto en el dominio (nombre@dominio.es).
                return direccion.Address == correo && direccion.Host.IndexOf('.') >= 0;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // Crea las tablas y las ciudades. Se puede ejecutar muchas veces sin duplicar nada
        // gracias a IF NOT EXISTS e INSERT OR IGNORE.
        private void CrearBaseDeDatos()
        {
            using (var conexion = new SqliteConnection(CadenaConexion))
            {
                conexion.Open();

                using (var comando = conexion.CreateCommand())
                {
                    // Esquema exacto del enunciado (sin cambiar nombres)
                    comando.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Ciudad (
                            Id     INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre TEXT NOT NULL UNIQUE
                        );

                        CREATE TABLE IF NOT EXISTS Usuario (
                            Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre    TEXT    NOT NULL,
                            Correo    TEXT    NOT NULL UNIQUE,
                            CiudadId  INTEGER NOT NULL REFERENCES Ciudad(Id),
                            Fecha     TEXT    NOT NULL,
                            Acepta    INTEGER NOT NULL CHECK (Acepta = 1),
                            FechaAlta TEXT    NOT NULL DEFAULT (datetime('now','localtime'))
                        );

                        INSERT OR IGNORE INTO Ciudad (Nombre) VALUES
                            ('Almería'), ('Cádiz'), ('Córdoba'), ('Granada'),
                            ('Huelva'), ('Jaén'), ('Málaga'), ('Sevilla');";

                    comando.ExecuteNonQuery(); // ejecuta todo el script
                }
            }
        }

        // Lee las ciudades de la BD y las pone en el ComboBox
        private void CargarCiudades()
        {
            var ciudades = new List<CiudadItem>();

            using (var conexion = new SqliteConnection(CadenaConexion))
            {
                conexion.Open();

                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = "SELECT Id, Nombre FROM Ciudad ORDER BY Nombre";

                    // ExecuteReader devuelve las filas; Read() avanza fila a fila
                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            ciudades.Add(new CiudadItem
                            {
                                Id = lector.GetInt32(0),      // columna 0 = Id
                                Nombre = lector.GetString(1)  // columna 1 = Nombre
                            });
                        }
                    }
                }
            }

            // El ComboBox muestra "Nombre" (DisplayMemberPath) y guarda "Id" (SelectedValuePath)
            cmbCiudad.ItemsSource = ciudades;
        }
    }

    // Representa una ciudad en el ComboBox. Las propiedades se llaman Id y Nombre
    // porque así lo indican DisplayMemberPath y SelectedValuePath en el XAML.
    public class CiudadItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}