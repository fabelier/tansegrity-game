using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Nn {

    public class Neural_network {
        public List<List<Neuron>> network;
        List<bool> output;

        // ====== CONSTRUCTORS ==================

        // Make a neural network with all input at zero and random weights
        // minimal constructor
        public Neural_network(int n_Layer, List<int> n_neuronIlayer)
        {
            Neuron tmp;
            List<Neuron> tmp_layer;
            network = new List<List<Neuron>>();
            for (int i = 0; i < n_Layer; i++)
            {
                tmp_layer = new List<Neuron>();
                for (int j = 0; j < n_neuronIlayer[i]; j++)
                {
                    if (i == 1)  // special treatment for first layer, that take input of the network
                    {                                                // for this case the weight is    
                        List<double> n = new List<double>(); n.Add(1); // a List of one element valued at 1 
                        tmp = new Neuron(n);
                    }
                    else
                    {
                        tmp = new Neuron(n_neuronIlayer[i - 1]); // fill Input with n_neuronIlayer[i - 1] random values
                    }
                    tmp_layer.Add(tmp);
                }
                this.network.Add(tmp_layer);
            }
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
        }


        // ===== METHODS =========================

            // = GET/SET =========================

       
        

            // = USEFULL METHODS =================
        public List<bool> fire(List<double> input)
        {
            // test
                // si input.Count == network[1].Count
                // tout les input 0<...<1
            // fait le transfert layer par layer des input

        }


    }
}
