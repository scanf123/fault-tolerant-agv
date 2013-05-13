using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FuzzyAGV
{
    public class Linguistica
    {
        #region Atributos

        #region BackupAtributos
        //private List<ConjuntoNebuloso> _lstConjuntoN;
        //private double _intervalo;
        #endregion
        
        private double _vlNumerico;
        private string _nome;
        private Dictionary<string, ConjuntoNebuloso> _conjuntos;

        #endregion

        #region Construtores

        #region Backup COnstrutor

        //public Linguistica()
        //{
        //    _lstConjuntoN = new List<ConjuntoNebuloso>();
        //    _intervalo = 0.1;

        //}

        //public Linguistica(List<ConjuntoNebuloso> conjunto)
        //{
        //    _lstConjuntoN = conjunto;
        //    _intervalo = 0.1;
        //}

        #endregion

        public Linguistica(string nome)
        {
            this._nome = nome;
            _conjuntos = new Dictionary<string, ConjuntoNebuloso>();
        }

        #endregion

        #region Propriedades

        #region backup Propriedade

        //public double IntervaloConsLing
        //{
        //    get { return _intervalo; }
        //    set { _intervalo = value; }
        //}

        //public List<ConjuntoNebuloso> LstConjuntoN
        //{
        //    get { return _lstConjuntoN; }
        //    set { _lstConjuntoN = value; }
        //}

        #endregion        

        public double VlNumerico
        {
            get { return _vlNumerico; }
            set { _vlNumerico = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        internal Dictionary<string, ConjuntoNebuloso>  Conjuntos
        {
            get { return _conjuntos; }
            //set { _conjuntos= value; }
        }


        #endregion

        #region Métodos

        #region BackupMetodos

        //public string Significado(double x)
        //{
        //    string strRetorno = String.Empty;
        //    double y;

        //    foreach (ConjuntoNebuloso c in _lstConjuntoN)
        //    {
        //        y = 0;

        //        y = c.Pertinencia(x);
        //        if (y > 0)
        //            strRetorno = strRetorno + c.Nome + " - " + y + " ";
        //    }

        //    return strRetorno;
        //}


        //public bool VarConsistente(double x)
        //{
        //    double retorno = 0;
        //    double y;

        //    foreach (ConjuntoNebuloso c in _lstConjuntoN)
        //    {
        //        y = 0;

        //        y = c.Pertinencia(x);
        //        if (y > 0)
        //            retorno = retorno + y;
        //    }

        //    return (retorno == 1);
        //}

        //public bool LingConsistente()
        //{
        //    bool verificacao = true;

        //    for (double d = 0; d <= 50; d += _intervalo)
        //    {
        //        verificacao = verificacao && VarConsistente(d);
        //    }

        //    return verificacao;
        //}        


        #endregion

        
        #endregion
                
    }
}
