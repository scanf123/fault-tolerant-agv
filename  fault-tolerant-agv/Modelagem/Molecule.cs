using System;
using System.Collections.Generic;
using System.Text;

namespace AGVFaultTolerant
{
    public class Molecule
    {
        int[] _input;
        int _function;
        int _levelsBack;
        ParameterMolecule _output;

        public ParameterMolecule Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public int LevelsBack
        {
            get { return _levelsBack; }
            set { _levelsBack = value; }
        }

        public int[] Input
        {
            get { return _input; }
            set { _input = value; }
        }

        /// <summary>
        /// 6 - AND
        /// 7 - XOR
        /// 8 - OR
        /// </summary>
        public int Function
        {
            get { return _function; }
            set { _function = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcao">5 - INV,  6 - AND,  7 - XOR , 8 - OR, 9 - ADD, 10 - INPUT, 11 - OUTPUT</param>
        /// <param name="entrada">Caso Seja um "INPUT", passar null, do mais, passar as entradas que estao sendo recebidas</param>
        /// <param name="saida"></param>
        public Molecule(int funcao, int[] entrada, ParameterMolecule saida)
        {
            _function = funcao;
            if (entrada == null)
                _function = 10;
            else
                _input = (int[])entrada;
            _output = saida;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="funcao"></param>
        /// <param name="saida"></param>
        public Molecule(int funcao, ParameterMolecule saida)
        {
            _function = funcao;

            _function = 10;
            _input = null;
            _levelsBack = 0;
            _output = saida;
        }
    }

    /// <summary>
    /// Identifica a molecula e armazena sua saida
    /// </summary>
    public class ParameterMolecule
    {
        public ParameterMolecule(int id, bool output)
        {
            Id = id;
            Output = output;
        }

        public int Id;
        public Boolean Output;
    }
}
