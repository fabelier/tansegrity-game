using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geneticAlgo
{
    class geneticOperator
    {
        double crossoverProba;
        double mutationProba;
        Random rand;

        public geneticOperator()
        {
            crossoverProba = 0.7;
            mutationProba = 0.01;
            rand = new Random();
        }
        public List<indiv> crossover(indiv A, indiv B)
        {
            int posCrossingover = rand.Next(0, A.getSize());
            indiv A2 = new indiv(A.getData().GetRange(0, posCrossingover).Concat(B.getData().GetRange(posCrossingover, B.getSize() - posCrossingover)).ToList());
            indiv B2 = new indiv(B.getData().GetRange(0, posCrossingover).Concat(A.getData().GetRange(posCrossingover, A.getSize() - posCrossingover)).ToList());
            List<indiv> newIndivs = new List<indiv>();
            newIndivs.Add(A2);
            newIndivs.Add(B2);
            return newIndivs;
        }
        public indiv mutate(indiv A)
        {
            int posMutation = rand.Next(0, A.getSize());
            double mutation = (rand.NextDouble()*2-1)/10; //mutation between -0.1 and 0.1
            indiv mutator = new indiv(A.getData());
            mutator.setDataAtPos(posMutation, A.getData()[posMutation] + mutation);

            return mutator;
        }
        public List<indiv> applyGeneticChanges(List<indiv> pop)
        {
            List<indiv> newPop = new List<indiv>(pop);
            double geneticChangeProba;
            for (int i = 0; i < pop.Count; i++)
            {
                for (int o = 0; o < pop.Count; o++)
                {
                    if (i != o)
                    {
                        geneticChangeProba = rand.NextDouble();
                        if (geneticChangeProba < crossoverProba)
                        {
                            newPop.AddRange(crossover(pop[i], pop[o]));
                        }
                    }
                }
                geneticChangeProba = rand.NextDouble();
                if (geneticChangeProba < mutationProba)
                {
                    newPop.Add(mutate(pop[i]));
                }
            }
            return newPop;
        }
    }
}
