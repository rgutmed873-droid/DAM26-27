using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Habitos
{
    internal class Program
    {
        // Cadena de conexión única (Aplicamos el principio DRY)
        private static readonly string connectionString = "Data Source=habitos.db;";

        static void Main(string[] args)
        {
            // 1. Inicializar Base de Datos y Tabla de errores al arrancar
            CrearTablas();
            RegistrarLogEnFichero("Aplicación iniciada.");

            bool salir = false;

            // Bucle principal del menú
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("   REGISTRO DE HÁBITOS DIARIOS   ");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Ver hábitos registrados");
                Console.WriteLine("2. Insertar nuevo hábito");
                Console.WriteLine("3. Actualizar hábito");
                Console.WriteLine("4. Eliminar hábito");
                Console.WriteLine("5. Salir");
                Console.WriteLine("=================================");
                Console.Write("Selecciona una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarHabitos();
                        Pausar();
                        break;
                    case "2":
                        InsertarHabito();
                        Pausar();
                        break;
                    case "3":
                        ActualizarHabito();
                        Pausar();
                        break;
                    case "4":
                        EliminarHabito();
                        Pausar();
                        break;
                    case "5":
                        salir = true;
                        RegistrarLogEnFichero("Aplicación cerrada por el usuario.");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                        Pausar();
                        break;
                }
            }
        }

        // ==========================================
        // 1. BASE DE DATOS: CREACIÓN DE TABLAS
        // ==========================================
        static void CrearTablas()
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();

                    // Tabla de hábitos y Tabla extra para Registro de Errores
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS habitos (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Cantidad INTEGER,
                            Fecha TEXT
                        );
                        CREATE TABLE IF NOT EXISTS errores (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Mensaje TEXT,
                            Fecha TEXT
                        );";

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ManejarError("Error al crear tablas iniciales", ex);
            }
        }

        // ==========================================
        // 2. OPERACIONES CRUD (HÁBITOS)
        // ==========================================
        static void InsertarHabito()
        {
            Console.WriteLine("\n--- INSERTAR NUEVO HÁBITO ---");
            int cantidad = LeerEntero("Introduce la cantidad (ej. nº de vasos de agua): ");
            string fecha = LeerFecha("Introduce la fecha (AAAA-MM-DD) o pulsa ENTER para 'hoy': ");

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO habitos (Cantidad, Fecha) VALUES (@cantidad, @fecha)";
                    command.Parameters.AddWithValue("@cantidad", cantidad);
                    command.Parameters.AddWithValue("@fecha", fecha);

                    command.ExecuteNonQuery();
                }

                Console.WriteLine("¡Hábito registrado con éxito!");
                RegistrarLogEnFichero($"Insertado hábito: Cantidad={cantidad}, Fecha={fecha}");
            }
            catch (Exception ex)
            {
                ManejarError("Error al insertar hábito", ex);
            }
        }

        static void MostrarHabitos()
        {
            Console.WriteLine("\n--- LISTA DE HÁBITOS REGISTRADOS ---");

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT Id, Cantidad, Fecha FROM habitos ORDER BY Fecha DESC";

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No hay hábitos registrados aún.");
                            return;
                        }

                        Console.WriteLine("ID\tCANTIDAD\tFECHA");
                        Console.WriteLine("---------------------------------");
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int cantidad = reader.GetInt32(1);
                            string fecha = reader.GetString(2);

                            Console.WriteLine($"{id}\t{cantidad}\t\t{fecha}");
                        }
                    }
                }
                RegistrarLogEnFichero("Consulta de hábitos realizada.");
            }
            catch (Exception ex)
            {
                ManejarError("Error al consultar hábitos", ex);
            }
        }

        static void ActualizarHabito()
        {
            MostrarHabitos();
            Console.WriteLine("\n--- ACTUALIZAR HÁBITO ---");
            int id = LeerEntero("Introduce el ID del registro a modificar: ");
            int nuevaCantidad = LeerEntero("Introduce la nueva cantidad: ");
            string nuevaFecha = LeerFecha("Introduce la nueva fecha (AAAA-MM-DD) o ENTER para 'hoy': ");

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UPDATE habitos SET Cantidad = @cantidad, Fecha = @fecha WHERE Id = @id";
                    command.Parameters.AddWithValue("@cantidad", nuevaCantidad);
                    command.Parameters.AddWithValue("@fecha", nuevaFecha);
                    command.Parameters.AddWithValue("@id", id);

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("¡Hábito actualizado correctamente!");
                        RegistrarLogEnFichero($"Actualizado hábito ID={id}: Cantidad={nuevaCantidad}, Fecha={nuevaFecha}");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún registro con ese ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                ManejarError("Error al actualizar hábito", ex);
            }
        }

        static void EliminarHabito()
        {
            MostrarHabitos();
            Console.WriteLine("\n--- ELIMINAR HÁBITO ---");
            int id = LeerEntero("Introduce el ID del registro que deseas borrar: ");

            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM habitos WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    int filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        Console.WriteLine("¡Hábito eliminado correctamente!");
                        RegistrarLogEnFichero($"Eliminado hábito ID={id}");
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún registro con ese ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                ManejarError("Error al borrar hábito", ex);
            }
        }

        // ==========================================
        // 3. VALIDACIÓN DE ENTRADAS (ROBUSTEZ)
        // ==========================================
        static int LeerEntero(string mensaje)
        {
            int numero;
            Console.Write(mensaje);
            while (!int.TryParse(Console.ReadLine(), out numero) || numero < 0)
            {
                Console.Write("Entrada no válida. Por favor, introduce un entero positivo: ");
            }
            return numero;
        }

        static string LeerFecha(string mensaje)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine()?.Trim();

            // Comando rápido: si pulsa Enter o escribe "hoy", asigna la fecha actual
            if (string.IsNullOrEmpty(entrada) || entrada.ToLower() == "hoy")
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime fechaValida;
            while (!DateTime.TryParse(entrada, out fechaValida))
            {
                Console.Write("Fecha incorrecta (formato válido: AAAA-MM-DD). Inténtalo de nuevo: ");
                entrada = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(entrada) || entrada.ToLower() == "hoy")
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
            }

            return fechaValida.ToString("yyyy-MM-dd");
        }

        // ==========================================
        // 4. EXTRAS: MANEJO DE ERRORES Y LOGS
        // ==========================================
        static void ManejarError(string contexto, Exception ex)
        {
            Console.WriteLine($"\n[ERROR] {contexto}: {ex.Message}");

            // Extra 1: Guardar error en la Base de Datos SQLite
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

            // Extra 2: Guardar error en el Fichero .log diario
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

                // Genera un archivo con formato: DD_MM_YYYY.log
                string nombreArchivo = $"{DateTime.Now:dd_MM_yyyy}.log";
                string rutaCompleta = Path.Combine(carpetaLogs, nombreArchivo);

                string linea = $"[{DateTime.Now:HH:mm:ss}] {mensaje}{Environment.NewLine}";
                File.AppendAllText(rutaCompleta, linea);
            }
            catch { /* Evitar fallos globales */ }
        }

        static void Pausar()
        {
            Console.WriteLine("\nPulsa cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}