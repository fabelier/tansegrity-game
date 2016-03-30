using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ConsoleApplication1
{
    class Program : MonoBehaviour
    {
        void Start()
        {
            geneticAlgo.gradientDescent GD;
            int nb_iterations = 1000;
            int nb_indiv_in_pop = 100;
            List<int> nb_neurons_by_layers = new List<int>(new int[] { 5, 4, 4, 3});
            GD = new geneticAlgo.gradientDescent(nb_iterations, nb_indiv_in_pop, nb_neurons_by_layers);
            GD.update();
            Debug.Log(string.Format("eval value obtained : {0}", GD.getBestIndiv().getEvalValue()));
        }
    }
}
