using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Recompensa
    {
        public Point Posicion { get; set; }
        public ConsoleColor oColor { get; set; }
        public Tablero oColisionTablero { get; set; } 

        public Recompensa(ConsoleColor color, Tablero tablero) 
        { 
            oColor = color;
            oColisionTablero = tablero;
        }

        private void Generar()
        {
            Console.ForegroundColor = oColor;
            Console.SetCursorPosition(Posicion.X, Posicion.Y);
            Console.Write("©");
        }

        public bool GenerarRecompensa(Snake snake) 
        {
            //Sumamos la cabeza al array del cuerpo
            int nSnakeLength = snake.lstCuerpo.Count + 1;
            if((oColisionTablero.nArea - nSnakeLength) <=0)
                return false;
            
            Random rnd = new Random();
            int nX = rnd.Next(oColisionTablero.pLimiteTop.X + 1, oColisionTablero.pLimiteBottom.X);
            int nY = rnd.Next(oColisionTablero.pLimiteTop.Y + 1, oColisionTablero.pLimiteBottom.Y);
            Posicion = new Point(nX, nY);

            foreach(Point item in snake.lstCuerpo)
            {
                if ((nX == item.X && nY==item.Y) ||
                    (nX == snake.pCabeza.X && nY == snake.pCabeza.Y))
                {
                    if (GenerarRecompensa(snake))
                        return true;
                }
            }

            Generar();
            return true;
        }
    }
}
