using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FuzzyAGV
{
    public class IntReta
    {
        #region Atributos

        private double _x0, _y0, _x1, _y1, _m;

        #endregion

        #region Propriedades

        public double M
        {
            get { return _m; }
            set { _m = value; }
        }

        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }

        public double X1
        {
            get { return _x1; }
            set { _x1 = value; }
        }

        public double Y0
        {
            get { return _y0; }
            set { _y0 = value; }
        }

        public double X0
        {
            get { return _x0; }
            set { _x0 = value; }
        }

        public IntReta()
        {
            X0 = 0;
            Y0 = 0;
            X1 = 0;
            Y1 = 0;
        }


        #endregion

        #region Construtores

        public IntReta(double x0, double y0, double x1, double y1)
        {
            X0 = x0;
            Y0 = y0;
            X1 = x1;
            Y1 = y1;
        }

        #endregion

        #region Métodos

        public void CalculaCoefReta()
        {
            if ((X1 - X0) != 0)
                M = (Y1 - Y0) / (X1 - X0);
        }

        public double Evaluate(double x)
        {
            double y;


            y = Y0 + (M * (x - X0));
            return y;
        }

        #endregion
        
    }
}
