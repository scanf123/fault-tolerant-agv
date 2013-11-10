using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AGVFaultTolerant
{


    /// <summary>
    /// Classe que representa um algoritmo gen�tico para problema das oito rainhas
    /// </summary>
    public class EA
    {
        // Tamanho da popula��o
        private int populationSize;
        private CircuitoChromosome _bestCircuit = null;

        public CircuitoChromosome BestCircuit
        {
            get { return _bestCircuit; }
            set { _bestCircuit = value; }
        }

        public int PopulationSize
        {
            get { return populationSize; }
            set { populationSize = value; }
        }
        //Numero de vezes que ser� colnado a popula��o para gerar clones mutados 
        //e gera��o atual 
        private int clonepopulation, generation;

        /// <summary>
        //Numero de vezes que ser� colnado a popula��o para gerar clones mutados 
        /// </summary>
        public int Clonepopulation
        {
            get { return clonepopulation; }
            set { clonepopulation = value; }
        }

        //_distance - position counter two motors
        //_time - value of the IO period counter
        private double _distance, _time;

        // Taxa de muta��o, o calculo do mutation_rate � feito de forma dinamica e depende do fitness
        //private double mutation_rate;
        // Gerador de n�meros rand�micos
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
            set { _time = value; if (_chromosomes.Count > 0) foreach (CircuitoChromosome q in _chromosomes) q.Time = _time; }
        }

        public double Distance
        {
            get { return _distance; }
            //Replica para os cromossomos
            set { _distance = value; if (_chromosomes.Count > 0) foreach (CircuitoChromosome q in _chromosomes) q.Distance = _distance; }
        }
        private int killnumber;
        //private int _mi;
        private int _k;//Constant to be turned by the user (utilizada no calculo do numero de bits a ser mutado)


        private List<CircuitoChromosome> _chromosomes;

        /// <summary>
        /// Propriedade para expor gera��o
        /// </summary>
        public int Generation
        {
            get { return generation; }
        }

        /// <summary>
        /// Constant to be turned by the user (utilizada no calculo do numero de bits a ser mutado)
        /// </summary>
        public int K
        {
            get { return _k; }
            set { _k = value; }
        }



        /// <summary>
        /// Inicializa o algoritmo gen�tico, inicializando a popula��o e o vetor com os fitness.
        /// </summary>
        /// <param name="populationSize"> Tamanho da popula��o </param>
        /// <param name="clones"> N�mero de clones </param>
        /// <param name="mutation_rate"> Taxa de muta��o </param>
        public EA(int populationSize, int clones, bool[] sensors)
        {
            this.populationSize = populationSize;
            this.clonepopulation = clones;
            this.generation = 0;

            this._chromosomes = new List<CircuitoChromosome>(populationSize);
            InitializePopulation(sensors);
        }


        public void FindSolution()
        {
            generation++;

            //Apenas o melhor individuo ser� mantido
            Selecao();

        }

        public void CalculaNormFit()
        {
            double fitTotalPopulacao = 0;
            //Avalia popula��o de pais
            foreach (CircuitoChromosome c in this._chromosomes)
            {
                fitTotalPopulacao += c.Fitness;
            }

            //Avalia popula��o de pais
            foreach (CircuitoChromosome c in this._chromosomes)
            {
                c.GetNormFitness(fitTotalPopulacao);
                //c.GetNormFitness(fitTotalPopulacao, this._chromosomes.Count);
            }

        }

        private void Selecao()
        {

            //AvaliatePopulation();
            List<CircuitoChromosome> lsttmpClonesPais = new List<CircuitoChromosome>();



            foreach (CircuitoChromosome c in _chromosomes)
            {
                c.Clones.Sort();
            }
            _chromosomes.Sort();


            //seleciona apenas os melhores para compor a proxima gera��o de pais
            List<CircuitoChromosome> lstTmp = new List<CircuitoChromosome>();
            foreach (CircuitoChromosome c in _chromosomes)
            {
                lstTmp.Add(c.Clones[0]);
            }
            lstTmp.Add(_chromosomes[0]);

            lstTmp.Sort();




            //BestCircuit = RetornaListaOrdenadaPorFitness(lsttmpClonesPais)[0];
            BestCircuit = lstTmp[0];
            for (int i = 0; i < populationSize; i++)
            {
                //Apenas o melhor individuo ser� usado como modelo para a proxima gera��os
                //Utiliza��o de eletismos
                _chromosomes[i] = BestCircuit;
                _chromosomes[i].Clones = new List<CircuitoChromosome>();
            }


        }

        private List<CircuitoChromosome> RetornaListaOrdenadaPorFitness(List<CircuitoChromosome> lstCircuitos)
        {
            List<CircuitoChromosome> lstRetorno = new List<CircuitoChromosome>();
            lstRetorno.AddRange(lstCircuitos.ToArray());


            int min;
            CircuitoChromosome aux;
            for (int i = 0; i < lstRetorno.ToArray().Length - 1; i++)
            {
                min = i;

                for (int j = i + 1; j < lstRetorno.ToArray().Length; j++)
                    if (lstRetorno[j].Fitness < lstRetorno[min].Fitness)
                        min = j;

                if (min != i)
                {
                    aux = lstRetorno[min];
                    lstRetorno[min] = lstRetorno[i];
                    lstRetorno[i] = aux;
                }
            }


            return lstRetorno;
        }

        private void Evaluate()
        {
            //Avalia popula��o de pais
            foreach (CircuitoChromosome c in this._chromosomes)
            {
                //Avalia popula��o de clones
                foreach (CircuitoChromosome clone in c.Clones)
                {
                    clone.GetFitness();
                }
                c.GetFitness();
            }
        }


        /// <summary>
        /// Retorna o indiv�duo com o melhor fitness
        /// </summary>
        /// <returns></returns>
        public CircuitoChromosome GetBestIndividual()
        {
            AvaliatePopulation();
            _chromosomes.Sort();
            return _chromosomes[0];
        }


        public void InitializePopulation(bool[] sensors)
        {
            for (int i = 0; i < populationSize; i++)
                this._chromosomes.Add(CircuitoChromosome.CreateRandomChromosome(sensors));
        }

        /// <summary>
        /// Avalia a popula��o calculando o fitness de todos os indiv�duos
        /// </summary>
        public void AvaliatePopulation()
        {
            foreach (CircuitoChromosome c in this._chromosomes)
            {
                foreach (CircuitoChromosome clone in c.Clones)
                {
                    clone.GetFitness();
                }
                c.GetFitness();
            }
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

            //ordena clones
            foreach (CircuitoChromosome c in _chromosomes)
            {
                c.Clones.Sort();
            }
            _chromosomes.Sort();

            //seleciona apenas os melhores para compor a proxima gera��o de pais
            List<CircuitoChromosome> lstTmp = new List<CircuitoChromosome>();
            foreach (CircuitoChromosome c in _chromosomes)
            {
                lstTmp.Add(c.Clones[0]);
            }
            lstTmp.Add(_chromosomes[0]);

            lstTmp.Sort();
            for (int i = 0; i < populationSize; i++)
            {
                _chromosomes[i] = lstTmp[i];
            }



        }


        #region M�todos de sele��o

        /// <summary>
        /// Escolhe randomicamente e sem repeti�a� 3 individuos, o mais apto sobrevive, os outros 2 s�o descartados da popula��o
        /// </summary>
        private void MetodoDoTorneio()
        {
            CircuitoChromosome c1, c2, c3;
            int[] listaSorteados = { -1, -1, -1 };
            int ctIndex = 0;
            if (this._chromosomes.Count > 0)
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
                        indexChromo = random.Next((_chromosomes.Count - 1));

                        //Verifica repeti��o de item                                                
                        for (int i = 0; i < 3; i++)
                            if (listaSorteados[i] == indexChromo) repetido = true;

                        if (this._chromosomes.Count <= 3) repetido = false;
                    } while (repetido);

                    //Adiciona item a lista de sorteados
                    listaSorteados[ctIndex] = indexChromo;
                    ctIndex++;
                }

                //Escolhido tres individuos randomicamente sem repeti��o, faz-se o torneio
                c1 = this._chromosomes[listaSorteados[0]];
                c2 = this._chromosomes[listaSorteados[1]];
                c3 = this._chromosomes[listaSorteados[2]];

                if (c1.GetFitness() >= c2.GetFitness() && c1.GetFitness() >= c3.GetFitness())
                {
                    //c1 � o melhor
                    this._chromosomes.Remove(c2);
                    this._chromosomes.Remove(c3);
                }
                if (c2.GetFitness() >= c1.GetFitness() && c2.GetFitness() >= c3.GetFitness())
                {
                    //c2 � o melhor
                    this._chromosomes.Remove(c1);
                    this._chromosomes.Remove(c3);
                }
                if (c3.GetFitness() >= c2.GetFitness() && c3.GetFitness() >= c1.GetFitness())
                {
                    //c3 � o melhor
                    this._chromosomes.Remove(c2);
                    this._chromosomes.Remove(c1);
                }
            }
        }

        /// <summary>
        /// Cada candidato possui uma fatia da �roleta� proporcional a sua aptid�o
        /// </summary>
        /// <returns></returns>
        private CircuitoChromosome MetodoDaRoleta()
        {
            Hashtable htLista = new Hashtable();

            //Cada item sera adicionado a lista "fitness" vezes, p/ que tenha maior chance de ser sorteado
            Int64 htct = 0;
            //for (Int32 i = 0; i < (populationSize-1); i++)
            for (Int32 i = 0; i < (this._chromosomes.Count); i++)
            {
                for (Int32 j = 0; j < this._chromosomes[i].GetFitness(); j++)
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
            Int64 keyRoleta = Convert.ToInt64(random.Next((int)(htct - 1)).ToString());

            //Pega seu valor, que o indice de onde se localiza em "this.chromosomes"
            Int32 index = Convert.ToInt32(htLista[(Int64)keyRoleta].ToString());

            return this._chromosomes[index];
        }

        #endregion


        public CircuitoChromosome GetCurrentChromosome(int parent)
        {
            return _chromosomes[parent];
        }

        public CircuitoChromosome GetCurrentCloneChromosome(int ctControleParent, int ctControleClones)
        {
            return _chromosomes[ctControleParent].Clones[ctControleClones];
        }

        /// <summary>
        /// Instancia randomicamnete todos os clones
        /// </summary>
        /// <param name="ctControleParent"></param>
        /// <param name="sensors"></param>
        public void InitializeClones(int ctControleParent, bool[] sensors)
        {
            for (int i = 0; i < clonepopulation; i++)
                this._chromosomes[ctControleParent].Clones.Add(CircuitoChromosome.CreateRandomClone(sensors, this._chromosomes[ctControleParent].NormFitness, this._chromosomes[ctControleParent].Cgp.Genotype, this._k));

        }

        /// <summary>
        /// Instancia randomicamnete apenas um clone
        /// </summary>
        /// <param name="ctControleParent"></param>
        /// <param name="sensors"></param>
        public void InitializeClone(int ctControleParent, bool[] sensors)
        {
            if (this._chromosomes[ctControleParent].Clones.Count < clonepopulation)
                this._chromosomes[ctControleParent].Clones.Add(CircuitoChromosome.CreateRandomClone(sensors, this._chromosomes[ctControleParent].NormFitness, this._chromosomes[ctControleParent].Cgp.Genotype, this._k));
        }
    }
}

