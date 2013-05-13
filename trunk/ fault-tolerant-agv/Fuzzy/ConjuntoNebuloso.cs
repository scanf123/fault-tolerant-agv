using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FuzzyAGV
{
    public class ConjuntoNebuloso
    {
        #region Atributos

        private List<IntReta> _lstRetas;
        private string _nome;

        #endregion

        #region Construtores

        public ConjuntoNebuloso()
        {
            LstRetas = new List<IntReta>();
        }

        public ConjuntoNebuloso(string nome):this()
        {
            _nome = nome;
        }

        #endregion

        #region Propriedades

        public List<IntReta> LstRetas
        {
            get { return _lstRetas; }
            set { _lstRetas = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        #endregion

        #region Métodos
        
        public double Pertinencia(double x)
        {
            double xret = 0;
            foreach (IntReta r in LstRetas)
            {
                if (r.X0 <= x && r.X1 >= x)
                {
                    xret = r.Evaluate(x);
                    if (xret > 0 && xret < 1)
                        return xret;
                }
            }
            return xret;
        }

        public void AdicionarReta(double x0, double y0, double x1, double y1)
        {
            IntReta r = new IntReta(x0, y0, x1, y1);
            r.CalculaCoefReta();
            
            LstRetas.Add(r);
        }

        #endregion

    }
}
