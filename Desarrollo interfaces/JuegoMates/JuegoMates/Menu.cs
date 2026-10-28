using System;

namespace JuegoMatematico
{
    class Menu
    {
        private Historial historial;
        private MotorJuego motor;

        public Menu()
        {
            historial = new Historial();
            motor = new MotorJuego(historial);
        }

        public void Iniciar()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=== JUEGO MATEMÁTICO ===");
                Console.WriteLine("1. Suma");
                Console.WriteLine("2. Resta");
                Console.WriteLine("3. Multiplicación");
                Console.WriteLine("4. División");
                Console.WriteLine("5. Ver historial de juegos");
                Console.WriteLine("6. Salir");
                Console.Write("Elige una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1": motor.Jugar("Suma"); break;
                    case "2": motor.Jugar("Resta"); break;
                    case "3": motor.Jugar("Multiplicación"); break;
                    case "4": motor.Jugar("División"); break;
                    case "5": historial.MostrarHistorial(); break;
                    case "6":
                        salir = true;
                        Console.WriteLine("¡Gracias por jugar! Cerrando programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Pulsa cualquier tecla para intentar de nuevo.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}