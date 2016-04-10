using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace geneticAlgo
{
    //[Serializable]
    public class gradientDescent
    {
        public int iteration { get; set; }
        public List<indiv> pop { get; set; }
        public int sizePop { get; set; }
        public int nbIterationMax { get; set; }
        System.Random rand;
        public indiv bestIndiv { get; set; }
        public geneticOperator GO { get; set; }
        public bool DoTournament { get; set; }

        // ====== CONSTRUCTORS ==================

        // basic constuctor not very usefull
        public gradientDescent()
        {
            this.DoTournament = false;
            this.iteration = 0;
            this.nbIterationMax = 1000;
            this.sizePop = 10;
            this.GO = new geneticOperator();
            this.rand = new System.Random();
            this.pop = new List<indiv>();
            this.bestIndiv = null;
        }

        // construct a gradientDescent which will evolve sizePop neural networks for nbIterationMax iterations
        // the 1st element of nbNeuronByLayers is the nbr of neurons on the input layer, and the last element is the number of outputs of the last layer
        public gradientDescent(int nbIterationMax, int sizePop, List<int> nbNeuronByLayers, bool DoTournament)
        {
            this.DoTournament = DoTournament;
            this.iteration = 0;
            this.nbIterationMax = nbIterationMax;
            this.sizePop = sizePop;
            this.GO = new geneticOperator();
            this.rand = new System.Random();
            createPop(nbNeuronByLayers);
            this.bestIndiv = pop[0];
        }

        //same but with a set percentage of the pop which will be mutated and crossovered
        public gradientDescent(int nbIterationMax, int sizePop, List<int> nbNeuronByLayers, double mutantPercent, double crossoveredPercent, bool DoTournament)
        {
            this.DoTournament = DoTournament;
            this.iteration = 0;
            this.nbIterationMax = nbIterationMax;
            this.sizePop = sizePop;
            this.GO = new geneticOperator(mutantPercent, crossoveredPercent);
            this.rand = new System.Random();
            createPop(nbNeuronByLayers);
            this.bestIndiv = pop[0];
        }

        //constructor by copy
        public gradientDescent(gradientDescent g)
        {
            DoTournament = g.DoTournament;
            iteration = g.iteration;
            pop = new List<indiv>(g.pop);
            sizePop = g.sizePop;
            nbIterationMax = g.nbIterationMax;
            rand = new System.Random();
            GO = new geneticOperator(g.GO);
            bestIndiv = new indiv(g.bestIndiv);
            runEvals();
        }
        // ====== METHODS ==================

        //main loop will use generateNeighbors, runEvals, and then changePop on each iteration :
        //generateNeighbors : select by tournament some of the best indivs of the pop, then apply genetic operation on them to create other indivs
        //runEvals : run the eval on the new pop    
        //changePop : select sizePop nbr of indiv in this new pop, and go to the next iteration

        public void generateNeighbors()
        {
            if (iteration < nbIterationMax)
            {
                //apply changes on the pop to create new indivs
                pop = GO.applyGeneticChangesPercent(pop);
            }
        }

        public void runEvals()
        {
            if (iteration < nbIterationMax)
            {
                //begin the evals of the new pop
                for (int ind = 0; ind < pop.Count; ind++)
                {
                    if (pop[ind].getEvalValue() == -1)
                    {
                        pop[ind].eval();
                    }
                }
            }
        }

        public void changePop()
        {
            if (iteration < nbIterationMax)
            {
                // simple selection
                if (DoTournament == false)
                    pop = simpleSelection(pop);

                // selection by tournament
                else
                    pop = tournamentSelection(pop, sizePop);

                bestIndiv = pop[0];

                //print the best indiv every 10% of the nbIterationMax
                //if (iteration % (nbIterationMax / 10) == 0)
                //{
                Debug.Log(string.Format("iteration : {0}, best_indiv : {1}", iteration, bestIndiv.getEvalValue()));
                //}
                iteration += 1;
            }
        }

        // check if unity has finished to evaluate the new pop in the environnement
        public bool areEvalsFinished()
        {
            bool check = true;
            for (int i = 0; i < pop.Count; i++)
            {
                if (pop[i].isEvalFinished() == false)
                {
                    check = false;
                }
            }
            return check;
        }

        // return a pop resized with sizePop indivs with the best evalValues
        public List<indiv> simpleSelection(List<indiv> pop)
        {
            pop = pop.OrderByDescending(x => x.getEvalValue()).ToList();
            return pop.GetRange(0, sizePop);
        }

        //for each round this method will choose 2 indivs with probabilities based on their evalValue, and select the one which has the best evalValue
        //at the end it will return a list containing the indivs selected at each round
        public List<indiv> tournamentSelection(List<indiv> pop, int numberOfRounds = 2)
        {
            //choose 2 indiv randomly with proba accorded to their eval values and select the best out of the two. Repeat the process numberOfRounds times.
            // Warning ! numberOfRounds needs to be >= 2 because the first round is always won by the better indiv
            double somme;
            indiv max = new indiv();
            for(int i = 0; i < pop.Count; i++)
            {
                if (pop[i].getEvalValue() > max.getEvalValue())
                    max = pop[i];
            }
            List<indiv> winners = new List<indiv>();
            winners.Add(max);
            List<indiv> competitors = new List<indiv>(pop);
            List<double> proba;
            indiv firstCompetitor = new indiv();
            indiv secondCompetitor = new indiv();
            int round = 1;
            double posFirst;
            double posSecond;
            // transform eval values to probas
            do
            {
                somme = 0;
                proba = new List<double>();
                for (int i = 0; i < competitors.Count; i++)
                {
                    somme += competitors[i].getEvalValue();
                    proba.Add(somme);
                }
                for (int d = 0; d < proba.Count; d++)
                {
                    proba[d] /= somme;
                }
                // choosing 2 random competitors accordingly to the probas
                posFirst = rand.NextDouble();
                posSecond = rand.NextDouble();
                for (int i = 0; i < proba.Count; i++)
                {
                    if (posFirst <= proba[i])
                    {
                        firstCompetitor = new indiv(pop[i]);
                        break;
                    }
                }
                for (int i = 0; i < proba.Count; i++)
                {
                    if (posSecond <= proba[i])
                    {
                        secondCompetitor = new indiv(pop[i]);
                        break;
                    }
                }
                //choosing the best out of the two
                if (firstCompetitor.getEvalValue() >= secondCompetitor.getEvalValue())
                {
                    winners.Add(firstCompetitor);
                    competitors.Remove(secondCompetitor);
                }
                else if (secondCompetitor.getEvalValue() >= firstCompetitor.getEvalValue())
                {
                    winners.Add(secondCompetitor);
                    competitors.Remove(firstCompetitor);
                }
                round += 1;
            } while (round <= numberOfRounds-1);
            return winners;
        }

        //create a population of neural_networks with random weigths on each input of each neuron of each layers and evaluate them
        public void createPop(List<int> nbNeuronByLayers)
        {
            pop = new List<indiv>();
            indiv ind;
            for (int y = 0; y < sizePop; y++) {
                ind = new indiv(nbNeuronByLayers);
                ind.eval();
                pop.Add(ind);
            }
        }


        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }


        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            catch(Exception ex)
            {
                Debug.Log(ex);
                return new T();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        // ====== GET/SET ==================

        public indiv getBestIndiv()
        {
            return bestIndiv;
        }

    }
}
