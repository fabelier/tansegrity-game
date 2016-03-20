﻿using UnityEngine;
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
        public List<bool> fire(List<double> network_input)
        {
            // test
                // si input.Count == network[1].Count
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
                    if (i == 1) // First layer have to be taken differently
                    {
                        network[i][j].setInput(network_input[j], 1);
                        control = network[i][j].fire();
                    }
                    else
                    {
                        for (int k = 0; k < network[i - 1].Count; k++) // Parse the previous layer and take outputs of neurons
                        {
                            network[i][j].setInput(network[i - 1][k].getInput(k),k); 
                        }
                        control = network[i][j].fire();
                        if (i == network[i].Count - 1 && control != -1)  // At the last layer, build the output
                        {
                            if (control < threashold) tmp_output = false;
                            else tmp_output = true;
                            output.Add(tmp_output);
                        }
                    }
                    if(control == -1) // Gestion des Execption
                    {
                        Debug.Log("one Neuron returned -1 !!");
                        throw new System.Exception("Arreted in Neural_network.fire(). got bad value");
                    }
                }
            }
            return output;

        }


    }
}
