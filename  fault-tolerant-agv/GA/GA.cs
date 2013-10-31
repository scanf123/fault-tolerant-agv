using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//namespace QuadradoSoma
namespace AGVFaultTolerant
{

    /// <summary>
    /// Classe que representa um algoritmo genético para problema das oito rainhas
    /// </summary>
    public class GA
    {
        // Tamanho da população
        private int populationSize;

        public int PopulationSize
        {
            get { return populationSize; }
            set { populationSize = value; }
        }
        //Numero de vezes que será colnado a população para gerar clones mutados 
        //e geração atual 
        private int clonepopulation, generation;

        /// <summary>
        //Numero de vezes que será colnado a população para gerar clones mutados 
        /// </summary>
        public int Clonepopulation
        {
            get { return clonepopulation; }
            set { clonepopulation = value; }
        }

        //_distance - position counter two motors
        //_time - value of the IO period counter
        private double _distance, _time;

        // Taxa de mutação, o calculo do mutation_rate é feito de forma dinamica e depende do fitness
        //private double mutation_rate;
        // Gerador de números randômicos
        private static Random random = new Random(DateTime.Now.Millisecond);

        private bool solucao = false;

        public bool Solucao
        {
            get { return solucao; }
            set { solucao = value; }
        }

        public double Time
        {
            get { return _time; }
            //Replica para os cromossomos
            set { _time = value; if (chromosomes.Count > 0) foreach (CircuitoChromosome q in chromosomes) q.Time = _time; }
        }

        public double Distance
        {
            get { return _distance; }
            //Replica para os cromossomos
            set { _distance = value; if (chromosomes.Count > 0) foreach (CircuitoChromosome q in chromosomes) q.Distance = _distance; }
        }
        private int killnumber;
        //private int _mi;
        private int _k;//Constant to be turned by the user (utilizada no calculo do numero de bits a ser mutado)


        private List<CircuitoChromosome> chromosomes;

        /// <summary>
        /// Propriedade para expor geração
        /// </summary>
        public int Generation
        {
            get { return generation; }
        }


        /// <summary>
        /// Numero de vezes que será colnado a população para gerar filhos mutados
        /// </summary>
        //public int Mi
        //{
        //    get { return _mi; }
        //    set { _mi = value; }
        //}

        /// <summary>
        /// Constant to be turned by the user (utilizada no calculo do numero de bits a ser mutado)
        /// </summary>
        public int K
        {
            get { return _k; }
            set { _k = value; }
        }



        /// <summary>
        /// Inicializa o algoritmo genético, inicializando a população e o vetor com os fitness.
        /// </summary>
        /// <param name="populationSize"> Tamanho da população </param>
        /// <param name="clones"> Número de clones </param>
        /// <param name="mutation_rate"> Taxa de mutação </param>
        public GA(int populationSize, int clones, bool[] sensors)
        {
            this.populationSize = populationSize;
            this.clonepopulation = clones;
            this.generation = 0;

            this.chromosomes = new List<CircuitoChromosome>(populationSize);
            InitializePopulation(sensors);
        }


        public void FindSolution()
        {
            generation++;
            Evaluate();
            //GenerateChildren();
            //Os Clones são cridos e avaliados dinamicamente (cda qual é implementado por 140 ms)
            //QSChromosome c = GetBestIndividual();           

            //Apenas o melhor individuo será mantido
            Selecao();
            //Selection(populationSize - 1);
            //Atribui ao Pai, os melhores individuos dos clones (se houverem mudanças para melhor no circuito)
            //return GetBestIndividual();
        }

        private void Selecao()
        {

            AvaliatePopulation();

            //ordena clones
            foreach (CircuitoChromosome c in chromosomes)
            {
                c.Clones.Sort();
            }
            chromosomes.Sort();

            //seleciona apenas os melhores para compor a proxima geração de pais
            List<CircuitoChromosome> lstTmp = new List<CircuitoChromosome>();
            foreach (CircuitoChromosome c in chromosomes)
            {
                lstTmp.Add(c.Clones[0]);
            }
            lstTmp.Add(chromosomes[0]);

            lstTmp.Sort();
            for (int i = 0; i < populationSize; i++)
            {
                //Apenas o melhor individuo será usado como modelo para a proxima geraçãos
                //Utilização de eletismos
                chromosomes[i] = lstTmp[0];
            }

        }

        private void Evaluate()
        {
            //Avalia população de pais
            foreach (CircuitoChromosome c in this.chromosomes)
            {
                //Avalia população de clones
                foreach (CircuitoChromosome clone in c.Clones)
                {
                    clone.GetFitness();
                }
                c.GetFitness();
            }
        }


        /// <summary>
        /// Retorna o indivíduo com o melhor fitness
        /// </summary>
        /// <returns></returns>
        public CircuitoChromosome GetBestIndividual()
        {
            AvaliatePopulation();
            chromosomes.Sort();
            return chromosomes[0];
        }

        /// <summary>
        /// Inicializa a população
        /// </summary>
        //public void InitializePopulation()
        //{
        //    for (int i = 0; i < populationSize; i++)
        //        this.chromosomes.Add(QSChromosome.CreateRandomChromosome());
        //}

        public void InitializePopulation(bool[] sensors)
        {
            for (int i = 0; i < populationSize; i++)
                this.chromosomes.Add(CircuitoChromosome.CreateRandomChromosome(sensors));
        }

        /// <summary>
        /// Avalia a população calculando o fitness de todos os indivíduos
        /// </summary>
        public void AvaliatePopulation()
        {
            foreach (CircuitoChromosome c in this.chromosomes)
            {
                foreach (CircuitoChromosome clone in c.Clones)
                {
                    clone.GetFitness();
                }
                c.GetFitness();
            }
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

            //ordena clones
            foreach (CircuitoChromosome c in chromosomes)
            {
                c.Clones.Sort();
            }
            chromosomes.Sort();

            //seleciona apenas os melhores para compor a proxima geração de pais
            List<CircuitoChromosome> lstTmp = new List<CircuitoChromosome>();
            foreach (CircuitoChromosome c in chromosomes)
            {
                lstTmp.Add(c.Clones[0]);
            }
            lstTmp.Add(chromosomes[0]);

            lstTmp.Sort();
            for (int i = 0; i < populationSize; i++)
            {
                chromosomes[i] = lstTmp[i];
            }



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
                CircuitoChromosome c = GetBestIndividual();
                double fit = c.GetFitness();
                int norm_fi = (int)Math.Truncate(fit);
                List<CircuitoChromosome> lstClonedMutated = new List<CircuitoChromosome>(); ;

                List<CircuitoChromosome> Cloned;
                //Clona _mi vezes a população inicial
                //for (int i = 0; i < (_mi); i++)
                for (int i = 0; i < (this.clonepopulation); i++)
                {
                    Cloned = new List<CircuitoChromosome>();
                    Cloned.AddRange(this.chromosomes.ToArray());
                    for (int j = 0; j < Cloned.Count; j++)
                    {
                        //Aplica mutação no individuo
                        Cloned[i].Mutate(_k, norm_fi);
                    }
                    //Adiciona os clones mutados a população atual
                    lstClonedMutated.AddRange(Cloned.ToArray());
                }
                this.chromosomes.AddRange(lstClonedMutated.ToArray());
            }
        }


        #region Métodos de seleção

        /// <summary>
        /// Escolhe randomicamente e sem repetiçaõ 3 individuos, o mais apto sobrevive, os outros 2 são descartados da população
        /// </summary>
        private void MetodoDoTorneio()
        {
            CircuitoChromosome c1, c2, c3;
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

                if (c1.GetFitness() >= c2.GetFitness() && c1.GetFitness() >= c3.GetFitness())
                {
                    //c1 é o melhor
                    this.chromosomes.Remove(c2);
                    this.chromosomes.Remove(c3);
                }
                if (c2.GetFitness() >= c1.GetFitness() && c2.GetFitness() >= c3.GetFitness())
                {
                    //c2 é o melhor
                    this.chromosomes.Remove(c1);
                    this.chromosomes.Remove(c3);
                }
                if (c3.GetFitness() >= c2.GetFitness() && c3.GetFitness() >= c1.GetFitness())
                {
                    //c3 é o melhor
                    this.chromosomes.Remove(c2);
                    this.chromosomes.Remove(c1);
                }
            }
        }

        /// <summary>
        /// Cada candidato possui uma fatia da “roleta” proporcional a sua aptidão
        /// </summary>
        /// <returns></returns>
        private CircuitoChromosome MetodoDaRoleta()
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


        public CircuitoChromosome GetCurrentChromosome(int parent)
        {
            return chromosomes[parent];
        }

        public CircuitoChromosome GetCurrentCloneChromosome(int ctControleParent, int ctControleClones)
        {
            return chromosomes[ctControleParent].Clones[ctControleClones];
        }

        public void InitializeClones(int ctControleParent, bool[] sensors)
        {
            for (int i = 0; i < clonepopulation; i++)
                this.chromosomes[ctControleParent].Clones.Add(CircuitoChromosome.CreateRandomClone(sensors, this.chromosomes[ctControleParent].GetFitness(), this.chromosomes[ctControleParent].Cgp.Genotype, this._k));

        }
    }
}

