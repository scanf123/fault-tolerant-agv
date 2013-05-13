using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FuzzyAGV
{
    public class Clausula
    {
        #region Atributos

        private Dictionary<string, Linguistica> _contexto;
        private Linguistica _variavel;
        private ConjuntoNebuloso _conjunto;

        #endregion


        public Linguistica Variavel
        {
            get { return _variavel; }
            set { _variavel = value; }
        }

        public ConjuntoNebuloso Conjunto
        {
            get { return _conjunto; }
            set { _conjunto = value; }
        }

        #region Construtores

        public Clausula(Dictionary<string, Linguistica> contexto, string variavel, string conjunto)
        {
            this._contexto = contexto;
            this._variavel = contexto[variavel];
            this._conjunto = this._variavel.Conjuntos[conjunto];
        }

        #endregion

        #region Métodos

        public double Confianca()
        {
            return _conjunto.Pertinencia(_variavel.VlNumerico);
        }

        #endregion
        


    }
}
