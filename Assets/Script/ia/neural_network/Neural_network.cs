﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Nn {

    public class Neural_network {
        public List<List<Neuron>> network;
        List<bool> output;

        private System.Random rand;

        // ====== CONSTRUCTORS ==================
        // minimal constructor
        public Neural_network()
        {
            network = new List<List<Neuron>>();
            output = new List<bool>();
        }
        // Make a neural network with all input at zero and random weights
        public Neural_network( List<int> n_neuronIlayer)
        {
            Neuron tmp;
            List<Neuron> tmp_layer;
            network = new List<List<Neuron>>();
            int n_Layer = n_neuronIlayer.Count;
            rand = new System.Random();
            for (int i = 0; i < n_Layer; i++)
            {
                tmp_layer = new List<Neuron>();
                for (int j = 0; j < n_neuronIlayer[i]; j++)
                {
                    if (i == 0)  // special treatment for first layer, that take input of the network
                    {                                                // for this case the weight is    
                        List<double> n = new List<double>(); n.Add(1); // a List of one element valued at 1
                        tmp = new Neuron(n, rand, false); 
                        //tmp = new Neuron(n,0);    // and the biais is set to 0
                    }
                    else
                    {
                        tmp = new Neuron(n_neuronIlayer[i - 1], rand); // fill Input with n_neuronIlayer[i - 1] random values
                    }
                    tmp_layer.Add(tmp);
                }
                this.network.Add(tmp_layer);
            }
            output = new List<bool>();

        }
        // If specific weight are needed use this constructor
        public Neural_network(List<List<List<double>>> network_weight)
        {
            Neuron tmp;
            List<Neuron> tmp_layer;
            List<double> tmp_weight;
            network = new List<List<Neuron>>();
            rand = new System.Random();
            for (int i = 0; i < network_weight.Count; i++)
            {
                tmp_layer = new List<Neuron>();
                for (int j = 0; j < network_weight[i].Count; j++)
                {
                    tmp_weight = new List<double>(network_weight[i][j]);
                    tmp = new Neuron(tmp_weight, rand);
                    tmp_layer.Add(tmp);
                }
                this.network.Add(tmp_layer);
            }
            output = new List<bool>();
        }
        public Neural_network(List<List<Neuron>> Nn)
        {
            network = new List<List<Neuron>>(Nn);
            output = new List<bool>();
        }
        public Neural_network(Neural_network neuroNet)
        {
            network = new List<List<Neuron>>(neuroNet.getNetwork());
            output = new List<bool>(neuroNet.getOutput());
        }

        // ===== METHODS =========================
            // = GET/SET =========================

        public int getNbLayer()
        {
            return network.Count;
        }
        public int getNbNeuronAtLayer(int layer_num)
        {
            return network[layer_num].Count;
        }
        public List<List<Neuron>> getNetwork()
        {
            return this.network;
        }
        public List<Neuron> getLayer(int layer_num)
        {
            return network[layer_num];
        }
        public Neuron getNeuron(int layer_num, int Neuron_num)
        {
            return network[layer_num][Neuron_num];
        }
        public List<bool> getOutput()
        {
            return this.output;
        }
        public void setWeigth(double w, int layer_num, int Neuron_num, int weigth_num)
        {
            network[layer_num][Neuron_num].setWeight(w, weigth_num);
        }
        public double getWeigth(int layer_num, int Neuron_num, int weigth_num)
        {
            return network[layer_num][Neuron_num].getWeigth(weigth_num);
        }
        public void addNeuron(int layer_num, Neuron neur)
        {
            network[layer_num].Add(neur);
        }

            // = To visualize ====================

        public void toString()
        {
            string str = "";
            for (int i = 0; i < network.Count; i++)
            {
                str += string.Format("Layer {0} \n", i);
                for (int j= 0; j < network[i].Count; j++)
                {
                    str += string.Format("     Neuron {0} :\n {1}", j, network[i][j].toString(false));
                }
            }
            Debug.Log(str);
        }

        public string size()
        {
            
            string str = string.Format("  nbLayer :" + network.Count + "\n");
            for (int i = 0; i < network.Count; i++)
            {
               str += string.Format("Layer {0}:{1}  ", i, network[i].Count );
            }
            return str;
        }

            // = USEFULL METHODS =================
        public List<bool> fire(List<double> network_input)
        {
            // test
                // si la taille de network_input n'est pas coherente == erreur 
            if(network_input.Count != network[0].Count)
            {
                Debug.Log("You may have as many inputs as the number of neuron in the first layer.");
                throw new System.Exception();
            }

            // tout les input -1<...<1
            // fait le transfert layer par layer des inputs
            double control;
            double threashold = 0.5; // advice = 0
            bool tmp_output;
            output.Clear();
            //List<bool> output = new List<bool>();
            for (int i = 0; i < network.Count; i++)  // boucle in network
            {
                for (int j = 0; j < network[i].Count; j++) // boucle in each layer to set the inputs
                {
                    if (i == 0) // First layer have to be taken differently
                    {
                        network[i][j].setInput(network_input[j], 0);
                        control = network[i][j].fire();
                    }
                    else
                    {
                        network[i][j].setInput(1, 0);//bias
                        for (int k = 0; k < network[i - 1].Count; k++) // Parse the previous layer and take outputs of neurons
                        {
                            network[i][j].setInput(network[i - 1][k].fire_val,k+1);  // get the output of Neurons in layer i-1
                        }
                        control = network[i][j].fire();
                        
                        if (i == network.Count - 1 && control != -1)  // At the last layer, build the output
                        {
                            //toString();
                            //Debug.Log("control : " + control);
                            if (control < threashold) tmp_output = false;
                            else tmp_output = true;
                            output.Add(tmp_output);
                        }
                    }
                    if(control == -1) // Gestion des Exception
                    {
                        Debug.Log("one Neuron returned -1 !!");
                        throw new System.Exception("stop in Neural_network.fire(). got bad value");
                    }
                }
            }
            return output;

        }

        public void gradientRetropropagation(List<bool> targetOutput, double lambda = 0.4)
        {

            if (output.Count == 0) throw new System.Exception("Did you even fire before retropropagate ?");
            if(targetOutput.Count != output.Count)
                throw new System.Exception("Bad targetOutput in args");

            List<Neuron> layer;
            List<double> tmpWeight;

            // set the prediction error of the network 
            List<List<double>> globalError = new List<List<double>>();
            for (int i = 0; i < network.Count-1 ; i++){ globalError.Add( new List<double>()) ; }

            List<double> error = predictionError(targetOutput);

            
            // retroPropagate the error
            for (int i = network.Count-1; i>=1; i--)  // reverse parse of the network 
            {
                layer = network[i];
                if(i != network.Count -1)    error = new List<double>();
                for (int j = 0; j < layer.Count; j++)      // in each layer
                {
                    // take the output layer 
                    if (i == network.Count -1 ) // derivate of the neuronal activation fonction
                        error[j] = error[j] * sigmoidDerivate(layer[j].combinaison());
                    else
                    {
                        tmpWeight = new List<double>();
                        
                        foreach (Neuron neuron in network[i+1])  // get weights of the sinapse in beetween the neuron i,j and the next layer
                        {
                            tmpWeight.Add(neuron.getWeigth(j + 1)); //  Take care that the fisrt Input is the bias;
                        }
                        error.Add(sigmoidDerivate(layer[j].combinaison()) * prod(globalError[i], tmpWeight)); // error is still 
                    }
                }
                globalError[i-1] = error;
            }

            // CORRECT THE WEIGHTS
            double weight;
            for (int i = 1; i < network.Count; i++) // parse the different layers
            {
                for (int j = 0; j < network[i].Count; j++) // in layer n
                {
                    for (int k = 0; k < network[i-1].Count; k++) // you have to consider fire val from layer n - 1
                    {
                        weight = network[i][j].getWeigth(k + 1);
                        weight += lambda * globalError[i-1][j] * network[i - 1][k].fire_val; // global Error do not have an error for the first layer
                        network[i][j].setWeight(weight,k + 1);
                    }
                }
            }

        }

        // Return a list containing { -1, 1, 0 }
        private List<double> predictionError(List<bool> target)
        {
            List<double> error = new List<double>();
            for(int i = 0; i < target.Count ;i++)
            {
                if (target[i] == true && output[i] == false) error.Add( 1);
                else if (target[i] == false && output[i] == true) error.Add( -1);  // il y a une erreur là
                else error.Add( 0);
            }
            return error;
        }


        // return the sigmoid derivate
        private double sigmoidDerivate(double val, double lambda = 0.2)
        {
            return -System.Math.Exp(-lambda * val) / System.Math.Pow((1 + System.Math.Exp(-lambda * val)), 2);
        }

        // product of two list
        private double prod(List<double> list1, List<double> list2 )
        {
            if (list1.Count != list2.Count) throw new System.Exception("The two list have to have the same size");
            double val = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                val += list1[i] * list2[i];
            }
            return val;
        }

    }
}
