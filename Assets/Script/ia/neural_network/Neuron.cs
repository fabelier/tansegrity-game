using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct Input{
    public bool input { get; set; }
    public double weight { get; set; }
}

public class Neuron : MonoBehaviour {
    public List<Input> inputs;
    public double thershold { get; set; }
    public bool fire { get; private set; }  // is the neuron activated ?  default: FALSE 

    // constructor
    /*public Neuron(List<Input> inputs){
        this.inputs = inputs;
        thershold = 0.5;
    }*/


    //========== Methods =======================================


    /*public void setWeights(double weight, int position) {
        if (inputs.Count <= position)
        {
            this.inputs[position].weight = weight;
        }
        else  print("try to set weight in a too high position");
        
    }*/

    // Function that sum up the weigthed values returned by the input neurons 
    /*public double combinaison(List<Input> inputs) {
        double val = 0;
        int tmp_input;

        for (int i = 0; i < inputs.Count; i++){
            if (inputs[i].input) tmp_input = 1; // transform the bool value in integer
            else tmp_input = 0;

            val = val + inputs[i].weight * tmp_input;
        }

        return val;
    }*/


    // Function that descide weather the neuron fire
    // val    : must be in ]0,1]
    // lambda : in orther to control the slope of the function, the more lambda is high, the more the slope is important
    /*public bool activation(double val, double lambda =1.0){
        bool descision;

        val = 1 / (1 + Mathf.Exp( (float) (-lambda * val)));  // sigmoid function
        if (val < 0.5){
            descision = false;
        }
        else{
            descision = true;
        }
        return descision;
    }
    */
	// Use this for initialization
	void Start (List<Input> inputs) {
	
	}
	
	// Update is called once per frame
	void Update(){
        double val = 0, lambda = 1; // 
        int tmp_input;
        // Get the input

        // =============================================
        // sum the values   : COMBINAISON
        for (int i = 0; i < inputs.Count; i++)
        {
            if (inputs[i].input) tmp_input = 1; // transform the bool value in integer
            else tmp_input = 0;

            val = val + inputs[i].weight * tmp_input;
        }

        // ============================================
        // compute if the neuron will fire or not
        val = 1 / (1 + Mathf.Exp((float)(-lambda * val)));  // sigmoid function  <--------------
        if (val < 0.5)
        {
            fire = false;
        }
        else {
            fire = true;
        }

    }
}
