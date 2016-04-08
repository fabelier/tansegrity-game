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
            Debug.Log(state);
            if (state == "first_frame")
            {
                state = "waiting_for_evals";
                int nb_iterations = 1000;
                int nb_indiv_in_pop = 10;
                List<int> nb_neurons_by_layers = new List<int>(new int[] { 19, 32, 8, 3 });
                GD = new geneticAlgo.gradientDescent(nb_iterations, nb_indiv_in_pop, nb_neurons_by_layers);

                
            }
            else if(state == "compute_neural_networks")
            {
                GD.generateNeighbors();
                GD.runEvals();
                state = "waiting_for_evals";
            }
            else if (state == "waiting_for_evals")
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
