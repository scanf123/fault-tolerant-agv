using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FuzzyAGV
{
    public class Defuzz
    {
        public static double Calcular ( Linguistica saida , Dictionary<string , double >  fzzOut, double ini, double fim,  int n)
        {
            double it = (fim - ini) / n;
            double soma1 = 0, soma2 = 0;

            for (double i = ini; i < fim; i += it)
            {
                double confianca = 0;
                double maior = 0;
                foreach (KeyValuePair<string, double> item in fzzOut)
                {
                    //Alterado pois o dictionary contem uma descrição diferente do conjunto
                    //String conjunto = item.Key.Substring(4);
                    confianca = saida.Conjuntos[item.Key].Pertinencia(i);
                    if (confianca > item.Value)
                        confianca = item.Value;
                    if (confianca > maior)
                        maior = confianca;
                }
                soma1 += i * maior;
                soma2 += maior;
            }
            if (soma2 == 0)
                soma2 = 1;
            return soma1 / soma2;

        }
    }
}
