using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace geneticAlgo
{
    public class gradientDescent
    {
        int iteration;
        List<indiv> pop;
        int sizePop;
        int nbIterationMax;
        System.Random rand;
        indiv bestIndiv;
        geneticOperator GO;
        List<indiv> selectedIndivs;

        // ====== CONSTRUCTORS ==================

        // basic constuctor not very usefull
        public gradientDescent()
        {
            this.iteration = 0;
            this.nbIterationMax = 1000;
            this.sizePop = 10;
            this.GO = new geneticOperator();
            this.rand = new System.Random();
            createPop(new List<int>(new int[] { 19, 32, 8,3 }));
            this.bestIndiv = pop[0];
        }

        // construct a gradientDescent which will evolve sizePop neural networks for nbIterationMax iterations
        // the 1st element of nbNeuronByLayers is the nbr of neurons on the input layer, and the last element is the number of outputs of the last layer
        public gradientDescent(int nbIterationMax, int sizePop, List<int> nbNeuronByLayers)
        {
            this.iteration = 0;
            this.nbIterationMax = nbIterationMax;
            this.sizePop = sizePop;
            this.GO = new geneticOperator();
            this.rand = new System.Random();
            createPop(nbNeuronByLayers);
            this.bestIndiv = pop[0];
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
                // selection by tournament
                selectedIndivs = tournamentSelection(pop, pop.Count / 2);
                //apply changes on the selected pop to create new indivs
                pop = GO.applyGeneticChanges(selectedIndivs, sizePop);
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
                // select the sizePop best indivs from the newly generated pop to have the same size as pop for the next iteration
                pop = simpleSelection(pop);

                bestIndiv = pop[0];

                //print the best indiv every 10% of the nbIterationMax
                //if (iteration % (nbIterationMax / 10) == 0)
                //{
                Debug.Log(string.Format("iteration : {0}, best_indiv : {1}", iteration, bestIndiv));
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
            // Warning ! numberOfRounds needs to be >= 2
            double somme;
            List<indiv> winners = new List<indiv>();
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
            } while (round <= numberOfRounds);
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

        // ====== GET/SET ==================

        public indiv getBestIndiv()
        {
            return bestIndiv;
        }
    }
}
