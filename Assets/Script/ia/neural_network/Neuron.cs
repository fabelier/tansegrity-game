using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Nn
{
    public struct Input
    {
        public double input;// { get; set; }
        public double weight;// { get; set; }
    }

    public class Neuron
    {
        public List<Input> inputs;
        //public double biais;
        public double fire_val { get; private set; }  // is the neuron activated ?  default: 0
 
        // ======== CONSTRUCTOR ==============================

        public Neuron()
        {
            inputs = new List<Input>();
            //biais = 0;
            fire_val = 0;
        }
        // this Neuron is initialited with random weights
        // and Input.input values at 0
        public Neuron(int n_weights, System.Random rand)
        {
            // Input fill
            Input tmp;
            this.inputs = new List<Input>();
            //biais = rand.NextDouble();
            for (int i = 0; i < n_weights; i++)
            {
                tmp = new Input();
                tmp.weight = rand.NextDouble(); tmp.input = 0;
                inputs.Add(tmp);
            }
        }
        //public Neuron(List<double> weight, double biais)
        public Neuron(List<double> weight)
        {
            // Input fill
            Input tmp;
            //this.biais = biais;
            this.inputs = new List<Input>();
            for (int i = 0; i < weight.Count; i++)
            {
                tmp = new Input();
                tmp.weight = weight[i]; tmp.input = 0;
                inputs.Add(tmp);
            }
        }

        //public Neuron(List<double> weight , List<double> input, double biais)
        public Neuron(List<double> weight, List<double> input)
        {
            if(weight.Count != input.Count)
            {
                Debug.Log("The weight and the input list have different size ! What did you expect ?");
                throw new IndexOutOfRangeException();
            }
            //this.biais = biais;
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
        //public Neuron(List<Input> inputs, double biais)
        public Neuron(List<Input> inputs)
        {
            //this.biais = biais;
            // Input fill
            this.inputs = new List<Input>(inputs);
            // Take the descision
            main(inputs);
        }

        //========== Methods =======================================

        // ===== GET/SET =======================================
        public void setWeight(double weights, int position) {
            if (inputs.Count > position)
            {
                double save_input = this.inputs[position].input;
                this.inputs[position] = new Input { input = save_input, weight = weights };
            }
            else
            {
                Debug.Log("try to set weight in a too high position");
                throw new IndexOutOfRangeException();
            }
        }
        public void setInput(double input, int position)
        {
            if (inputs.Count > position)
            {
                double save_weight = this.inputs[position].weight;
                this.inputs[position] = new Input { input = input, weight = save_weight  };
            }
            else
            {
                Debug.Log(string.Format("try to set input in position : {0}, but there is only {1} input", position, inputs.Count));
                throw new IndexOutOfRangeException();
            }
        }
        public void setInput(Input input, int position)
        {
            if( inputs.Count > position)
                inputs[position] = input;
            else
            {
                Debug.Log("setInput: try to set Input in a too high position");
                throw new IndexOutOfRangeException();
            }
        }
        //public void setBiais(double biais)
        //{
        //    this.biais = biais;
        //}

        public double getInput(int position)
        {
            if (inputs.Count > position)
                return inputs[position].input;
            else
            {
                Debug.Log("getInput :try to get input in a too high position");
                throw new IndexOutOfRangeException();
            }
        }
        public double getWeigth(int position)
        {
            if (inputs.Count > position)
                return inputs[position].weight;
            else
            {
                Debug.Log("getInput :try to get input in a too high position");
                throw new IndexOutOfRangeException();
            }
        }
       // public double getBiais()
        //{
        //    return biais;
        //}
        // Get the complete list of inputs stored in the Input struct
        public List<double> getAllInput()
        {
            List<double> tmp_input = new List<double>();
            for (int i = 0; i < inputs.Count; i++)
            {
                tmp_input.Add(inputs[i].input);
            }
            return tmp_input;
        }
        public int getNbWeigths()
        {
            return inputs.Count;
        }

        // ===== TO VISUALIZE ===================================

        public string toString(bool debug)
        {
            string str = "";
            //string str = string.Format("\t Biais : {0}\n",biais);
            //str = string.Format("Neuron have : \n");
            for (int i = 0; i < inputs.Count; i++)
            {
                str += string.Format("\t \t Input {0} :input ={1}, weight= {2} \n", i,inputs[i].input,inputs[i].weight);
            }
            if (debug) Debug.Log(str);
            return str;
        }

        // ===== USEFUL Methods =================================

        private void main(List<Input> inputs)
        {
            double val = combinaison(inputs);
            activation(val);
        }
        public double fire()
        {
            try
            {
                return activation(combinaison(inputs));
            }
            catch
            {
                Debug.Log("You have not set the Inputs or the biais of the neuron correctly");
                //throw new TypeInitializationException('Neuron',ArgumentNullException de); // J'arrive pas a gérer l'exeption
                throw new Exception("You do not have set the input, I guess");
            }
        }

        // Function that sum up the weigthed values returned by the input neurons 
        public double combinaison(List<Input> inputs) {
            double val = 0;

            for (int i = 0; i < inputs.Count; i++){
                val = val + inputs[i].weight * inputs[i].input;
            }
            //val += biais;

            return val;
        }

        // Function that descide if the neuron fire
        // val    : must be in [-1,1]
        // lambda : in orther to control the slope of the function, the more lambda is high, the more the slope is important
        public double activation(double val, double lambda =1.0){

            // ACTIVATION FUNCTION
            val = 1 / (1 + Mathf.Exp( (float) (-lambda * val)));  // sigmoid function
            this.fire_val = val;
            return val;
        }
    }
}
