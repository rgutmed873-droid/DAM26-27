using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using System.Windows.Input;

namespace Habitos
{
    // MAINWINDOW.XAML.CS: lo que hace la ventana.
    // Es tu Program.cs de consola adaptado: los mismos métodos (CrearTablas,
    // InsertarHabito, ManejarError, RegistrarLogEnFichero...), pero en vez de
    // Console.ReadLine y Console.WriteLine usan los controles de la ventana.
    public partial class MainWindow : Window
    {
        // Cadena de conexión única (igual que en la versión de consola)
        private const string connectionString = "Data Source=habitos.db;";

        // Id del hábito elegido en la tabla. 0 significa "ninguno elegido".
        private int idSeleccionado = 0;

        public MainWindow()
        {
            InitializeComponent(); // construye los controles del XAML
        }

        // =====================================================
        // EVENTOS DE LA VENTANA
        // =====================================================

        // Se ejecuta al abrirse la ventana (equivale al inicio de Main en la consola)
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CrearTablas();
            RegistrarLogEnFichero("Aplicación iniciada.");
            dpFecha.SelectedDate = DateTime.Today;
            MostrarHabitos();
        }

        // Se ejecuta al cerrarse la ventana (equivale a la opción "Salir")
        private void Window_Closed(object? sender, EventArgs e)
        {
            RegistrarLogEnFichero("Aplicación cerrada por el usuario.");
        }

        // Se ejecuta al pulsar CUALQUIER tecla en la ventana principal.
        // "Preview" significa que la ventana recibe la tecla ANTES que el control
        // donde estás escribiendo, así que ESC funciona estés donde estés.
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Si el calendario del DatePicker está desplegado, ESC solo debe cerrar
            // el calendario y no toda la aplicación.
            if (e.Key == Key.Escape && !dpFecha.IsDropDownOpen)
            {
                Close(); // cierra la ventana principal = termina el programa
                         // (antes se ejecuta Window_Closed, que escribe en el log)
            }
        }

        // Se ejecuta al hacer clic en una fila de la tabla (ver RowStyle en el XAML).
        // Abre la ventana de detalle con los datos de ese hábito.
        private void Fila_Click(object sender, MouseButtonEventArgs e)
        {
            // sender es la fila pulsada (DataGridRow); su propiedad Item es el Habito de esa fila.
            // "as" lo convierte a Habito (o null si no lo fuera)
            DataGridRow? fila = sender as DataGridRow;
            Habito? h = fila?.Item as Habito;

            if (h == null)
            {
                return;
            }

            RegistrarLogEnFichero($"Consultado detalle del hábito ID={h.Id}");

            DetalleWindow ventana = new DetalleWindow(h);
            ventana.Owner = this; // la ventana de detalle "pertenece" a la principal (se centra sobre ella)

            // ShowDialog() abre la ventana y ESPERA a que se cierre (es modal):
            // mientras esté abierta no se puede tocar la ventana principal.
            // Cuando se cierra con ESC, el programa continúa aquí y vuelve a la anterior.
            ventana.ShowDialog();
        }

        // Se ejecuta al elegir otra fila de la tabla
        private void dgHabitos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // "as Habito" convierte la fila elegida en un Habito (o null si no hay ninguna)
            Habito? elegido = dgHabitos.SelectedItem as Habito;

            if (elegido == null)
            {
                idSeleccionado = 0;
                btnModificar.IsEnabled = false;
                btnEliminar.IsEnabled = false;
                return;
            }

            // Guardamos su Id, activamos los botones y copiamos sus datos al formulario
            idSeleccionado = elegido.Id;
            btnModificar.IsEnabled = true;
            btnEliminar.IsEnabled = true;

            txtNombre.Text = elegido.Nombre;
            txtCantidad.Text = elegido.Cantidad.ToString();
            dpFecha.SelectedDate = elegido.Fecha;
        }

        // =====================================================
        // BOTONES
        // =====================================================

        // Botón Añadir (opción 2 de la consola: InsertarHabito)
        private void btnAnadir_Click(object sender, RoutedEventArgs e)
        {
            Habito? h = LeerFormulario();
            if (h == null)
            {
                return; // los datos no eran válidos y ya se ha avisado
            }

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO habitos (Nombre, Cantidad, Fecha) VALUES (@nombre, @cantidad, @fecha)";
                    command.Parameters.AddWithValue("@nombre", h.Nombre);
                    command.Parameters.AddWithValue("@cantidad", h.Cantidad);
                    command.Parameters.AddWithValue("@fecha", FechaParaBd(h.Fecha));

                    command.ExecuteNonQuery();
                }

                RegistrarLogEnFichero($"Insertado hábito '{h.Nombre}': Cantidad={h.Cantidad}, Fecha={FechaParaBd(h.Fecha)}");
            }
            catch (Exception ex)
            {
                ManejarError("Error al insertar hábito", ex);
                return;
            }

            Limpiar();
            MostrarHabitos();
            txtEstado.Text = "Hábito añadido: " + h.Nombre;
        }

        // Botón Modificar (opción 3 de la consola: ActualizarHabito)
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (idSeleccionado == 0)
            {
                Avisar("Elige primero un hábito de la tabla.", dgHabitos);
                return;
            }

            Habito? h = LeerFormulario();
            if (h == null)
            {
                return;
            }

            int id = idSeleccionado;

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE habitos SET Nombre = @nombre, Cantidad = @cantidad, Fecha = @fecha WHERE Id = @id";
                    command.Parameters.AddWithValue("@nombre", h.Nombre);
                    command.Parameters.AddWithValue("@cantidad", h.Cantidad);
                    command.Parameters.AddWithValue("@fecha", FechaParaBd(h.Fecha));
                    command.Parameters.AddWithValue("@id", id);

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas == 0)
                    {
                        Avisar("No se encontró ningún registro con ese ID.", dgHabitos);
                        return;
                    }
                }

                RegistrarLogEnFichero($"Actualizado hábito ID={id}: '{h.Nombre}', Cantidad={h.Cantidad}, Fecha={FechaParaBd(h.Fecha)}");
            }
            catch (Exception ex)
            {
                ManejarError("Error al actualizar hábito", ex);
                return;
            }

            Limpiar();
            MostrarHabitos();
            txtEstado.Text = "Hábito modificado: " + h.Nombre;
        }

        // Botón Eliminar (opción 4 de la consola: EliminarHabito)
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (idSeleccionado == 0)
            {
                Avisar("Elige primero un hábito de la tabla.", dgHabitos);
                return;
            }

            int id = idSeleccionado;
            string nombre = txtNombre.Text;

            // Preguntar antes de borrar
            MessageBoxResult respuesta = MessageBox.Show(
                "¿Eliminar el hábito '" + nombre + "'?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (respuesta != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM habitos WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas == 0)
                    {
                        Avisar("No se encontró ningún registro con ese ID.", dgHabitos);
                        return;
                    }
                }

                RegistrarLogEnFichero($"Eliminado hábito ID={id}");
            }
            catch (Exception ex)
            {
                ManejarError("Error al borrar hábito", ex);
                return;
            }

            Limpiar();
            MostrarHabitos();
            txtEstado.Text = "Hábito eliminado: " + nombre;
        }

        // Botón Limpiar
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
            txtEstado.Text = "Listo";
        }

        // =====================================================
        // FORMULARIO
        // =====================================================

        // Lee el formulario y lo convierte en un Habito.
        // Si algún dato no es válido, avisa y devuelve null (el "?" indica que puede ser null).
        private Habito? LeerFormulario()
        {
            string nombre = txtNombre.Text.Trim();
            if (nombre == "")
            {
                Avisar("Escribe el nombre del hábito.", txtNombre);
                return null;
            }

            // TryParse convierte el texto a número sin dar error si no puede (devuelve false)
            int cantidad;
            if (!int.TryParse(txtCantidad.Text.Trim(), out cantidad) || cantidad < 0)
            {
                Avisar("La cantidad debe ser un número entero positivo.", txtCantidad);
                return null;
            }

            // SelectedDate es DateTime? (puede ser null si no se ha elegido fecha)
            if (dpFecha.SelectedDate == null)
            {
                Avisar("Elige una fecha.", dpFecha);
                return null;
            }

            Habito h = new Habito();
            h.Nombre = nombre;
            h.Cantidad = cantidad;
            h.Fecha = dpFecha.SelectedDate.Value;
            return h;
        }

        // Vacía el formulario y deja la fecha de hoy
        private void Limpiar()
        {
            txtNombre.Clear();
            txtCantidad.Clear();
            dpFecha.SelectedDate = DateTime.Today;
            dgHabitos.SelectedItem = null; // quita la fila elegida (y desactiva Modificar y Eliminar)
            txtNombre.Focus();
        }

        // Muestra un aviso y pone el cursor en el campo que falla
        private void Avisar(string mensaje, UIElement campo)
        {
            MessageBox.Show(mensaje, "Datos incorrectos", MessageBoxButton.OK, MessageBoxImage.Warning);
            campo.Focus();
        }

        // La fecha se guarda en la base de datos como texto: 2026-11-05
        private static string FechaParaBd(DateTime fecha)
        {
            return fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        // =====================================================
        // BASE DE DATOS
        // =====================================================

        static void CrearTablas()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    // Tabla de hábitos y tabla extra para el registro de errores
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS habitos (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre TEXT NOT NULL,
                            Cantidad INTEGER,
                            Fecha TEXT
                        );
                        CREATE TABLE IF NOT EXISTS errores (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Mensaje TEXT,
                            Fecha TEXT
                        );";
                    command.ExecuteNonQuery();

                    // Si habitos.db es antigua y no tiene la columna Nombre, se la añadimos
                    // (CREATE TABLE IF NOT EXISTS no modifica una tabla que ya existe)
                    var comprobar = connection.CreateCommand();
                    comprobar.CommandText = "SELECT COUNT(*) FROM pragma_table_info('habitos') WHERE name = 'Nombre'";
                    bool tieneNombre = Convert.ToInt32(comprobar.ExecuteScalar()) > 0;

                    if (!tieneNombre)
                    {
                        var alterar = connection.CreateCommand();
                        alterar.CommandText = "ALTER TABLE habitos ADD COLUMN Nombre TEXT NOT NULL DEFAULT ''";
                        alterar.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ManejarError("Error al crear tablas iniciales", ex);
            }
        }

        // Lee los hábitos de la base de datos y los pone en la tabla de la ventana
        // (opción 1 de la consola: MostrarHabitos)
        private void MostrarHabitos()
        {
            List<Habito> lista = new List<Habito>();

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT Id, Nombre, Cantidad, Fecha FROM habitos ORDER BY Fecha DESC, Id DESC";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Habito h = new Habito();
                            h.Id = reader.GetInt32(0);
                            h.Nombre = reader.GetString(1);
                            h.Cantidad = reader.GetInt32(2);
                            h.Fecha = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            lista.Add(h);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ManejarError("Error al consultar hábitos", ex);
                return;
            }

            dgHabitos.ItemsSource = lista; // la tabla enseña esta lista
            txtTotal.Text = "Registros: " + lista.Count;
            RegistrarLogEnFichero("Consulta de hábitos realizada.");
        }

        // =====================================================
        // ERRORES Y LOG (igual que en la versión de consola)
        // =====================================================

        static void ManejarError(string contexto, Exception ex)
        {
            // En la consola era Console.WriteLine; aquí es una ventana de aviso
            MessageBox.Show(contexto + ": " + ex.Message, "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);

            // Guardar el error en la base de datos
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO errores (Mensaje, Fecha) VALUES (@mensaje, @fecha)";
                    command.Parameters.AddWithValue("@mensaje", $"{contexto}: {ex.Message}");
                    command.Parameters.AddWithValue("@fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
            }
            catch { /* Evitar cierres si falla la BD */ }

            // Guardar el error en el fichero .log diario
            RegistrarLogEnFichero($"[ERROR] {contexto}: {ex.Message}");
        }

        static void RegistrarLogEnFichero(string mensaje)
        {
            try
            {
                string carpetaLogs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(carpetaLogs))
                {
                    Directory.CreateDirectory(carpetaLogs);
                }

                // Un fichero por día: DD_MM_YYYY.log
                string nombreArchivo = $"{DateTime.Now:dd_MM_yyyy}.log";
                string rutaCompleta = Path.Combine(carpetaLogs, nombreArchivo);

                string linea = $"[{DateTime.Now:HH:mm:ss}] {mensaje}{Environment.NewLine}";
                File.AppendAllText(rutaCompleta, linea);
            }
            catch { /* Evitar fallos globales */ }
        }
    }
}
