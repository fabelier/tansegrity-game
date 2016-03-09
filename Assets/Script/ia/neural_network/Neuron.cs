using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Nn
{
    public struct Input
    {
        public bool input { get; set; }
        public double weight { get; set; }
    }

    public class Neuron  // enlever le MonoBehaviour
    {
        public List<Input> inputs;
        public double thershold { get; set; }
        public bool fire { get; private set; }  // is the neuron activated ?  default: FALSE 

        private Random rand = new Random();
        
        // ======== CONSTRUCTOR ==============================

        // this Neuron is initialited with random weights
        public Neuron(int n_weights)
        {
            Input tmp;
            this.inputs = new List<Input>();

            for (int i = 0; i < n_weights; i++)
            {
                tmp = new Input();
                double p = rand.N
            }

        }

        public Neuron(List<double> weight){
            this.inputs = new List<Input>;
            thershold = 0.5;
        }


        //========== Methods =======================================


        public void setWeights(double weight, int position) {
            if (inputs.Count <= position)
            {
                this.inputs[position].weight= weight;
            }
            else  Debug.Log("try to set weight in a too high position");

        }

        // Function that sum up the weigthed values returned by the input neurons 
        public double combinaison(List<Input> inputs) {
            double val = 0;
            int tmp_input;

            for (int i = 0; i < inputs.Count; i++){
                if (inputs[i].input) tmp_input = 1; // transform the bool value in integer
                else tmp_input = 0;

                val = val + inputs[i].weight * tmp_input;
            }

            return val;
        }


        // Function that descide weather the neuron fire
        // val    : must be in ]0,1]
        // lambda : in orther to control the slope of the function, the more lambda is high, the more the slope is important
        public bool activation(double val, double lambda =1.0){
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
    }
}
