using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

//namespace FaultTolerantAGV
namespace AGVFaultTolerant
{
    public class Reta
    {
        #region Atributos

        private bool _retaVertical;
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

        /// <summary>
        /// Classe que representa uma reta
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <param name="angleGraus">Angulo em Graus</param>
        public Reta(double? m, Point p, double? angleGraus)
        {
            _retaVertical = false;
            if (m != null)
            {
                this._m = (double)m;
                this._p1 = p;
            }
            if (angleGraus != null)
            {
                this._m = CalculaMPorAngle((double)angleGraus);
                this._p1 = p;
            }
        }


        public Reta(Point p1, Point p2)
        {
            _retaVertical = false;
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
            if ((p2.X - p1.X) == 0)
            {
                //Reta vertical
                _retaVertical = true;
                m = -1;
            }
            else
            {
                m = (p2.Y - p1.Y) / (p2.X - p1.X);
                _retaVertical = false;
            }
            return m;
        }

        public double CalculaMPerpendicularAReta()
        {
            if (!_retaVertical)
                return _m * -1;
            else
                return 0;
        }

        public double CalculaValorY(double x)
        {
            double y = 0;
            if (!_retaVertical)
                y = (_m * (_p2.X - _p1.X)) + P1.Y;

            return y;
        }

        public double CalculaValorX(double y)
        {
            double x = 0;
            if (!_retaVertical)
                x = ((_p2.Y - P1.Y) / _m) + P1.X;
            else
                x = P1.X;

            return x;
        }


        #endregion




    }
}
