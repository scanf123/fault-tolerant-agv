using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FaultTolerantAGV
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

            //x1^2 - 2*x1*xc1 + xc1^2 + ( r1. )

            List<double> lstRetorno = new List<double>();
            lstRetorno.Add(x1);
            lstRetorno.Add(x2);
            return lstRetorno.ToArray();
        }

    }
}
