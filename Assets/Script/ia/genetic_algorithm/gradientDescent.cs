using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace geneticAlgo
{
    class gradientDescent
    {
        indiv pop;
        int nbIterationMax;
        int nbNeighbors;
        Random rand;

        public gradientDescent()
        {
            this.nbIterationMax = 1000;
            this.nbNeighbors = 100;
            createDebugpop();
            this.pop.eval();
            rand = new Random();
        }
        public gradientDescent(int nbIterationMax, int nbNeighbors)
        {
            this.nbIterationMax = nbIterationMax;
            this.nbNeighbors = nbNeighbors;
            createDebugpop();
            this.pop.eval();
            rand = new Random();
        }

        public void update()
        {
            List<indiv> neighborhood;
            for (int i = 0; i < nbIterationMax; i++)
            {
                neighborhood = new List<indiv>();
                neighborhood.Add(pop);
                indiv neighboor;
                while(neighborhood.Count != nbNeighbors)
                {
                    neighboor = mutate(pop);
                    if (!neighborhood.Contains(neighboor))
                    {
                        neighboor.eval();
                        neighborhood.Add(neighboor);
                    }
                }
                //Console.WriteLine("before :");
                //for(int j = 0; j < neighborhood.Count; j++) { Console.WriteLine("\t{0}", neighborhood[j]); }
                neighborhood = neighborhood.OrderByDescending(o => o.getEvalValue()).ToList();
                //Console.WriteLine("after : ");
                //for (int j = 0; j < neighborhood.Count; j++) { Console.WriteLine("\t{0}", neighborhood[j]); }
                pop = neighborhood[0];
                Console.WriteLine("iteration n°{0}", i);
                debuging();
            }
        }
        public indiv mutate(indiv pop)
        {
            double mutation = rand.NextDouble() * 0.2 - 0.1;
            indiv mutant = new indiv(pop.getData() + mutation);
            return (mutant);
        }
        public void createDebugpop()
        {
            double tmp = new Random().NextDouble();
            pop = new indiv(tmp);
        }
        public void debuging()
        {
            Console.WriteLine("\tpop : {0}",pop.getData());
            Console.WriteLine("\tEval Value : {0}", pop.getEvalValue());
        }
    }
}
