using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//namespace QuadradoSoma
namespace FuzzyAGV
{

    /// <summary>
    /// Classe que representa um algoritmo gen�tico para problema das oito rainhas
    /// </summary>
    public class GA
    {
        // Tamanho da popula��o
        private int populationSize;
        // N�mero m�ximo de gera��es e gera��o atual 
        private int generations, generation;
        // Taxa de muta��o
        private double mutation_rate;
        // Gerador de n�meros rand�micos
        private static Random random = new Random(DateTime.Now.Millisecond);

        private int[] totLin;
        private int[] totCol;
        private int max;
        private bool solucao = false;



        private int killnumber;

        private List<QSChromosome> chromosomes;
        /// <summary>
        /// Propriedade para expor gera��o
        /// </summary>
        public int Generation
        {
            get { return generation; }
        }

        public bool Solucao
        {
            get { return solucao; }
            set { solucao = value; }
        }


        /// <summary>
        /// Inicializa o algoritmo gen�tico, inicializando a popula��o e o vetor com os fitness.
        /// </summary>
        /// <param name="populationSize"> Tamanho da popula��o </param>
        /// <param name="generations"> Gera��es </param>
        /// <param name="mutation_rate"> Taxa de muta��o </param>
        public GA(int populationSize, int generations, double mutation_rate, int[] Lin, int[] Col, int maxVal)
        {

            this.populationSize = populationSize;
            this.generations = generations;
            this.mutation_rate = mutation_rate;
            this.generation = 0;

            totLin = Lin;
            totCol = Col;
            max = maxVal;

            this.chromosomes = new List<QSChromosome>(populationSize);
            InitializePopulation();
        }

        /// <summary>
        /// Busca a solu��o, criando as sucessivas gera��es da popula��o. 
        /// </summary>
        public QSChromosome FindSolution()
        {
            for (int i = 0; i < generations; i++)
            {
                //Usado para depura��o para acompanhar o fitness do melhor elemento
                if (i % 1000 == 0)
                    i = i;

                generation++;
                Selection(populationSize / 3);
                GenerateChildren();
                QSChromosome c = GetBestIndividual();
                if (c.GetFitness() == 0)
                {
                    solucao = true;
                    return c;
                }
            }
            return GetBestIndividual();
        }


        /// <summary>
        /// Retorna o indiv�duo com o melhor fitness
        /// </summary>
        /// <returns></returns>
        public QSChromosome GetBestIndividual()
        {
            AvaliatePopulation();
            chromosomes.Sort();
            return chromosomes[0];
        }

        /// <summary>
        /// Inicializa a popula��o
        /// </summary>
        public void InitializePopulation()
        {
            for (int i = 0; i < populationSize; i++)
                this.chromosomes.Add(QSChromosome.CreateRandomChromosome(totLin, totCol, max));
        }

        /// <summary>
        /// Avalia a popula��o calculando o fitness de todos os indiv�duos
        /// </summary>
        public void AvaliatePopulation()
        {
            foreach (QSChromosome c in this.chromosomes)
                c.GetFitness();
        }

        /// <summary>
        /// Opera��o de sele��o, que elimina um n�mero de indiv�duos da popula��o. 
        /// </summary>
        /// <param name="killnumber"> Quantos indiv�duos devem ser eliminados. </param>
        public void Selection(int killnumber)
        {
            int countKill = killnumber;
            this.killnumber = killnumber;

            AvaliatePopulation();
            chromosomes.Sort();

            //QSChromosome c;
            //int i = chromosomes.Count - 1;

            while (countKill > 0)
            {
                MetodoDoTorneio();
                countKill -= 2;


                //Sorteio proporcional ao fitnes
                //fitness, mais prox. de 0 , dificuldade de ser sorteado
                //c = MetodoDaRoleta();
                //this.chromosomes.Remove(c);
                //countKill--;                

                ////Extremamente mais r�pido
                //c = this.chromosomes[i];
                ////Compara o individuo com uma valor randomico, se for maior descarta-o
                //if (c.Fitness >= random.Next(max))
                //{
                //    this.chromosomes.Remove(c);
                //    countKill--;
                //}
                //if (i > 0)
                //    i--;
                //else
                //    i = chromosomes.Count - 1;
            }
            if (countKill < 0)
                countKill = countKill;
        }

        /// <summary>
        /// Gerando os filhos da popula��o atual utilizando operadores de crossover e muta��o.
        /// Para controlar o tamanho da popula��o, o n�mero de filhos gerados n�o deve ultrapassar
        ///  o de indiv�duos eliminados pela sele��o.
        /// </summary>
        public void GenerateChildren()
        {
            if (this.chromosomes.Count > 0)
            {
                //Repete p/ nao diminuir a popula��o
                while (populationSize > this.chromosomes.Count)
                {

                    for (int i = 0; i < (killnumber); i++)
                    {
                        int index = -1;

                        do { index = random.Next(populationSize - killnumber + i); } while (index >= this.chromosomes.Count);
                        QSChromosome pai = this.chromosomes[index];
                        do { index = random.Next(populationSize - killnumber + i); } while (index >= this.chromosomes.Count);
                        QSChromosome mae = this.chromosomes[index];
                        QSChromosome nenem = pai.Crossover(mae);

                        nenem.Mutate(mutation_rate);

                        this.chromosomes.Add(nenem);
                    }

                }
            }
        }


        #region M�todos de sele��o

        /// <summary>
        /// Escolhe randomicamente e sem repeti�a� 3 individuos, o mais apto sobrevive, os outros 2 s�o descartados da popula��o
        /// </summary>
        private void MetodoDoTorneio()
        {
            QSChromosome c1, c2, c3;
            int[] listaSorteados = { -1, -1, -1 };
            int ctIndex = 0;
            if (this.chromosomes.Count > 0)
            {

                while (ctIndex < 3)
                {
                    //Inicialmente n�o ha repeticao de chromossomo escolhido
                    int indexChromo = -1;
                    bool repetido;
                    do
                    {
                        repetido = false;
                        //Pega um chromossomo random
                        indexChromo = random.Next((chromosomes.Count - 1));

                        //Verifica repeti��o de item
                        for (int i = 0; i < 3; i++)
                            if (listaSorteados[i] == indexChromo) repetido = true;

                        if (this.chromosomes.Count <= 3) repetido = false;
                    } while (repetido);

                    //Adiciona item a lista de sorteados
                    listaSorteados[ctIndex] = indexChromo;
                    ctIndex++;

                }

                //Escolhido tres individuos randomicamente sem repeti��o, faz-se o torneio
                c1 = this.chromosomes[listaSorteados[0]];
                c2 = this.chromosomes[listaSorteados[1]];
                c3 = this.chromosomes[listaSorteados[2]];

                if (c1.GetFitness() <= c2.GetFitness() && c1.GetFitness() <= c3.GetFitness())
                {
                    //c1 � o menor
                    this.chromosomes.Remove(c2);
                    this.chromosomes.Remove(c3);
                }
                if (c2.GetFitness() <= c1.GetFitness() && c2.GetFitness() <= c3.GetFitness())
                {
                    //c2 � o menor
                    this.chromosomes.Remove(c1);
                    this.chromosomes.Remove(c3);
                }
                if (c3.GetFitness() <= c2.GetFitness() && c3.GetFitness() <= c1.GetFitness())
                {
                    //c3 � o menor
                    this.chromosomes.Remove(c2);
                    this.chromosomes.Remove(c1);
                }
            }
        }

        /// <summary>
        /// Cada candidato possui uma fatia da �roleta� proporcional a sua aptid�o
        /// </summary>
        /// <returns></returns>
        private QSChromosome MetodoDaRoleta()
        {

            Hashtable htLista = new Hashtable();

            //Cada item sera adicionado a lista "fitness" vezes, p/ que tenha maior chance de ser sorteado
            Int64 htct = 0;
            //for (Int32 i = 0; i < (populationSize-1); i++)
            for (Int32 i = 0; i < (this.chromosomes.Count); i++)
            {
                for (Int32 j = 0; j < this.chromosomes[i].GetFitness(); j++)
                {
                    //Key               |Value
                    //Indice na roleta |Indice no "chromosomes"
                    htLista.Add(htct, i.ToString());
                    htct++;
                    j++;
                }
                i++;
            }

            //Retorna um item randomico(com limita��o do tamanho do vetor de sorteio)            
            //Int64 index = (int)htLista[random.Next((int)htct)];
            Int64 keyRoleta = Convert.ToInt64(random.Next((int)(htct - 1)).ToString());

            //Pega seu valor, que o indice de onde se localiza em "this.chromosomes"
            Int32 index = Convert.ToInt32(htLista[(Int64)keyRoleta].ToString());

            return this.chromosomes[index];
        }

        #endregion

    }
}
