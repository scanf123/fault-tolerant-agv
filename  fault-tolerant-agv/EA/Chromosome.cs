using System;
using System.Collections.Generic;
using System.Text;

namespace AGVFaultTolerant
{

    /// <summary>
    /// Classe que representa um cromossomo do controle de navegaçao
    /// </summary>
    public class CircuitoChromosome : IComparable<CircuitoChromosome>
    {
        // Vetor com os valores do quadrado
        byte btest = new byte();

        private List<CircuitoChromosome> clones;

        private CGP _cgp;
        double _distance;
        double _time;
        // Objeto random para geração de números randômicos
        private static Random _random = new Random(DateTime.Now.Millisecond);
        private double _fitness;
        private double _normFitness;
        private int _n;

        public int N
        {
            get { return _n; }
            set { _n = value; }
        }

        public double NormFitness
        {
            get { return _normFitness; }
            set { _normFitness = value; }
        }

        /// <summary>
        /// Position counter of two motors
        /// </summary>
        public double Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// IO period counter
        /// </summary>
        public double Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public CGP Cgp
        {
            get { return _cgp; }
            set { _cgp = value; }
        }

        /// <summary>
        /// 22 LUTs*16 bits/LUT = 352 bits        
        /// </summary>
        public int[] Individual
        {
            get { return Cgp.Genotype; }
            set { Cgp.Genotype = value; }
        }

        /// <summary>
        /// 8 input bits from the sensors
        /// </summary>
        public ParameterMolecule[] InputBits
        {
            get { return Cgp.Input; }
            set { Cgp.Input = value; }
        }

        /// <summary>
        /// Armazena os clones do cromossomo atual
        /// </summary>
        public List<CircuitoChromosome> Clones
        {
            get { return clones; }
            set { clones = value; }
        }


        /// <summary>
        /// 2 output bits to the motors        
        /// </summary>
        public ParameterMolecule[] OutputBits
        {
            get { return Cgp.Output; }
            set { Cgp.Output = value; }
        }

        public double Fitness
        {
            get { return _fitness; }
        }


        // Construtor - recebe o vetor com os valores
        public CircuitoChromosome(int[] individual, ParameterMolecule[] inputBits, ParameterMolecule[] outputBits)
        {
            //_cgp = new CGP(16, 3, 4, 3, lstInt.ToArray(), lstMolOUTPUT.ToArray(), lstMolINPUT.ToArray());
            Cgp = new CGP(16, 3, 4, 3, individual, outputBits, inputBits);
            Cgp.Evaluate();
            bool viraEsq = false;
            if (Cgp.Output[0].Output == true && Cgp.Output[0].Output == false)
                viraEsq = true;

            clones = new List<CircuitoChromosome>();
            _n = -1;
        }


        public static CircuitoChromosome CreateRandomChromosome(bool[] senesors)
        {
            CircuitoChromosome qSChromosomeRetorno;

            //GENOTIPO
            List<int> lstInt = new List<int>();
            //Gene
            string strGen = "0;1;2;3;8;4;5;6;7;8;8;9;10;11;8;12;13;14;15;8;16;17;18;19;8;20;21;22;23;8;24;25;26;27;8;28;29;30;31;8;32;33;34;35;8;36;37;38;39;8;40;41;42;43;8;44;45;46;47;8;48;49;50;51;8;52;53;54;55;8;56;57;58;59;8;60;61;62;63;8;64;65;66;67;8;68;69;70;71;8;72;73;74;75;8;76;77;78;79;8;64;65;66;67;8;68;69;70;71;8;72;73;74;75;8;76;77;78;79;8;64;65;66;67;8;68;69;70;71;8;72;73;74;75;8;76;77;78;79;8;64;65;66;67;8;68;69;70;71;8;72;73;74;75;8;76;77;78;79;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8;80;81;82;83;8";

            //Indices das operações lógicas da LUT lógicas
            int[] indexes = new int[] { 4, 10, 14, 19, 24, 29, 34, 39, 44, 49, 54, 59, 64, 69, 74, 79, 84, 89, 94, 99, 164, 169 };

            foreach (string s in strGen.Split(';'))
                lstInt.Add(Convert.ToInt32(s));

            int index = 0;
            List<int> lstIndexToMuatate = new List<int>();
            foreach (int item in lstInt)
            {
                if (item == 8)
                    lstIndexToMuatate.Add(index);

                index++;
            }


            //INPUT
            List<ParameterMolecule> lstMolINPUT = new List<ParameterMolecule>();
            //Molecule moleculeTmmp;
            for (int i = 0; i < 64; i++)
            {
                lstMolINPUT.Add(new ParameterMolecule(i, senesors[i % 8]));
            }


            //OUTPUT
            List<ParameterMolecule> lstMolOUTPUT = new List<ParameterMolecule>();
            lstMolOUTPUT.Add(new ParameterMolecule(96, false));
            lstMolOUTPUT.Add(new ParameterMolecule(97, false));

            qSChromosomeRetorno = new CircuitoChromosome(lstInt.ToArray(), lstMolINPUT.ToArray(), lstMolOUTPUT.ToArray());

            return qSChromosomeRetorno;
        }




        /// <summary>
        /// Fitness, o cálculo de quanto um cromossomo é bom
        /// </summary>
        /// <returns> Retorna a avaliação deste cromossomo. Valor "zero" é o cromossomo ideal, o objetivo da busca. </returns>
        public double GetFitness()
        {
            //_fitness = ((_distance * _time) / 1000);
            //return (_fitness = (_time * _distance));
            //Melhor caso "_fitness = 1"
            //Pior caso "_fitness = 0"
            System.Diagnostics.Debug.WriteLine(string.Format("_fitness={0} ; _time={1} ; _distance={2} ; ", ((_time * _distance) / 140), _time, _distance));
            //return (_fitness = ((_time * _distance) / 280));
            return (_fitness = ((_time * _distance) / 140));

            //return (_fitness = (_time * _distance));

        }

        //Alterado para retornar o maior fitness
        public int CompareTo(CircuitoChromosome obj)
        {
            if (this._fitness > obj._fitness)
                return -1;
            else if (this._fitness < obj._fitness)
                return 1;
            else
                return 0;
        }

        private void AlteragaGeneAgv(int ctParada, ref string txtGeneAgv, int novoValor)
        {
            int ct = 1;
            List<int> lstInt = new List<int>();

            foreach (string s in txtGeneAgv.Split(';'))
            {
                lstInt.Add(Convert.ToInt32(s));

                if (ct == ctParada)
                    lstInt[lstInt.Count - 1] = novoValor;
                ct++;
            }
            txtGeneAgv = "";
            foreach (int i in lstInt)
                txtGeneAgv = txtGeneAgv + i + ";";

            //Remove o ultimo ';'
            txtGeneAgv = txtGeneAgv.Remove(txtGeneAgv.Length - 1);
        }

        public static CircuitoChromosome CreateRandomClone(bool[] sensors, double normFitnessParent, int[] genotypeParent, int k)
        {
            CircuitoChromosome qSChromosomeRetorno;

            //GENOTIPO
            List<int> lstInt = new List<int>();
            //Copia do pai
            lstInt.AddRange(genotypeParent);

            //Indices das operações lógicas da LUT lógicas
            int[] indexes = new int[] { 4, 10, 14, 19, 24, 29, 34, 39, 44, 49, 54, 59, 64, 69, 74, 79, 84, 89, 94, 99, 164, 169 };

            //lstIntBackup.AddRange(lstInt.ToArray());

            //INPUT
            List<ParameterMolecule> lstMolINPUT = new List<ParameterMolecule>();
            //Molecule moleculeTmmp;
            for (int i = 0; i < 64; i++)
            {
                lstMolINPUT.Add(new ParameterMolecule(i, sensors[i % 8]));
            }

            //Altera randomicamente o numero de portas de acordo com o fitness do pai            
            int portasAlteradas = 0;


            portasAlteradas = (int)(k * (1 - Math.Abs(normFitnessParent)));
            portasAlteradas = (portasAlteradas < 0) ? 0 : portasAlteradas;

            int index;
            List<int> lstIndexs = new List<int>();

            int indexMutate = -1;
            Random r = new Random();

            for (int i = 0; i < portasAlteradas; i++)
            {
                //int indextmp = r.Next((indexes.Length - 1));

                int indextmp = r.Next((indexes.Length));
                //Porta a ser alterada
                indexMutate = indexes[indextmp];

                int novaFuncPorta = 5 + r.Next(4);
                //Altera a função lógica da porta
                lstInt[indexMutate] = novaFuncPorta;
            }

            //OUTPUT
            List<ParameterMolecule> lstMolOUTPUT = new List<ParameterMolecule>();
            lstMolOUTPUT.Add(new ParameterMolecule(96, false));
            lstMolOUTPUT.Add(new ParameterMolecule(97, false));

            qSChromosomeRetorno = new CircuitoChromosome(lstInt.ToArray(), lstMolINPUT.ToArray(), lstMolOUTPUT.ToArray());
            qSChromosomeRetorno._n = portasAlteradas;
            return qSChromosomeRetorno;
        }

        internal void GetNormFitness(double fitTotalPopulacao)
        {
            //TODO: Alterar o GetNorm fitness para trabalhar como uma proporção linear, como uma reta, variando do melhor caso fitness = 5  (normFit=1), e pior caso fitness = 0(normFit=0)
            if (fitTotalPopulacao == 0)
                _normFitness = 0;
            else
            {
                //fitTotalPopulacao = fitTotalPopulacao / tamPopulacao;
                _normFitness = _fitness / fitTotalPopulacao;
            }
        }
    }


}


