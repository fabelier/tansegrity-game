using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Nn;
using geneticAlgo;

public class test_Nn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //List<double> input = new List<double> { 0.5, .65, .6, .1 };
        //List<double> weight = new List<double> { 0.4, .25, .134, .21 };
        //Neuron neu = new Neuron(weight, input);
        //neu.toString();

        List<int> n_layer_ect = new List<int>() { 19, 32, 8, 3 };
        indiv prout = new indiv(n_layer_ect);

        
        //prout.toString();
        GameObject link = UnityEngine.Object.Instantiate(Resources.Load("Link")) as GameObject;
        link.GetComponent<LinkTansegrity_IA>().Init(prout);
       

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
