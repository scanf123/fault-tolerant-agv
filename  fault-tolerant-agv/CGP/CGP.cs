using System;
using System.Collections.Generic;
using System.Text;

namespace AGVFaultTolerant
{
    public class CGP
    {
        int[] _genotype;
        Molecule[,] _digitalCircuit;
        ParameterMolecule[] _output;
        ParameterMolecule[] _input;
        int linha, coluna, palavra;

        public ParameterMolecule[] Input
        {
            get { return _input; }
            set { _input = value; }
        }


        public ParameterMolecule[] Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public int[] Genotype
        {
            get { return _genotype; }
            set { _genotype = value; }
        }

        public Molecule[,] DigitalCircuit
        {
            get { return _digitalCircuit; }
            set { _digitalCircuit = value; }
        }

        /// <summary>
        /// Cartesian  Genetic Programming
        /// </summary>
        /// <param name="n"> Linhas de moleculas </param>
        /// <param name="m"> Colunas de Moleculas </param>
        /// <param name="w"> Tamanho do dado (data width) </param>
        /// <param name="p"> Quantidade de nos intermediarios (level back parameter) (Geralmente, numero de colunas -1 )</param>
        public CGP(int n, int m, int w, int p, int[] genotype, ParameterMolecule[] output, ParameterMolecule[] input)
        {
            _genotype = genotype;
            _digitalCircuit = new Molecule[n, m];
            _input = input;
            _output = output;
            linha = n;
            coluna = m;
            palavra = w;

            //Instanciando a matriz que representa o circuito genericamente
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //Contador ID é por coluna
                    _digitalCircuit[j, i] = new Molecule(6, new int[w], new ParameterMolecule(((linha * 4) + j + (i * linha)), false));
                }
            }
            //Constroi o Circuito a partir do GENE Passado
            BuilCircuitFromGene();
        }

        /// <summary>
        /// Assign molecules in "_digitalCircuit" from the coded "_genotype"
        /// </summary>
        public void BuilCircuitFromGene()
        {
            int ctl = 0, ctc = 0, ctGeral = 0;
            List<int> lstInputTmp = new List<int>();

            ///Percorrendo o Genotipo
            foreach (int gen in _genotype)
            {
                //if (ctGeral % 4 == 0 && ctGeral != 0)

                //Trata-se da função                
                if ((palavra + ((palavra * ctl) + ctl) + ((ctc == 0) ? 0 : (palavra * linha * ctc + (ctc * linha)))) == ctGeral)
                {
                    //Atribui funcao
                    _digitalCircuit[ctl, ctc].Function = gen;
                    //Atribui entradas
                    _digitalCircuit[ctl, ctc].Input = lstInputTmp.ToArray();
                    //Atribui nivel do level back
                    _digitalCircuit[ctl, ctc].LevelsBack = 1;

                    lstInputTmp.Clear();
                    ctl++;
                }
                else
                {
                    lstInputTmp.Add(gen);//Trata-se das entradas

                }
                ctGeral++;
                if (ctl == linha)
                {
                    ctl = 0;
                    ctc++;
                }
            }
        }

        /// <summary>
        /// Retorna as saidas do circuito a partir das entradas setadas em "Input"
        /// </summary>
        /// <returns></returns>
        public bool[] Evaluate()
        {
            for (int j = 0; j < coluna; j++)
            {
                for (int i = 0; i < linha; i++)
                {
                    //Atualiza da esquerda p/ a direita
                    _digitalCircuit[i, j].Output.Output = EvaluateMolecule(_digitalCircuit[i, j].Output.Id);
                    //Atualiza output
                    if (OutPutContains(_digitalCircuit[i, j].Output.Id))
                        OutputId(_digitalCircuit[i, j].Output.Id, _digitalCircuit[i, j].Output.Output);
                }
            }


            List<bool> lstRetorno = new List<bool>();
            for (int i = 0; i < _output.Length; i++)
            {
                lstRetorno.Add(_output[i].Output);
            }
            return lstRetorno.ToArray();
        }

        /// <summary>
        /// Avalia (atualiza saida) da molecula especifica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EvaluateMolecule(int id)
        {
            //Verificar se o ID pertence ao IPUT, se sim, retornar o valor do INPUT
            if (InputContains(id))
                return InputId(id);

            foreach (Molecule m in _digitalCircuit)
            {
                //Encontrou molecula
                if (id == m.Output.Id)
                {
                    switch (m.Function)
                    {
                        //5 - INV,  6 - AND,  7 - XOR , 8 - OR, 9 - ADD, 10 - INPUT, 11 - OUTPUT</param>
                        case 5:
                            //INV
                            return !EvaluateMolecule(m.Input[0]) && EvaluateMolecule(m.Input[1]) && !EvaluateMolecule(m.Input[2]) && EvaluateMolecule(m.Input[3]);
                            break;
                        case 6:
                            //AND
                            return EvaluateMolecule(m.Input[0]) && EvaluateMolecule(m.Input[1]) && EvaluateMolecule(m.Input[2]) && EvaluateMolecule(m.Input[3]);
                            break;
                        case 7:
                            //XOR
                            // ( (XOR AB) ) XOR ( ( XOR CD ))
                            return (((EvaluateMolecule(m.Input[0]) != EvaluateMolecule(m.Input[1]))) != ((EvaluateMolecule(m.Input[2]) != EvaluateMolecule(m.Input[3]))));
                            break;
                        case 8:
                            //OR
                            return EvaluateMolecule(m.Input[0]) || EvaluateMolecule(m.Input[1]) || EvaluateMolecule(m.Input[2]) || EvaluateMolecule(m.Input[3]);
                            break;
                        //case 9:
                        //ADD
                        ////    break;
                        //case 10:
                        //    //INPUT
                        //    break;
                        //case 11:
                        //    //OUTPUT
                        //    break;
                        default:
                            break;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Retorna o valor da entrada desejada
        /// </summary>
        /// <param name="id">Id da entrada a qual quer saber o valor</param>
        /// <returns></returns>
        private bool InputId(int id)
        {
            bool retorno = false;
            for (int i = 0; i < _input.Length; i++)
                if (id == Input[i].Id)
                {
                    retorno = Input[i].Output;
                    break;
                }

            return retorno;
        }

        /// <summary>
        /// Verifica se a entrada desejada existe
        /// </summary>
        /// <param name="id">Id da entrada a ser verificada</param>
        /// <returns></returns>
        private bool InputContains(int id)
        {
            for (int i = 0; i < _input.Length; i++)
                if (id == Input[i].Id)
                    return true;
            return false;
        }

        /// <summary>
        /// Avalia se o GEnotype é valido
        /// Restricoes por alelos
        /// </summary>
        /// <returns></returns>
        public bool AvaliarGeneImplementavel()
        {
            bool retorno = true;


            //TODO: Avaliar o Genotipo
            //1 Tamanho
            //2 Portas logicas existentes
            //3 Conexoes corretas

            return retorno;
        }

        /// <summary>
        /// Verifica se o vetor de saida contem o Id
        /// </summary>
        /// <param name="id">Id procurado</param>
        /// <returns></returns>
        private bool OutPutContains(int id)
        {
            for (int i = 0; i < _output.Length; i++)
                if (id == _output[i].Id)
                    return true;
            return false;
        }

        /// <summary>
        /// Atualiza saida especifica com o valor passado como parametro
        /// </summary>
        /// <param name="id">Id da saida a ser atualizada</param>
        /// <param name="value">Novo valor da Saida</param>
        private void OutputId(int id, Boolean value)
        {
            bool retorno = false;
            for (int i = 0; i < _output.Length; i++)
                if (id == _output[i].Id)
                {
                    _output[i].Output = value;
                    break;
                }
        }

    }
}

