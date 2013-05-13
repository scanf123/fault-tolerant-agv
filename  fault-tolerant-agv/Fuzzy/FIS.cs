using System;
using System.Collections.Generic;
using System.Text;
//using AForge.Fuzzy;

namespace FuzzyAGV
{    

    // Fuzzy Inference System
    public class FIS
    {

        private Dictionary<String, Linguistica> dbFuzzy;        

        // Criando o Sistema de inferência com sua base de dados e de regras
        public FIS()
        {

            //Corredor width 100

            //Criar todos os conjuntos Nebolusos
            //Distancia
            ConjuntoNebuloso cnDistPerto = new ConjuntoNebuloso("Perto");
            cnDistPerto.AdicionarReta(0, 1, 15, 1);
            cnDistPerto.AdicionarReta(15, 1,30, 0);

            ConjuntoNebuloso cnDistMedio = new ConjuntoNebuloso("Medio");
            cnDistMedio.AdicionarReta(15, 0, 30, 1);
            cnDistMedio.AdicionarReta(30, 1, 100, 1);
            cnDistMedio.AdicionarReta(100, 1,115 ,0);

            ConjuntoNebuloso cnDistLonge = new ConjuntoNebuloso("Longe");
            cnDistLonge.AdicionarReta(100, 0, 115, 1);
            cnDistLonge.AdicionarReta(115, 1, 100000, 1);

            //Angulo
            ConjuntoNebuloso cnAnguloEsq = new ConjuntoNebuloso("Esquerda");
            cnAnguloEsq.AdicionarReta(-45, 1, -35, 1);
            cnAnguloEsq.AdicionarReta(-35, 1, 0, 0);
           
            ConjuntoNebuloso cnAnguloManter = new ConjuntoNebuloso("Manter");
            cnAnguloManter.AdicionarReta(-35, 0, 0, 1);
            cnAnguloManter.AdicionarReta(0, 1,35, 0);

            ConjuntoNebuloso cnAnguloDireita = new ConjuntoNebuloso("Direita");
            cnAnguloDireita.AdicionarReta(0, 0, 35, 1);
            cnAnguloDireita.AdicionarReta(35, 1, 45, 1);
            

            //Variaval Linguistica
            //Distancia
            //Direita
            Linguistica lvDistanciaDir = new Linguistica("Direita");
            lvDistanciaDir.Conjuntos["Perto"] = cnDistPerto;
            lvDistanciaDir.Conjuntos["Medio"] = cnDistMedio;
            lvDistanciaDir.Conjuntos["Longe"] = cnDistLonge;

            //Esquerda
            Linguistica lvDistanciaEsq = new Linguistica("Esquerda");
            lvDistanciaEsq.Conjuntos["Perto"] = cnDistPerto;
            lvDistanciaEsq.Conjuntos["Medio"] = cnDistMedio;
            lvDistanciaEsq.Conjuntos["Longe"] = cnDistLonge;

            //Frente
            Linguistica lvDistanciaFrente = new Linguistica("Frente");
            lvDistanciaFrente.Conjuntos["Perto"] = cnDistPerto;
            lvDistanciaFrente.Conjuntos["Medio"] = cnDistMedio;
            lvDistanciaFrente.Conjuntos["Longe"] = cnDistLonge;

            //Frente
            Linguistica lvAngulo = new Linguistica("Angulo");
            lvAngulo.Conjuntos["Direita"] = cnAnguloDireita;
            lvAngulo.Conjuntos["Esquerda"] = cnAnguloEsq;
            lvAngulo.Conjuntos["Manter"] = cnAnguloManter;

            
            //Criando Bando Nebuloso
            dbFuzzy = new Dictionary<string, Linguistica>();
            dbFuzzy["Direita"] = lvDistanciaDir;
            dbFuzzy["Esquerda"] = lvDistanciaEsq;
            dbFuzzy["Frente"] = lvDistanciaFrente;
            dbFuzzy["Angulo"] = lvAngulo;

        }

        // Realizando a inferencia baseando-se nas variáveis de entrada
        public void DoInference(double rightDist, double leftDist, double frontDist, out double Angle, out double Speed)
        {
            //Angle = 0;
            Speed = 50;

            dbFuzzy["Direita"].VlNumerico = rightDist;

            dbFuzzy["Esquerda"].VlNumerico = leftDist;

            dbFuzzy["Frente"].VlNumerico = frontDist;

            //Cria o Banco de Regras
            List<Regra> rbFuzzy = BancoRegras();

            //obtendo sída nebuosa
            Dictionary<string, double> fzzOut = new Dictionary<string, double>();


            foreach (Regra re in rbFuzzy)
            {
                double pa = (double)re.Confianca();

                if (fzzOut.ContainsKey(re.Consequente.Conjunto.Nome))
                {
                    if (pa > fzzOut[re.Consequente.Conjunto.Nome])
                        fzzOut[re.Consequente.Conjunto.Nome] = pa;
                }
                else
                {
                    fzzOut[re.Consequente.Conjunto.Nome] = pa;
                }

            }

            #region Saída
            
            Angle = Convert.ToDouble(Defuzz.Calcular(dbFuzzy["Angulo"], fzzOut, -90, 90, 10000));//Math.Round(Convert.ToDouble(Defuzz.Calcular(dbFuzzy["Angulo"], fzzOut, -90, 90, 10000)), 2);        

            #endregion

        }

        private List<Regra> BancoRegras()
        {
            List<Regra> rbFuzzy = new List<Regra>();
            Regra regra;

            //Manter
            //Frontal Alta 
            //Direita Alta 
            //Esquerda Alta
            //Angulo Manter
            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe")); 
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);


            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Manter");
            rbFuzzy.Add(regra);

            //Vira a direita por padrão
            //TODO: Essas medias de exceções nao estao funcionando muito bem
            //Caso direita e esquerda longe e frente perto
            //Case direita e esquerda perto e frente perto
            //Case direita e esquerda medio e frente perto
            //regra = new Regra();
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            //regra.AdicionarNorma();
            //regra.AdicionarNorma();
            //regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            //rbFuzzy.Add(regra);

            //regra = new Regra();
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));
            //regra.AdicionarNorma();
            //regra.AdicionarNorma();
            //regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            //rbFuzzy.Add(regra);

            //regra = new Regra();
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));
            //regra.AdicionarNorma();
            //regra.AdicionarNorma();
            //if (lastTurn == String.Empty)
            //    regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            //else
            //    regra.Consequente = new Clausula(dbFuzzy, "Angulo", _lastTurn);
            //rbFuzzy.Add(regra);

            //regra = new Regra();
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            //regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            //regra.AdicionarNorma();
            //regra.AdicionarNorma();
            //if (lastTurn == String.Empty)
            //    regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            //else
            //    regra.Consequente = new Clausula(dbFuzzy, "Angulo", _lastTurn);
            //rbFuzzy.Add(regra);


            //Virar a direita            
            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);                        

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Direita");
            rbFuzzy.Add(regra);


            //Virar a Esquerda
            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);            

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Medio"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));            
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Medio"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);

            regra = new Regra();
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Frente", "Longe"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Direita", "Perto"));
            regra.AdicionarClausula(new Clausula(dbFuzzy, "Esquerda", "Longe"));
            regra.AdicionarNorma();
            regra.AdicionarNorma();
            regra.Consequente = new Clausula(dbFuzzy, "Angulo", "Esquerda");
            rbFuzzy.Add(regra);

            return rbFuzzy;
        }


    }
}
