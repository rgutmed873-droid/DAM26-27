using System;

namespace JuegoMatematico
{
    class MotorJuego
    {
        private Random random = new Random();
        private Historial historial;

        public MotorJuego(Historial historialCompartido)
        {
            this.historial = historialCompartido;
        }

        public void Jugar(string operacion)
        {
            int puntuacion = 0;
            int totalPreguntas = 5;

            Console.WriteLine($"\n--- Iniciando juego de {operacion} ---");

            for (int i = 1; i <= totalPreguntas; i++)
            {
                int num1 = 0;
                int num2 = 0;
                int respuestaCorrecta = 0;
                string simbolo = "";

                if (operacion == "Suma")
                {
                    num1 = random.Next(1, 50);
                    num2 = random.Next(1, 50);
                    respuestaCorrecta = num1 + num2;
                    simbolo = "+";
                }
                else if (operacion == "Resta")
                {
                    num1 = random.Next(1, 50);
                    num2 = random.Next(1, num1);
                    respuestaCorrecta = num1 - num2;
                    simbolo = "-";
                }
                else if (operacion == "Multiplicación")
                {
                    num1 = random.Next(1, 11);
                    num2 = random.Next(1, 11);
                    respuestaCorrecta = num1 * num2;
                    simbolo = "*";
                }
                else if (operacion == "División")
                {
                    do
                    {
                        num1 = random.Next(0, 101);
                        num2 = random.Next(1, 50);
                    }
                    while (num1 % num2 != 0);

                    respuestaCorrecta = num1 / num2;
                    simbolo = "/";
                }

                Console.Write($"Pregunta {i}: ¿Cuánto es {num1} {simbolo} {num2}? = ");
                string entrada = Console.ReadLine();
                int respuestaUsuario;

                if (int.TryParse(entrada, out respuestaUsuario))
                {
                    if (respuestaUsuario == respuestaCorrecta)
                    {
                        Console.WriteLine("¡Correcto! +1 punto.\n");
                        puntuacion++;
                    }
                    else
                    {
                        Console.WriteLine($"Incorrecto. La respuesta correcta era {respuestaCorrecta}.\n");
                    }
                }
                else
                {
                    Console.WriteLine($"Formato incorrecto (no es un número). La respuesta correcta era {respuestaCorrecta}.\n");
                }
            }

            Console.WriteLine($"Fin del juego. Tu puntuación es: {puntuacion} de {totalPreguntas}");
            historial.GuardarPartida(operacion, puntuacion, totalPreguntas);

            Console.WriteLine("Pulsa cualquier tecla para volver al menú principal.");
            Console.ReadKey();
        }
    }
}