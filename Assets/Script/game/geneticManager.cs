using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.game
{
    class geneticManager : MonoBehaviour
    {
        public string state;
        geneticAlgo.gradientDescent GD;

        void Start()
        {
            state = "first_frame";
            //Debug.Log(string.Format("eval value obtained : {0}", GD.getBestIndiv().getEvalValue()));
        }

        void Update()
        {
            if (state == "first_frame")
            {
                int nb_iterations = 1000;
                int nb_indiv_in_pop = 100;
                List<int> nb_neurons_by_layers = new List<int>(new int[] { 5, 4, 4, 3 });
                GD = new geneticAlgo.gradientDescent(nb_iterations, nb_indiv_in_pop, nb_neurons_by_layers);
                state = "waiting_for_evals";
            }
            if(state== "compute_neural_networks")
            {
                GD.generateNeighbors();
                GD.runEvals();
                state = "waiting_for_evals";
            }
            if (state == "waiting_for_evals")
            {
                if (GD.areEvalsFinished())
                {
                    GD.changePop();
                    state = "compute_neural_networks";
                }
            }


        }
    }
}
