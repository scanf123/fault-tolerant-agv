using System;
using System.Collections.Generic;
using System.Text;

//namespace QuadradoSoma
namespace FuzzyAGV
{

    /// <summary>
    /// Classe que representa um cromossomo do problema das 8 rainhas
    /// </summary>
    public class QSChromosome : IComparable<QSChromosome>
    {
        // Vetor com os valores do quadrado
        private int[,] valores = null;

        private int[] totLin = null;
        private int[] totCol = null;

        //Valor padr�o gen�rico
        private static int maxVal = 10000;


        // Objeto random para gera��o de n�meros rand�micos
        private static Random random = new Random(DateTime.Now.Millisecond);

        private int fitness;

        // Propriedade que exp�e o vetor para leitura
        public int[,] Valores
        {
            get
            {
                return valores;
            }
        }

        public int[] TotCol
        {
            get { return totCol; }
            set { totCol = value; }
        }

        public int[] TotLin
        {
            get { return totLin; }
            set { totLin = value; }
        }

        public int Fitness
        {
            get { return fitness; }
        }

        public int MaxVal
        {
            get { return maxVal; }
            set { maxVal = value; }
        }


        // Construtor - recebe o vetor com os valores
        public QSChromosome(int[,] vals)
        {
            this.valores = vals;
        }


        /// <summary>
        /// M�todo est�tico que instancia um cromossomo com valores rand�micos com um limite para o valor
        /// </summary>
        /// <param name="lin">Armazena o total da Linha</param>
        /// <param name="col">Armazena o total da coluna</param>
        /// <param name="max">Limite  do Valor para restringir o range do random</param>
        /// <returns></returns>
        public static QSChromosome CreateRandomChromosome(int[] lin, int[] col, int max)
        {
            maxVal = max;

            int[,] c = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int itmp = random.Next(maxVal);
                    c[i, j] = itmp;
                }

            }

            QSChromosome chr = new QSChromosome(c);
            chr.totLin = lin;
            chr.totCol = col;

            return chr;
        }


        /// <summary>
        /// Opera��o de crossover entre dois cromossomos. 
        /// </summary>
        /// <param name="pair"> Recebe um cromossomo (par) que "cruzar�" como atual.  </param>
        /// <returns> Retorna um filho com genes do cromossomo atual e do par. </returns>
        public QSChromosome Crossover(QSChromosome pair)
        {

            int[,] c = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    //Troca linha por coluna se linha mais coluna for par
                    if ((i + j) % 2 == 0)
                        c[i, j] = pair.valores[j, i];
                    else
                        c[j, i] = pair.valores[i, j];
                }
            }

            QSChromosome chr = new QSChromosome(c);
            //Passa a totaliza��o para o filho
            chr.totLin = pair.totLin;
            chr.totCol = pair.totCol;
            //Passa a aproxima��o para o filho
            chr.MaxVal = pair.MaxVal;

            return chr;
        }


        /// <summary>
        /// Muta��o - uma pequena chance de realizar muta��o no cromossomo, mudando um de seus genes.
        /// </summary>
        /// <param name="rate"> Percentual da chance de muta��o, n�mero entre 0 e 1. </param>
        public void Mutate(double rate)
        {
            if (random.NextDouble() <= rate)
                this.valores[random.Next(5), random.Next(5)] = random.Next(maxVal);
        }

        /// <summary>
        /// Fitness, o c�lculo de quanto um cromossomo � bom
        /// </summary>
        /// <returns> Retorna a avalia��o deste cromossomo. Valor "zero" � o cromossomo ideal, o objetivo da busca. </returns>
        public double GetFitness()
        {


            int fit = 0, sumH = 0, sumV = 0;

            for (int i = 0; i < 5; i++)
            {
                sumH = 0;
                sumV = 0;
                for (int j = 0; j < 5; j++)
                {
                    sumH += this.valores[i, j];
                    sumV += this.valores[j, i];
                }
                fit += Math.Abs(totLin[i] - sumH);
                fit += Math.Abs(totCol[i] - sumV);
            }

            fitness = fit;
            return fit;

        }

        public int CompareTo(QSChromosome obj)
        {
            if (this.fitness > obj.fitness)
                return 1;
            else if (this.fitness < obj.fitness)
                return -1;
            else
                return 0;
        }
    }


}


