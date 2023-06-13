using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    internal class Tablero
    {
        public string sTitulo { get; set; }
        public int nAncho { get; set; }
        public int nAlto { get; set; }
        public int nArea { get; set; }
        public ConsoleColor oColorFondo { get; set; }
        public ConsoleColor oColorTexto { get; set; }
        public Point pLimiteTop { get; set; }
        public Point pLimiteBottom { get; set; }
        public Snake oSnake { get; set; }

        public Tablero(string titulo, int ancho, int alto
                        , ConsoleColor colorFondo, ConsoleColor colorTexto
                        , Point limiteTop, Point limiteBottom) 
        {
            sTitulo = titulo;
            nAncho = ancho;
            nAlto = alto;
            oColorFondo = colorFondo;
            oColorTexto = colorTexto;
            pLimiteTop = limiteTop;
            pLimiteBottom = limiteBottom;
            nArea = ((pLimiteBottom.X - pLimiteTop.X) - 1) * ((pLimiteBottom.Y - pLimiteTop.Y) - 1);
            Iniciar();
        }

        public void Iniciar()
        {
            Console.SetWindowSize(nAncho, nAlto);            
            Console.Title = sTitulo;
            Console.CursorVisible = false;
            Console.BackgroundColor = oColorFondo;
            Console.Clear();
            oSnake = new Snake(new Point(pLimiteBottom.X/2, pLimiteBottom.Y - 3), ConsoleColor.DarkGray
                               , ConsoleColor.Gray, this, null);
            oSnake.GenerarCuerpo(4);
        }
        
        public void GenerarMarco()
        {
            Console.ForegroundColor = oColorTexto;
            //Recorremos el ancho del tablero
            for (int i = pLimiteTop.X; i < pLimiteBottom.X; i++)
            {
                Console.SetCursorPosition(i, pLimiteTop.Y);
                Console.Write("─");
                Console.SetCursorPosition(i, pLimiteBottom.Y);
                Console.Write("─");
            }
            //Recorremos la altura del tablero
            for (int i = pLimiteTop.Y; i < pLimiteBottom.Y; i++)
            {
                Console.SetCursorPosition(pLimiteTop.X, i);
                Console.Write("│");
                Console.SetCursorPosition(pLimiteBottom.X, i);
                Console.Write("│");
            }
            //Reemplazamos simbolo esquina superior izquierda
            Console.SetCursorPosition(pLimiteTop.X, pLimiteTop.Y);
            Console.Write("┌");
            //Reemplazamos simbolo esquina inferior izquierda
            Console.SetCursorPosition(pLimiteTop.X, pLimiteBottom.Y);
            Console.Write("└");
            //Reemplazamos simbolo esquina superior derecha
            Console.SetCursorPosition(pLimiteBottom.X, pLimiteTop.Y);
            Console.Write("┐");
            //Reemplazamos simbolo esquina inferior derecha
            Console.SetCursorPosition(pLimiteBottom.X, pLimiteBottom.Y);
            Console.Write("┘");
        }
        public void Menu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(pLimiteTop.X + (pLimiteBottom.X / 2) - 10
                                    , pLimiteTop.Y + (pLimiteBottom.Y / 2) - 4);
            Console.Write("SNAKE CÁLVICO GAME");
            Console.SetCursorPosition(pLimiteTop.X + (pLimiteBottom.X / 2) - 8
                                    , pLimiteTop.Y + (pLimiteBottom.Y / 2) - 2);
            Console.Write("ENTER - JUGAR");
            Console.SetCursorPosition(pLimiteTop.X + (pLimiteBottom.X / 2) - 8
                                    , pLimiteTop.Y + (pLimiteBottom.Y / 2) - 1);
            Console.Write("ESC - SALIR");
            
            oSnake.MoverMenu();
        }
        public void EscucharTeclado(ref bool ejecutando, ref bool jugando, Snake snake)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo oTecla = Console.ReadKey(true);
                if (oTecla.Key == ConsoleKey.Enter)
                {
                    jugando = true;
                    Console.Clear();
                    GenerarMarco();
                    snake.Iniciar();
                }
                if (oTecla.Key == ConsoleKey.Escape)
                {
                    ejecutando = false;
                    Console.Clear();                    
                }
            }
        }

        public void FinJuego(String sTexto)
        {
            Console.Clear ();
            GenerarMarco();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(pLimiteTop.X + (pLimiteBottom.X / 2) - 10
                                    , pLimiteTop.Y + (pLimiteBottom.Y / 2) - 2);
            Console.Write(sTexto); 
            Thread.Sleep (3000);
            Console.Clear();
            GenerarMarco();
        }
    }
}
