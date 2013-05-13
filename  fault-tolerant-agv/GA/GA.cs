using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//namespace QuadradoSoma
namespace FuzzyAGV
{

    /// <summary>
    /// Classe que representa um algoritmo genético para problema das oito rainhas
    /// </summary>
    public class GA
    {
        // Tamanho da população
        private int populationSize;
        // Número máximo de gerações e geração atual 
        private int generations, generation;
        // Taxa de mutação
        private double mutation_rate;
        // Gerador de números randômicos
        private static Random random = new Random(DateTime.Now.Millisecond);

        private int[] totLin;
        private int[] totCol;
        private int max;
        private bool solucao = false;



        private int killnumber;

        private List<QSChromosome> chromosomes;
        /// <summary>
        /// Propriedade para expor geração
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
        /// Inicializa o algoritmo genético, inicializando a população e o vetor com os fitness.
        /// </summary>
        /// <param name="populationSize"> Tamanho da população </param>
        /// <param name="generations"> Gerações </param>
        /// <param name="mutation_rate"> Taxa de mutação </param>
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
        /// Busca a solução, criando as sucessivas gerações da população. 
        /// </summary>
        public QSChromosome FindSolution()
        {
            for (int i = 0; i < generations; i++)
            {
                //Usado para depuração para acompanhar o fitness do melhor elemento
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
        /// Retorna o indivíduo com o melhor fitness
        /// </summary>
        /// <returns></returns>
        public QSChromosome GetBestIndividual()
        {
            AvaliatePopulation();
            chromosomes.Sort();
            return chromosomes[0];
        }

        /// <summary>
        /// Inicializa a população
        /// </summary>
        public void InitializePopulation()
        {
            for (int i = 0; i < populationSize; i++)
                this.chromosomes.Add(QSChromosome.CreateRandomChromosome(totLin, totCol, max));
        }

        /// <summary>
        /// Avalia a população calculando o fitness de todos os indivíduos
        /// </summary>
        public void AvaliatePopulation()
        {
            foreach (QSChromosome c in this.chromosomes)
                c.GetFitness();
        }

        /// <summary>
        /// Operação de seleção, que elimina um número de indivíduos da população. 
        /// </summary>
        /// <param name="killnumber"> Quantos indivíduos devem ser eliminados. </param>
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

                ////Extremamente mais rápido
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
        /// Gerando os filhos da população atual utilizando operadores de crossover e mutação.
        /// Para controlar o tamanho da população, o número de filhos gerados não deve ultrapassar
        ///  o de indivíduos eliminados pela seleção.
        /// </summary>
        public void GenerateChildren()
        {
            if (this.chromosomes.Count > 0)
            {
                //Repete p/ nao diminuir a população
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


        #region Métodos de seleção

        /// <summary>
        /// Escolhe randomicamente e sem repetiçaõ 3 individuos, o mais apto sobrevive, os outros 2 são descartados da população
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
                    //Inicialmente não ha repeticao de chromossomo escolhido
                    int indexChromo = -1;
                    bool repetido;
                    do
                    {
                        repetido = false;
                        //Pega um chromossomo random
                        indexChromo = random.Next((chromosomes.Count - 1));

                        //Verifica repetição de item
                        for (int i = 0; i < 3; i++)
                            if (listaSorteados[i] == indexChromo) repetido = true;

                        if (this.chromosomes.Count <= 3) repetido = false;
                    } while (repetido);

                    //Adiciona item a lista de sorteados
                    listaSorteados[ctIndex] = indexChromo;
                    ctIndex++;

                }

                //Escolhido tres individuos randomicamente sem repetição, faz-se o torneio
                c1 = this.chromosomes[listaSorteados[0]];
                c2 = this.chromosomes[listaSorteados[1]];
                c3 = this.chromosomes[listaSorteados[2]];

                if (c1.GetFitness() <= c2.GetFitness() && c1.GetFitness() <= c3.GetFitness())
                {
                    //c1 é o menor
                    this.chromosomes.Remove(c2);
                    this.chromosomes.Remove(c3);
                }
                if (c2.GetFitness() <= c1.GetFitness() && c2.GetFitness() <= c3.GetFitness())
                {
                    //c2 é o menor
                    this.chromosomes.Remove(c1);
                    this.chromosomes.Remove(c3);
                }
                if (c3.GetFitness() <= c2.GetFitness() && c3.GetFitness() <= c1.GetFitness())
                {
                    //c3 é o menor
                    this.chromosomes.Remove(c2);
                    this.chromosomes.Remove(c1);
                }
            }
        }

        /// <summary>
        /// Cada candidato possui uma fatia da “roleta” proporcional a sua aptidão
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

            //Retorna um item randomico(com limitação do tamanho do vetor de sorteio)            
            //Int64 index = (int)htLista[random.Next((int)htct)];
            Int64 keyRoleta = Convert.ToInt64(random.Next((int)(htct - 1)).ToString());

            //Pega seu valor, que o indice de onde se localiza em "this.chromosomes"
            Int32 index = Convert.ToInt32(htLista[(Int64)keyRoleta].ToString());

            return this.chromosomes[index];
        }

        #endregion

    }
}
