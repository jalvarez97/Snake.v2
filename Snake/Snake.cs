using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    internal class Snake
    {
        public bool estaViva { get; set; }
        public ConsoleColor oColorCabeza { get; set; }
        public ConsoleColor oColorCuerpo { get; set; }
        public Tablero oColisionTablero { get; set; }
        public List <Point> lstCuerpo { get; set; }
        public Point pCabezaInicial { get; set; }
        public Point pCabeza { get; set; }  
        public Recompensa oRecompensaColision { get; set; }
        public int Puntaje { get; set; }
        public int PuntajeMax { get; set; }
        enum Direccion 
        { 
            Arriba,Abajo,Derecha,Izquierda
        }
        private Direccion direccion;
        private bool bObteniendoRecompensa;

        public Snake (Point posicion, ConsoleColor colorCabeza
                        , ConsoleColor colorCuerpo ,Tablero tablero
                        , Recompensa recompensa)
        {
            oColorCabeza = colorCabeza;
            oColorCuerpo = colorCuerpo;
            oColisionTablero = tablero;
            pCabezaInicial = posicion;
            pCabeza = posicion;
            Puntaje = 0;
            PuntajeMax = 0;
            lstCuerpo = new List <Point> ();
            oRecompensaColision = recompensa;
            direccion = Direccion.Derecha;
        }

        public void Iniciar()
        {
            lstCuerpo.Clear ();
            pCabeza = pCabezaInicial;
            GenerarCuerpo(2);
            estaViva = true;
            direccion = Direccion.Derecha;
            oRecompensaColision.GenerarRecompensa(this);
        }

        public void GenerarCuerpo(int nPartes)
        {
            int nX = pCabeza.X - 1;
            for(int i = 0; i < nPartes; i++)
            {
                Console.SetCursorPosition(nX, pCabeza.Y);
                Console.Write("█");
                lstCuerpo.Add(new Point(nX, pCabeza.Y));
                nX--;
            }
        }

        public void Mover()
        {
            EscucharTeclado();
            Point pCabezaAnt = pCabeza;
            MoverCabeza();
            MoverCuerpo(pCabezaAnt);
            ColisionesRecompensa();
            if (ColisionesCuerpo())
            {
                Muerte();
                oColisionTablero.FinJuego("H A S   M U E R T O");
            }
        }

        public void MoverCabeza() 
        {
            Console.ForegroundColor = oColorCabeza;
            Console.SetCursorPosition(pCabeza.X, pCabeza.Y);
            Console.Write(" ");
            switch (direccion)
            {
                case Direccion.Arriba:
                    pCabeza = new Point(pCabeza.X, pCabeza.Y -1);
                    break;
                case Direccion.Abajo:
                    pCabeza = new Point(pCabeza.X, pCabeza.Y + 1);
                    break;
                case Direccion.Derecha:
                    pCabeza = new Point(pCabeza.X + 1, pCabeza.Y);
                    break;
                case Direccion.Izquierda:
                    pCabeza = new Point(pCabeza.X - 1, pCabeza.Y);
                    break;
            }
            ColisionesTableroBorde();
            //Posicionamos el curso en la nueva posicion actualizada segun direccion
            Console.SetCursorPosition(pCabeza.X, pCabeza.Y);
            Console.WriteLine("█");
        }
        
        public void MoverCuerpo(Point posCabezaAnterior)
        {
            Console.ForegroundColor = oColorCuerpo;
            Console.SetCursorPosition(posCabezaAnterior.X, posCabezaAnterior.Y);
            Console.Write("█");
            lstCuerpo.Insert(0, posCabezaAnterior);

            //Si acaba de obtener la recompensa no se elimina el ultimo caracter
            //del cuerpo de la serpiente.
            if (bObteniendoRecompensa) 
            {
                bObteniendoRecompensa = false;
                return;
            }

            Console.SetCursorPosition(lstCuerpo[lstCuerpo.Count -1].X, lstCuerpo[lstCuerpo.Count - 1].Y);
            Console.Write(" ");
            lstCuerpo.Remove(lstCuerpo[lstCuerpo.Count - 1]);
        }

        public void MoverMenu() 
        {
            direccion = Direccion.Derecha;
            Point pCabezaAnterior = pCabeza;
            MoverCabeza();
            MoverCuerpo(pCabezaAnterior);
        }

        private void EscucharTeclado()
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo oTecla = Console.ReadKey(true);  
                if (oTecla.Key == ConsoleKey.RightArrow
                    && (direccion != Direccion.Izquierda))
                    direccion = Direccion.Derecha;
                if (oTecla.Key == ConsoleKey.LeftArrow
                     && (direccion != Direccion.Derecha))
                    direccion = Direccion.Izquierda;
                if (oTecla.Key == ConsoleKey.UpArrow
                     && (direccion != Direccion.Abajo))
                    direccion = Direccion.Arriba;
                if (oTecla.Key == ConsoleKey.DownArrow
                     && (direccion != Direccion.Arriba))
                    direccion = Direccion.Abajo;
            }
        }
        
        private void ColisionesTableroBorde()
        {
            if (pCabeza.X <= oColisionTablero.pLimiteTop.X)
                pCabeza = new Point(oColisionTablero.pLimiteBottom.X - 1, pCabeza.Y);

            if (pCabeza.X >= oColisionTablero.pLimiteBottom.X)
                pCabeza = new Point(oColisionTablero.pLimiteTop.X + 1, pCabeza.Y);

            if (pCabeza.Y <= oColisionTablero.pLimiteTop.Y)
                pCabeza = new Point(pCabeza.X, oColisionTablero.pLimiteBottom.Y - 1);
            
            if (pCabeza.Y >= oColisionTablero.pLimiteBottom.Y)
                pCabeza = new Point(pCabeza.X, oColisionTablero.pLimiteTop.Y + 1);
        }

        private void ColisionesRecompensa()
        {
            if(pCabeza == oRecompensaColision.Posicion)
            {
                if (!oRecompensaColision.GenerarRecompensa(this))
                {
                    estaViva = false;
                    oColisionTablero.FinJuego("H A S   G A N A D O");
                }
                bObteniendoRecompensa = true;
                Puntaje++;
                if(Puntaje > PuntajeMax)
                    PuntajeMax = Puntaje;
            }
        }

        private bool ColisionesCuerpo() 
        {
            foreach(Point item in lstCuerpo)
            {
                if(pCabeza == item)
                {
                    estaViva = false;
                    return true;
                }
            }
            return false;
        }

        public void Muerte() 
        {
            Console.ForegroundColor = oColorCuerpo;
            foreach (Point item in lstCuerpo)
            {
                if (item == pCabeza)
                    continue;
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write("░");
                Thread.Sleep(150);
            }
        }

        public void Informacion(int nDistanciaX, int nDistanciaX2) 
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(oColisionTablero.pLimiteTop.X +  nDistanciaX, oColisionTablero.pLimiteTop.Y - 1 );
            Console.Write("Puntos: " + Puntaje + "  ");
            Console.SetCursorPosition(oColisionTablero.pLimiteTop.X + nDistanciaX2, oColisionTablero.pLimiteTop.Y - 1);
            Console.Write("Record: " + PuntajeMax + "  ");
        }
    }
}
