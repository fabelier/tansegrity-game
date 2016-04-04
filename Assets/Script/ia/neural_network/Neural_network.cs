using UnityEngine;
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
                        tmp = new Neuron(n); 
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
            for (int i = 0; i < network_weight.Count; i++)
            {
                tmp_layer = new List<Neuron>();
                for (int j = 0; j < network_weight[i].Count; j++)
                {
                    tmp_weight = new List<double>(network_weight[i][j]);
                    tmp = new Neuron(tmp_weight);
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
            network = neuroNet.getNetwork();
            output = neuroNet.getOutput();
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

            // tout les input 0<...<1
            // fait le transfert layer par layer des input
            double control;
            double threashold = 0.5; // advice = 0.5
            bool tmp_output;
            List<bool> output = new List<bool>();
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
                        for (int k = 0; k < network[i - 1].Count; k++) // Parse the previous layer and take outputs of neurons
                        {
                            network[i][j].setInput(network[i - 1][k].fire_val,k);  // get the output of Neurons in layer i-1
                        }
                        control = network[i][j].fire();
                        
                        if (i == network.Count - 1 && control != -1)  // At the last layer, build the output
                        {
                            //toString();
                            if (control < threashold) tmp_output = false;  // threshold mis a 0.5
                            else tmp_output = true;
                            output.Add(tmp_output);
                        }
                    }
                    if(control == -1) // Gestion des Execption
                    {
                        Debug.Log("one Neuron returned -1 !!");
                        throw new System.Exception("stop in Neural_network.fire(). got bad value");
                    }
                }
            }
            return output;

        }

    }
}
