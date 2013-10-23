using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FaultTolerantAGV
{
    public class Reta
    {
        #region Atributos

        private double _m;
        private Point _p1, _p2;

        #endregion

        #region Propriedades

        public double M
        {
            get { return _m; }
            set { _m = value; }
        }

        public Point P2
        {
            get { return _p2; }
            set { _p2 = value; }
        }

        public Point P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        #endregion

        #region Construtores

        public Reta(double m, Point p)
        {
            this._m = m;
            this._p1 = p;
        }

        public Reta(double angleGraus, Point p)
        {
            this._m = CalculaMPorAngle(angleGraus);
            this._p1 = p;
        }


        public Reta(Point p1, Point p2)
        {
            this._p1 = p1;
            this._p1 = p2;
            this._m = CalculaM(p1, p2);
        }

        #endregion

        #region Métodos

        private double CalculaMPorAngle(double angle)
        {
            double m;
            double angleRad = ((angle + 90) * Math.PI) / 180;
            m = Math.Tan(angleRad);
            return m;
        }

        private double CalculaM(Point p1, Point p2)
        {
            double m;
            if (p1 == null || p2 == null)
                throw new Exception("Antes de calcular m, é necessário atribuir p1, e p2");
            m = (p2.Y - p1.Y) / (p2.X - p2.X);
            return m;
        }

        public double CalculaMPerpendicularAReta()
        {
            return _m * -1;
        }

        public double CalculaValorY(double x)
        {
            double y = 0;
            y = (_m * (_p2.X - _p1.X)) + P1.Y;
            return y;
        }

        public double CalculaValorX(double y)
        {
            double x = 0;
            x = ((_p2.Y - P1.Y) / _m) + P1.X;
            return x;
        }


        #endregion




    }
}
