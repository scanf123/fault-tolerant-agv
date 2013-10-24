using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

//namespace FaultTolerantAGV
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

            //TODO: Implementar calculo de Secantes
            double a = (1 + Math.Pow(r1.M, 2));
            //double b = (2 * r1.P1.Y * r1.M) - (2 * _c.X) - (2 * r1.M * r1.P1.X) - (2 * r1.M * _c.Y) - (2 * _c.X);
            double b = (2 * r1.P1.Y * r1.M) - (2 * _c.X) - (2 * (Math.Pow(r1.M, 2)) * r1.P1.X) - (2 * r1.P1.Y * r1.M);
            //double c = (Math.Pow(r1.M, 2) * Math.Pow(r1.P1.X, 2)) - (2 * r1.P1.Y * r1.M * r1.P1.X) + (Math.Pow(r1.P1.Y, 2)) + (2 * r1.M * r1.P1.X * _c.Y) - (2 * r1.P1.Y * _c.Y) - (Math.Pow(_r, 2));
            double c = (Math.Pow(_c.X, 2)) + (Math.Pow(r1.P1.Y, 2)) - (2 * r1.P1.Y * r1.P1.X * r1.M) + (r1.P1.X * (Math.Pow(r1.M, 2))) - (2 * (Math.Pow(r1.P1.Y, 2))) + (2 * r1.P1.Y * r1.P1.X * r1.M) + (Math.Pow(_c.Y, 2)) - (Math.Pow(_r, 2));
            double delta = Math.Pow(b, 2) - (4 * a * c);
            if (delta > 0)
            {
                if (a != 0)
                {
                    x1 = (((-1 * b) + (Math.Sqrt(delta))) / (2 * a));
                    x2 = (((-1 * b) - (Math.Sqrt(delta))) / (2 * a));
                }
            }

            double[] arrRetorno = new double[2];
            arrRetorno[0] = x1;
            arrRetorno[1] = x2;

            return arrRetorno;
        }

    }
}
