using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AGVFaultTolerant
{
    public class Circulo
    {
        private Point _c;

        private double _r;

        public double R
        {
            get { return _r; }
            set { _r = value; }
        }
        public Point C
        {
            get { return _c; }
            set { _c = value; }
        }

        public Circulo(Point c, double r)
        {
            this._c = c;
            this._r = r;
        }

        public double[] CalculaValorXSecanteCircuferencia(Reta r1)
        {
            double x1, x2;
            x1 = x2 = 0;
            if (r1.RetaVertical)
            {
                x1 = x2 = r1.P1.X;
            }
            else
            {
                double a = (1 + Math.Pow(r1.M, 2));
                double b = (2 * r1.M * r1.B) - (2 * r1.M * this._c.Y) - (2 * this._c.X);
                double c = (Math.Pow(this._c.X, 2)) + (Math.Pow(r1.B, 2)) + (Math.Pow(this._c.Y, 2)) - (2 * r1.B * this._c.Y) - (Math.Pow(this._r, 2));
                double delta = Math.Pow(b, 2) - (4 * a * c);
                if (delta > 0)
                {
                    if (a != 0)
                    {
                        x1 = (((-1 * b) + (Math.Sqrt(delta))) / (2 * a));
                        x2 = (((-1 * b) - (Math.Sqrt(delta))) / (2 * a));
                    }
                }
            }

            double[] arrRetorno = new double[2];
            arrRetorno[0] = x1;
            arrRetorno[1] = x2;

            return arrRetorno;
        }


        internal double[] CalculaValorY(double x)
        {
            double y1, y2;
            y1 = y2 = 0;

            double a = 1;
            double b = -2 * _c.Y;
            double c = Math.Pow(_c.Y, 2) + (Math.Pow(x, 2) - (2 * x * _c.X) + Math.Pow(_c.X, 2)) - Math.Pow(_r, 2);
            double delta = Math.Pow(b, 2) - (4 * a * c);
            if (delta > 0)
            {
                if (a != 0)
                {
                    y1 = (((-1 * b) + (Math.Sqrt(delta))) / (2 * a));
                    y2 = (((-1 * b) - (Math.Sqrt(delta))) / (2 * a));
                }
            }

            double[] arrRetorno = new double[2];
            arrRetorno[0] = y1;
            arrRetorno[1] = y2;

            return arrRetorno;
        }
    }
}
