using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neuron : MonoBehaviour {
    double output;
    List<double> input;
    List<double> weights { get; set; }
    double thershold { get; set;}

    public Neuron(List<double> input){
        this.input = input;
    }
       
    public void setWeights(double weigth, int position){
        this.weights[position] = weigth;
    }

    public double combinaison(){




    }


    // Function that descide weather the neuron fire
    // val    : must be in ]0,1]
    // lambda : in orther to control the slope of the function, the more lambda is high, the more the slope is important
    public bool activation(double val, double lambda =1.0){
        bool descision;

        val = 1 / (1 + Mathf.Exp( (float) -lambda * val));  // sigmoid function
        if (val < 0.5){
            descision = false;
        }
        else{
            descision = true;
        }
        return descision;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
