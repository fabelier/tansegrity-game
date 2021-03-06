﻿using System;
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
        GameObject gameManager;
        private bool load = false;

        void Start()
        {
            gameManager = GameObject.Find("GameManager(Clone)");
            state = "first_frame";
            Application.runInBackground = true;
        }

        void Update()
        {
            //Debug.Log(state);
            if (state == "first_frame")
            {
                //load = gameManager.GetComponent<GameManager>().getLoad();
                load = false;
                if (load == true)
                {
                    Debug.Log("load == true");
                    GD = new gradientDescent(gradientDescent.ReadFromXmlFile<gradientDescent>("savedEvolution/test1.xml"));
                }
                else if (load == false)
                {
                    Debug.Log("load == false");
                    int nb_iterations = 10000;
                    int nb_indiv_in_pop = 32;
                    bool doTournament = true;
                    double mutationPercent = 0.8;
                    double crossoverPercent = 0.1;
                    List<int> nb_neurons_by_layers = new List<int>(new int[] { 18, 16, 8, 3 });
                    GD = new gradientDescent(nb_iterations, nb_indiv_in_pop, nb_neurons_by_layers, doTournament, mutationPercent, crossoverPercent);
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


        void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, h*0.98f, w, h*2/100);
            style.alignment = TextAnchor.LowerLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            string text = string.Format("best fitness : {0}, iteration : {1}", GD.getBestIndiv().getEvalValue(), GD.iteration);
            GUI.Label(rect, text, style);

            if (GUILayout.Button("Save"))
            {
                gradientDescent.WriteToXmlFile("savedEvolution/test1.xml", GD);
            }
        }
    }
}
