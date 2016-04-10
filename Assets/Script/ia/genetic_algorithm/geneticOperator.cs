using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace geneticAlgo
{
    public class geneticOperator
    {
        public double crossoverPercent { get; set; }
        public double mutationPercent { get; set; }
        public List<int> randomlyGeneratedNbNeuronsByLayers { get; set; }
        System.Random rand;
        
        // ====== CONSTRUCTORS ==================

        public geneticOperator()
        {
            crossoverPercent = 0.1;
            mutationPercent = 0.7;
            rand = new System.Random();
            randomlyGeneratedNbNeuronsByLayers = new List<int>(new int[] { 19, 32, 8, 3 });
        }

        //crossoverPercent + mutationPercent must be <= 1 (if <1 the rest will be the percentage of newly generated indivs)
        public geneticOperator(double crossoverPercent, double mutationPercent)
        {
            this.crossoverPercent = crossoverPercent;
            this.mutationPercent = mutationPercent;
            randomlyGeneratedNbNeuronsByLayers = new List<int>(new int[] { 19, 32, 8, 3 });
            rand = new System.Random();
        }

        public geneticOperator(double crossoverPercent, double mutationPercent, List<int> nbNeuronByLayers)
        {
            this.crossoverPercent = crossoverPercent;
            this.mutationPercent = mutationPercent;
            randomlyGeneratedNbNeuronsByLayers = new List<int>(nbNeuronByLayers);
            rand = new System.Random();
        }

        public  geneticOperator(geneticOperator GO)
        {
            crossoverPercent = GO.crossoverPercent;
            mutationPercent = GO.mutationPercent;
            randomlyGeneratedNbNeuronsByLayers = new List<int>(GO.randomlyGeneratedNbNeuronsByLayers);
            rand = new System.Random();
        }
        // ====== METHODS ==================

        //given two indiv A & B, this method will choose a random layer and a random neuron position in that layer and then do a crossover
        //by cuting A and B at this pos and mixing them together, and it will return a list containing the 2 mixed indivs
        public List<indiv> crossover(indiv A, indiv B)
        {
            int layer_num_Crossingover = rand.Next(1, A.getNbLayer()-1);//in the first layer all weigths are 1 and can't change so no need to crossover at this layer
            int Neuron_num_Crossingover = rand.Next(0, A.getNbNeuronAtLayer(layer_num_Crossingover)-1);

            List<Nn.Neuron> layer_CrossingoverA = new List<Nn.Neuron>(A.getLayer(layer_num_Crossingover).GetRange(0, Neuron_num_Crossingover));
            try
            {
                layer_CrossingoverA.AddRange(B.getLayer(layer_num_Crossingover).GetRange(Neuron_num_Crossingover, B.getNbNeuronAtLayer(layer_num_Crossingover) - Neuron_num_Crossingover));
            }
            catch
            {
                Debug.Log("erreur pendant crossingover : layer_num : " + layer_num_Crossingover + ", neuron_num : " + Neuron_num_Crossingover + ", number_neuron_at_layer : B : " + B.getNbNeuronAtLayer(layer_num_Crossingover)+" A : "+ A.getNbNeuronAtLayer(layer_num_Crossingover));
            }

            List<Nn.Neuron> layer_CrossingoverB = new List<Nn.Neuron>(B.getLayer(layer_num_Crossingover).GetRange(0, Neuron_num_Crossingover));
            layer_CrossingoverB.AddRange(A.getLayer(layer_num_Crossingover).GetRange(Neuron_num_Crossingover, A.getNbNeuronAtLayer(layer_num_Crossingover) - Neuron_num_Crossingover));

            List<List<Nn.Neuron>> NnA2 = new List<List<Nn.Neuron>>(A.getLayersRange(0, layer_num_Crossingover));
            NnA2.Add(layer_CrossingoverA);
            NnA2.AddRange(B.getLayersRange(layer_num_Crossingover+1, B.getNbLayer()));

            List<List<Nn.Neuron>> NnB2 = new List<List<Nn.Neuron>>(B.getLayersRange(0, layer_num_Crossingover));
            NnB2.Add(layer_CrossingoverB);
            NnB2.AddRange(A.getLayersRange(layer_num_Crossingover+1, A.getNbLayer()));



            indiv A2 = new indiv(NnA2);
            indiv B2 = new indiv(NnB2);
            List<indiv> newIndivs = new List<indiv>();
            newIndivs.Add(A2);
            newIndivs.Add(B2);
            return newIndivs;
        }

        //this method will choose a random weigth of a random neuron of a random layer and modify it by a random number between -0.1 and 0.1
        public indiv mutate(indiv A)
        {
            int layer_num_mutation = rand.Next(1, A.getNbLayer());//weights of the first layer must stay 1
            int Neuron_num_mutation = rand.Next(0, A.getNbNeuronAtLayer(layer_num_mutation));
            int weigth_num_mutation = rand.Next(0, A.getNeuron(layer_num_mutation, Neuron_num_mutation).getNbWeigths());
            double mutation = (rand.NextDouble()*2-1)/10; //mutation between -0.1 and 0.1
            indiv mutator = new indiv(A, -1);
            mutator.setDataAtPos(layer_num_mutation, Neuron_num_mutation, weigth_num_mutation, A.getNeuron(layer_num_mutation, Neuron_num_mutation).getWeigth(weigth_num_mutation) + mutation);
            return mutator;
        }

        //given the selected indivs, it will decide on each one if it will cross it with another one, or mutate it, or do nothing. This decision is based on the probabilities
        //set in the constructor
        public List<indiv> applyGeneticChanges(List<indiv> pop, int totalPopSize)
        {
            List<indiv> newPop = new List<indiv>(pop);
            double geneticChangeProba;
            while(newPop.Count < totalPopSize) //the new population created needs to be superior to the size of the pop
            // because next we use simple selection to reduce the number of element of the pop
            {
                for (int i = 0; i < pop.Count; i++)
                {
                    for (int o = 0; o < pop.Count; o++)
                    {
                        if (i != o) // no need to cross one indiv with itself
                        {
                            geneticChangeProba = rand.NextDouble();
                            if (geneticChangeProba < crossoverPercent)
                            {
                                newPop.AddRange(crossover(pop[i], pop[o])); // do a crossover
                            }
                        }
                    }
                    geneticChangeProba = rand.NextDouble();
                    if (geneticChangeProba < mutationPercent)
                    {
                        newPop.Add(mutate(pop[i])); // do a mutation
                    }
                }
            }
            return newPop;
        }

        public List<indiv> applyGeneticChangesPercent(List<indiv> pop)
        {
            List<indiv> newPop = new List<indiv>(pop);
            int nbrMutations = (int)Math.Floor(mutationPercent * pop.Count);
            int nbrCrossover = (int)Math.Floor(crossoverPercent * pop.Count);
            List<indiv> shuffle = new List<indiv>(pop.OrderBy(item => rand.Next()));
            for(int i = 0; i < shuffle.Count; i++)
            {
                if (i < nbrMutations)
                {
                    newPop.Add(mutate(pop[i]));
                }
                else if(i < nbrMutations + nbrCrossover-1)
                {
                    newPop.AddRange(crossover(pop[i], pop[i + 1]));
                }
                else
                {
                    newPop.Add(new indiv(randomlyGeneratedNbNeuronsByLayers));
                }
            }
            return newPop;
        }
    }
}
