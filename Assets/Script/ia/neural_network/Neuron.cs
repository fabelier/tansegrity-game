using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nn
{
    public struct Input
    {
        public double input { get; set; }
        public double weight { get; set; }
    }

    public class Neuron  // enlever le MonoBehaviour
    {
        public List<Input> inputs;
        public double fire_val { get; private set; }  // is the neuron activated ?  default: FALSE
        //public int in_layer { get; set; }
        //public int in_position { } 

        private System.Random rand;
        
        // ======== CONSTRUCTOR ==============================

        // this Neuron is initialited with random weights
        // and Input.input values at 0
        public Neuron(int n_weights)
        {
            // Input fill
            Input tmp;
            this.inputs = new List<Input>();
            rand = new System.Random();
            for (int i = 0; i < n_weights; i++)
            {
                tmp = new Input();
                tmp.weight = rand.NextDouble(); tmp.input = 0;
                inputs.Add(tmp);
            }
        }

        public Neuron(List<double> weight){
            // Input fill
            Input tmp;
            this.inputs = new List<Input>();
            for (int i = 0; i < weight.Count; i++)
            {
                tmp = new Input();
                tmp.weight = weight[i]; tmp.input = 0;
                inputs.Add(tmp);
            }
        }
        
        public Neuron(List<double> weight , List<double> input)
        {
            if(weight.Count != input.Count)
            {
                Debug.Log("The weight and the input list have different size ! What did you expect ?");
                throw new IndexOutOfRangeException();
            }
            // Input fill
            Input tmp;
            this.inputs = new List<Input>();
            for (int i = 0; i < weight.Count; i++)
            {
                tmp = new Input();
                tmp.weight = weight[i]; tmp.input = input[i];
                inputs.Add(tmp);
            }
            // Take the descision
            main(inputs);
        }

        // The most secure way to construct the Neuron class with the
        // Input list made. 
        public Neuron(List<Input> inputs)
        {
            // Input fill
            this.inputs = new List<Input>(inputs);
            // Take the descision
            main(inputs);
        }
        //========== Methods =======================================

            // ===== GET/SET =======================================
        public void setWeight(double weights, int position) {
            if (inputs.Count <= position)      
                inputs[position].weight = weights;
            else
            {
                Debug.Log("try to set weight in a too high position");
                throw new IndexOutOfRangeException();
            }
        }

        public void setInput(double input, int position)
        {
            if (inputs.Count <= position)
                inputs[position].input = input;
            else
            {
                Debug.Log("try to set input in a too high position");
                throw new IndexOutOfRangeException();
            }
        }

        public void setInput(Input input, int position)
        {
            if( inputs. Count <= position)
                inputs[position] = input;
            else
            {
                Debug.Log("try to set Input in a too high position");
                throw new IndexOutOfRangeException();
            }
        }

           // ===== USEFUL Methods =================================

        private void main(List<Input> inputs)
        {
            double val = combinaison(inputs);
            activation(val);
        }

        // Function that sum up the weigthed values returned by the input neurons 
        public double combinaison(List<Input> inputs) {
            double val = 0;

            for (int i = 0; i < inputs.Count; i++){
                val = val + inputs[i].weight * inputs[i].input;
            }

            return val;
        }


        // Function that descide weather the neuron fire
        // val    : must be in ]0,1]
        // lambda : in orther to control the slope of the function, the more lambda is high, the more the slope is important
        public double activation(double val, double lambda =1.0){

            // ACTIVATION FUNCTION
            val = 1 / (1 + Mathf.Exp( (float) (-lambda * val)));  // sigmoid function
            this.fire_val = val;
            return val;
        }
    }
}
