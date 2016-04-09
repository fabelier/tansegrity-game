using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using geneticAlgo;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.Script.game
{
    class geneticManager : MonoBehaviour
    {
        public string state;
        gradientDescent GD;

        void Start()
        {
            state = "first_frame";
          
        }

        void Update()
        {
            //Debug.Log(state);
            if (state == "first_frame")
            {
                int load = 1;
                if (load == 1)
                {
                    GD = new gradientDescent(gradientDescent.ReadFromXmlFile<gradientDescent>("savedEvolution/test1.xml"));
                }
                else
                {
                    int nb_iterations = 1000;
                    int nb_indiv_in_pop = 32;
                    List<int> nb_neurons_by_layers = new List<int>(new int[] { 19, 32, 8, 3 });
                    GD = new gradientDescent(nb_iterations, nb_indiv_in_pop, nb_neurons_by_layers);
                }

                state = "evaluation";

            }
            else if(state == "compute_neural_networks")
            {
                GD.generateNeighbors();
                GD.runEvals();
                state = "evaluation";
            }
            else if (state == "evaluation")
            {//this will wait for structures to update until the end of their evaluation
                if (GD.areEvalsFinished())
                {
                    GD.changePop();
                    state = "compute_neural_networks";
                }
            }


        }
        void OnDestroy()
        {
            gradientDescent.WriteToXmlFile("savedEvolution/test1.xml", GD);
        }
    }
}
