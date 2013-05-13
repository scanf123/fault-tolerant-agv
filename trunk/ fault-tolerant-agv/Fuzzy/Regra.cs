using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FuzzyAGV
{
    public class Regra
    {
        private List<object> _antecedente;
        private Clausula _consequente;

        public Clausula Consequente
        {
            get { return _consequente; }
            set { _consequente = value; }
        }

        public Regra()
        {
            _antecedente = new List<object>();
        }

        public void AdicionarClausula(Clausula clausula)
        {
            _antecedente.Add(clausula);
        }

        public void AdicionarNorma()
        {
            _antecedente.Add("E");
        }

        public void AdicionarCoNorma()
        {
            _antecedente.Add("OU");
        }

        public double  Confianca()
        {
            Stack<double> s = new Stack<double>();

            //Adiciona as clausulas
            //foreach (object o in _antecedente)
            //{
            //    if (o is Clausula)
            //    {
            //        Clausula c = o as Clausula;
            //        s.Push(c.Confianca());
            //    }
            //}

            //foreach (object o in _antecedente)
            //{
            //    if (o is string)
            //    {
            //        double x = s.Pop();
            //        double y = s.Pop();

            //        if (o.ToString() == "E")
            //        {
            //            //Regra recebe uma classe norma
            //            //e classe norma verifica
            //            s.Push(Math.Min(x, y));
            //        }
            //        else if (o.ToString() == "OU")
            //        {
            //            s.Push(Math.Max(x, y));
            //        }

            //    }
            //}

            foreach (object o in _antecedente)
            {
                if (o is string)
                {
                    double x = s.Pop();
                    double y = s.Pop();

                    if (o.ToString() == "E")
                    {
                        //Regra recebe uma classe norma
                        //e classe norma verifica
                        s.Push(Math.Min(x, y));
                    }
                    else if (o.ToString() == "OU")
                    {
                        s.Push(Math.Max(x, y));
                    }


                }
                else
                {
                    Clausula c = o as Clausula;
                    s.Push(c.Confianca());
                }

            }

            return s.Pop();
        }


    }
}
