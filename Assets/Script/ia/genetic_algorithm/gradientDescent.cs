using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace geneticAlgo
{
    class gradientDescent
    {
        List<indiv> pop;
        int sizePop;
        int nbIterationMax;
        System.Random rand;
        indiv bestIndiv;
        geneticOperator GO;

        // ====== CONSTRUCTORS ==================

        // basic constuctor not very usefull
        public gradientDescent()
        {
            this.nbIterationMax = 1000;
            this.sizePop = 10;
            this.GO = new geneticOperator();
            this.rand = new System.Random();
            createPop(new List<int>(new int[] { 5, 4, 4,3 }));
            this.bestIndiv = pop[0];
        }

        // construct a gradientDescent which will evolve sizePop neural networks for nbIterationMax iterations
        // the 1st element of nbNeuronByLayers is the nbr of neurons on the input layer, and the last element is the number of outputs of the last layer
        public gradientDescent(int nbIterationMax, int sizePop, List<int> nbNeuronByLayers)
        {
            this.nbIterationMax = nbIterationMax;
            this.sizePop = sizePop;
            this.GO = new geneticOperator();
            this.rand = new System.Random();
            createPop(nbNeuronByLayers);
            this.bestIndiv = pop[0];
        }

        // ====== METHODS ==================

        //main loop : select by tournament some of the best indivs of the pop, then apply genetic operation on them to create other indivs
        //then select sizePop nbr of indiv in this new pop, and go to the next iteration
        public void update()
        {
            List<indiv> neighborhood;
            List<indiv> selectedIndivs;
            for (int i = 0; i < nbIterationMax; i++)
            {
                neighborhood = new List<indiv>();
                neighborhood.AddRange(pop);

                // selection by tournament
                selectedIndivs = tournamentSelection(neighborhood, neighborhood.Count / 2);

                //apply changes on the selected pop to create new indivs
                neighborhood = GO.applyGeneticChanges(selectedIndivs, sizePop);

                //eval the new pop
                for (int ind = 0; ind < neighborhood.Count; ind++)
                {
                    neighborhood[ind].eval();
                }

                // select the sizePop best indivs from the newly generated pop to have the same size as pop for the next iteration
                pop = simpleSelection(neighborhood);

                bestIndiv = pop[0];

                //print the best indiv every 10% of the nbIterationMax
                if (i % (nbIterationMax / 10) == 0)
                {
                    Debug.Log(string.Format("iteration : {0}, best_indiv : {1}", i, bestIndiv));
                }
            }
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
