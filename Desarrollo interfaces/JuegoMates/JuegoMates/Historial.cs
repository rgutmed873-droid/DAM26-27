using System;
using System.Collections.Generic;

namespace JuegoMatematico
{
    class Historial
    {
        private List<string> partidas = new List<string>();

        public void GuardarPartida(string operacion, int puntuacion, int totalPreguntas)
        {
            string registro = $"Juego de {operacion}: {puntuacion}/{totalPreguntas} aciertos.";
            partidas.Add(registro);
        }

        public void MostrarHistorial()
        {
            Console.Clear();
            Console.WriteLine("=== HISTORIAL DE JUEGOS ===");

            if (partidas.Count == 0)
            {
                Console.WriteLine("Aún no has jugado ninguna partida.");
            }
            else
            {
                foreach (string partida in partidas)
                {
                    Console.WriteLine("- " + partida);
                }
            }

            Console.WriteLine("\nPulsa cualquier tecla para volver al menú principal.");
            Console.ReadKey();
        }
    }
}