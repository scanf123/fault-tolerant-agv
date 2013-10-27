using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AGVFaultTolerant
{
    public class Reta
    {
        #region Atributos

        private bool _retaVertical;

        public bool RetaVertical
        {
            get { return _retaVertical; }
            set { _retaVertical = value; }
        }
        private bool _retaHorizontal;

        public bool RetaHorizontal
        {
            get { return _retaHorizontal; }
            set { _retaHorizontal = value; }
        }
        private double _m;
        private double _b;

        private Point _p1, _p2;

        #endregion

        #region Propriedades

        public double B
        {
            get { return _b; }
            set { SetaB(value); }
        }

        public double M
        {
            get { return _m; }
            set { SetaA(value); }
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
            _retaHorizontal = false;
            if (m != null)
            {
                this._m = (double)m;
                if (_m == 0)
                    _retaVertical = true;
                this._p1 = p;
                if (_retaVertical)
                    this._b = 0;
                else
                    this._b = this._p1.Y - (this._m * this._p1.X);
            }
            if (angleGraus != null)
            {
                this._m = CalculaMPorAngle((double)angleGraus);
                this._p1 = p;
                this._b = this._p1.Y - (this._m * this._p1.X);
            }
        }


        public Reta(Point p1, Point p2)
        {
            _retaVertical = false;
            _retaHorizontal = false;
            this._p1 = p1;
            this._p1 = p2;
            this._m = CalculaM(p1, p2);
            this._b = (double)this._p1.Y - (this._m * (double)this._p1.X);
        }

        #endregion

        #region Métodos

        private void SetaB(double value)
        {
            this._b = value;
        }

        private void SetaA(double value)
        {
            this._m = value;
        }


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
            else if ((p2.Y - p1.Y) == 0)
            {
                _retaHorizontal = true;
                m = -1;
            }
            else
            {
                m = (double)(p2.Y - p1.Y) / (double)(p2.X - p1.X);
                _retaVertical = false;
                _retaHorizontal = false;
            }
            return m;
        }

        public double CalculaMPerpendicularAReta()
        {
            if (!_retaVertical && !_retaHorizontal)
                return (double)((double)-1 / (double)_m);
            else if (_retaHorizontal)
                return 0;
            else if (_retaVertical)
                return 1;
            return 0;
            //Ex:
            //Retas horizontais
            //y – y0 = m (x – x0)
            //y - k = 0 (x – 0)
            //y – k = 0 – 0
            //y = k

            //Retas Verticais 
            //x = k.


        }

        public double CalculaValorY(double x)
        {
            double y = 0;
            if (!_retaVertical)
                y = ((this._m * x) + this._b);
            else
                y = P1.Y;

            return y;
        }

        public double CalculaValorX(double y)
        {
            double x = 0;
            if (!_retaVertical)
                x = ((y - this._b) / this._m);
            else
                x = P1.X;

            return x;
        }


        #endregion




    }
}
