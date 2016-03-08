﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticAlgo
{
    class gradientDescent
    {
        List<indiv> pop;
        int sizePop;
        int nbIterationMax;
        int nbNeighbors;
        Random rand;
        indiv bestIndiv;
        geneticOperator GO;

        //constructors

        public gradientDescent()
        {
            this.nbIterationMax = 1000;
            this.nbNeighbors = 100;
            this.sizePop = 1;
            this.GO = new geneticOperator();
            this.rand = new Random();
            createDebugPop(2);
            this.bestIndiv = pop[0];
        }
        public gradientDescent(int nbIterationMax, int nbNeighbors, int sizePop, int dataSizeInIndivs)
        {
            this.nbIterationMax = nbIterationMax;
            this.nbNeighbors = nbNeighbors;
            this.sizePop = sizePop;
            this.GO = new geneticOperator();
            this.rand = new Random();
            createDebugPop(dataSizeInIndivs);
            this.bestIndiv = pop[0];
        }

        //methods

        public void update()
        {
            List<indiv> neighborhood;
            List<indiv> selectedIndivs;
            for (int i = 0; i < nbIterationMax; i++)
            {

                neighborhood = new List<indiv>();
                neighborhood.AddRange(pop);
                // if the population contains only one indiv this will artificially creates more indivs to be able, then, to select the best ones.
                if (nbNeighbors != 0)
                {
                    neighborhood.AddRange(generateNeighbors(pop[0], nbNeighbors));
                }

                // selection
                //selectedIndivs = neighborhood;
                selectedIndivs = tournamentSelection(neighborhood, neighborhood.Count/2);

                //genetic changes in the pop
                neighborhood = GO.applyGeneticChanges(selectedIndivs);
                //eval the new pop
                for (int ind = 0; ind < neighborhood.Count; ind++)
                {
                    neighborhood[ind].eval();
                    //Console.WriteLine("evals : {0}, data0 : {1}, data1 : {2}", neighborhood[ind].getEvalValue(), neighborhood[ind].getData()[0], neighborhood[ind].getData()[1]);
                }

                // select the sizePop best indivs from the newly generated pop to have the same size as pop for the next iteration
                pop = simpleSelection(neighborhood);
                if (i % 10 == 0)
                {
                    Console.WriteLine("iteration {0}, best eval : {1}", i, pop[0].getEvalValue());
                }
            }
            this.bestIndiv = pop[0];
        }
        public List<indiv> simpleSelection(List<indiv> pop)
        {

            pop = pop.OrderByDescending(x => x.getEvalValue()).ToList();
            return pop.GetRange(0, sizePop);
        }
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
            int test;
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
                test = 0;
                for (int i = 0; i < proba.Count; i++)
                {
                    if (posFirst <= proba[i])
                    {
                        firstCompetitor = new indiv(pop[i]);
                        test = i;
                        break;
                    }
                }
                for (int i = 0; i < proba.Count; i++)
                {
                    if (posSecond <= proba[i])
                    {
                        secondCompetitor = new indiv(pop[i]);
                        test = i;
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
        public List<indiv> generateNeighbors(indiv I, int nbNeighbors)
        {
            List<indiv> neighborhood = new List<indiv>();
            indiv neighboor;
            for (int nei = 0; nei < nbNeighbors; nei++)
            {
                neighboor = mutate(I);
                neighboor.eval();
                neighborhood.Add(neighboor);
            }
            return neighborhood;
        }
        public indiv mutate(indiv I)
        {
            double mutation;
            indiv mutant;
            mutation = rand.NextDouble() * 0.2 - 0.1;
            mutant = new indiv(I.getData());
            mutant.addDoubleToData(mutation);
            return (mutant);
        }
        public void createDebugPop(int sizeData)
        {
            pop = new List<indiv>();
            indiv ind;
            double data;
            for (int y = 0; y < sizePop; y++) {
                ind = new indiv();
                for (int i = 0; i < sizeData; i++)
                {
                    data = rand.NextDouble();
                    ind.addData(data);
                }
                ind.eval();
                pop.Add(ind);
            }
        }
        public void debuging()
        {
            for (int i = 0; i < sizePop; i++)
            {
                Console.WriteLine("\tpop {0} : {1}", i, pop[i].getData());
                Console.WriteLine("\tEval Value {0} : {1}", i, pop[i].getEvalValue());
            }
        }
        public indiv getBestIndiv()
        {
            return bestIndiv;
        }
    }
}
