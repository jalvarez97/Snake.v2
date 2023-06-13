using System;
using System.Drawing;
using System.Threading;

namespace Snake
{
    internal class Program
    {
        public static Tablero oTablero;
        public static Snake oSnake;
        public static Recompensa oRecompensa;       
        public static bool bEjecutando = true;
        public static bool bJugando = false;
        static void Main(string[] args)
        {
            oTablero = new Tablero("Snake App", 65, 20, ConsoleColor.Black
                                   , ConsoleColor.White, new Point(5,3), new Point(60,20));
            oTablero.GenerarMarco();
            
            oRecompensa = new Recompensa(ConsoleColor.DarkGreen, oTablero);
            

            oSnake = new Snake(new Point(8,5), ConsoleColor.DarkGray
                               , ConsoleColor.Gray, oTablero, oRecompensa);
            
            while (bEjecutando)
            {
                oTablero.Menu();
                oTablero.EscucharTeclado(ref bEjecutando, ref bJugando, oSnake);
                while (bJugando)
                {
                    oSnake.Informacion(0, 46);
                    oSnake.Mover();
                    if (!oSnake.estaViva)
                    {
                        bJugando = false;
                        oSnake.Puntaje = 0;
                    }                    
                    Thread.Sleep(100);
                }
                Thread.Sleep(100);
                
            }         
        }
    }
}
