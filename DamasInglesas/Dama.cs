using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DamasInglesas
{
    public partial class Dama : Form
    {
        protected int turno = 0;
        PictureBox seleccionada = null;//Ficha Seleccionada
        List<PictureBox> Rojas = new List<PictureBox>();
        List<PictureBox> Azules = new List<PictureBox>();
        protected bool Extra = false;//Turno Extra

        public Dama()
        {
            InitializeComponent();
            CargarLista();
        }
        //Cargar La lista de fichas
        private void CargarLista()
        {
            Azules.Add(Azul1);
            Azules.Add(Azul2);
            Azules.Add(Azul3);
            Azules.Add(Azul4);
            Azules.Add(Azul5);
            Azules.Add(Azul6);
            Azules.Add(Azul7);
            Azules.Add(Azul8);
            Azules.Add(Azul9);
            Azules.Add(Azul10);
            Azules.Add(Azul11);
            Azules.Add(Azul12);

            Rojas.Add(Roja1);
            Rojas.Add(Roja2);
            Rojas.Add(Roja3);
            Rojas.Add(Roja4);
            Rojas.Add(Roja5);
            Rojas.Add(Roja6);
            Rojas.Add(Roja7);
            Rojas.Add(Roja8);
            Rojas.Add(Roja9);
            Rojas.Add(Roja10);
            Rojas.Add(Roja11);
            Rojas.Add(Roja12);

        }
        /// <summary>
        /// permite al jugar seleccionar una ficha
        /// </summary>
        /// <param name="objeto"></param>
        public void Seleccion(object objeto)
        {
            if(!Extra)
            {
                try
                {
                    seleccionada.BackColor = Color.Black;
                }
                catch
                {

                }

                PictureBox ficha = (PictureBox)objeto;
                seleccionada = ficha;
                seleccionada.BackColor = Color.Cyan;

            }          


        }
                
        private void CuadroClick(object sender, MouseEventArgs e)
        {
            Movimiento((PictureBox)sender);
        }

        private int Promedio(int n1, int n2)
        {
            int resultado = n1 + n2;
            resultado /= 2;
            return Math.Abs(resultado);
        }

        private bool Validar(PictureBox origen, PictureBox destino, string color)
        {
            Point ptOrigen = origen.Location;
            Point ptDestino = destino.Location;
            int avance = ptOrigen.Y - ptDestino.Y;
            avance = color == "Roja" ? avance : (avance * -1);
            avance = seleccionada.Tag == "queen" ? Math.Abs(avance) : avance;

            if(avance == 50)
            {
                return true;
            }
            else
            if(avance == 100)
            {
                Point ptMedio = new Point(Promedio(ptDestino.X,ptOrigen.X), Promedio(ptDestino.Y, ptOrigen.Y));
                List<PictureBox> Contrario = color == "Roja" ? Azules : Rojas;

                for(int i = 0; i < Contrario.Count; ++i)
                {
                    if(Contrario[i].Location == ptMedio)
                    {
                        Contrario[i].Location = new Point(0, 0);
                        Contrario[i].Visible = false;
                        return true;
                    }
                }
            }
            return false;
        }
        //si hay movimientos extra
        private bool MovimientoExtra(string color)
        {
            List<PictureBox> Contrario = color == "Roja" ? Azules : Rojas;
            List<Point> posiciones = new List<Point>();

            int sigte = color == "Roja" ? -100 : 100;

            posiciones.Add(new Point(seleccionada.Location.X + 100, seleccionada.Location.Y + sigte));
            posiciones.Add(new Point(seleccionada.Location.X - 100, seleccionada.Location.Y + sigte));

            if(seleccionada.Tag == "queen")
            {
                posiciones.Add(new Point(seleccionada.Location.X + 100, seleccionada.Location.Y - sigte));
                posiciones.Add(new Point(seleccionada.Location.X - 100, seleccionada.Location.Y - sigte));

            }

            bool resultado = false;

            for (int i = 0; i < posiciones.Count; ++i)
            {
                if(posiciones[i].X >= 50 && posiciones[i].X <= 400 && posiciones[i].Y >= 50 && posiciones[i].Y <= 400)
                {
                    if( !Ocupado(posiciones[i], Rojas) && !Ocupado(posiciones[i],Azules) )
                    {
                        Point ptMedio = new Point(Promedio(posiciones[i].X, seleccionada.Location.X), Promedio(posiciones[i].Y, seleccionada.Location.Y));
                        if(Ocupado(ptMedio, Contrario))
                        {
                            resultado = true;
                        }
                    }
                }
            }


            return resultado;
        }
        //verificar si el cuadro sigte esta ocupado
        private bool Ocupado(Point point, List<PictureBox> contrario)
        {
            for (int i = 0; i < contrario.Count; ++i)
            {
                if(point == contrario[i].Location)
                {
                    return true;
                }
            }
            return false;
        }
        //mover ficha en el cuadro
        private void Movimiento(PictureBox cuadro)
        {
            if(seleccionada != null)
            {
                string color = seleccionada.Name.ToString().Substring(0, 4);

                if (Validar(seleccionada,cuadro,color))//Validacion
                {               
                    Point ant = seleccionada.Location;
                    seleccionada.Location = cuadro.Location;
                    int avance = ant.Y - cuadro.Location.Y;

                    if(!MovimientoExtra(color) | Math.Abs(avance) == 50)//Movimiento Extra
                    {
                        IfQueen(color);
                        turno++;
                        seleccionada.BackColor = Color.Black;
                        seleccionada = null;
                        Extra = false;
                    }
                    else
                    {
                        Extra = true;
                    }

                }
            }
        }
        //Verificar si se convertira en reyna
        private void IfQueen(string color)
        {
            if(color == "Azul" && seleccionada.Location.Y == 400)
            {
                seleccionada.BackgroundImage = Properties.Resources.QueenAzul;
                seleccionada.Tag = "queen";
            }
            else
            if (color == "Roja" && seleccionada.Location.Y == 50)
            {
                seleccionada.BackgroundImage = Properties.Resources.QueenRoja;
                seleccionada.Tag = "queen";
            }

        }

        private void SeleccionRoja(object sender, MouseEventArgs e)
        {
            if (turno % 2 == 0)
            {
                Seleccion(sender);
            }
            else
                MessageBox.Show("Turno Del Azul");
        }

        private void SeleccionAzul(object sender, MouseEventArgs e)
        {
            if (turno % 2 == 1)
            {
                Seleccion(sender);
            }
            else
                MessageBox.Show("Turno Del Rojo");
        }

        private void DamaInglesa_Load(object sender, EventArgs e)
        {

        }
    }
}
