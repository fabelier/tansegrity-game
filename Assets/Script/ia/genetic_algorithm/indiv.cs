using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.game;

namespace geneticAlgo
{
    class indiv
    {
        Nn.Neural_network Nn;
        double evalValue;//-1 represent the eval not computed

        // ====== CONSTRUCTORS ==================

        // basic constructor
        public indiv()
        {
            Nn = new Nn.Neural_network();
            evalValue = -1;
        }

        //create an indiv with a neural network containing size_layers neurons by layers
        public indiv(List<int> size_layers)
        {
            this.Nn = new Nn.Neural_network(size_layers);
            this.evalValue = -1;
        }

        // copy an indiv
        public indiv(indiv I)
        {
            this.Nn = new Nn.Neural_network(I.getNn());
            //this.data = new List<double>(I.getData());
            evalValue = I.getEvalValue();
        }

        //create an indiv with a specified neural network Nn
        public indiv(List<List<Nn.Neuron>> Nn)
        {
            this.Nn = new Nn.Neural_network(Nn);
            evalValue = -1;
        }

        //create an indiv with a neural network containing the weigths specified in data
        public indiv(List<List<List<double>>> data)
        {
            this.Nn = new Nn.Neural_network(data);
            this.evalValue = -1;
        }

        //same as before but with the evalvalue already computed
        public indiv(List<List<List<double>>> data, double evalValue)
        {
            this.Nn = new Nn.Neural_network(data);
            this.evalValue = evalValue;
        }

        // ====== METHODS ==================

        //create a tansegrity structure controlled by the Nn on the environement
        public void eval()
        {
            evalValue = 0;
            GameObject tansegrityStruct = Instantiate(Resources.Load("Link")) as GameObject;
            tansegrityStruct.init(this);
            //tansegrity_creator tansegrityStruct = new tansegrity_creator(this); //create a tansegrity structure in the environnement which will then set the evalvalue once updates finished
        }

        //check if unity has finished to evaluate the indiv in the environnement
        public bool isEvalFinished()
        {
            return evalValue != -1;
        }

        //add a double to all weigths of all neurons of all layers of the neural network
        public void addDoubleToWeigths(double d)
        {
            for(int layer_num = 0; layer_num < Nn.getNetwork().Count; layer_num++)
            {
                for(int Neuron_num = 0; Neuron_num < Nn.getNetwork()[layer_num].Count; Neuron_num++)
                {
                    for (int weigth_num = 0; weigth_num < Nn.getNetwork()[layer_num][Neuron_num].getNbWeigths(); weigth_num++)
                    {
                        Nn.setWeigth(Nn.getWeigth(layer_num, Neuron_num, weigth_num)+d, layer_num, Neuron_num, weigth_num);
                    }
                }
            }
        }

        // ====== GET/SET ==================

        //set one weigth of the neural network to newData
        public void setDataAtPos(int layer_num, int Neuron_num, int weigth_num, double newData)
        {
            Nn.setWeigth(newData, layer_num, Neuron_num, weigth_num);
        }

        //getter/setter for Nn
        public void setNeuralNetwork(Nn.Neural_network Nn)
        {
            this.Nn = Nn;
        }
        public Nn.Neural_network getNn()
        {
            return Nn;
        }

        //return one layer of the neural network
        public List<Nn.Neuron> getLayer(int layer_num)
        {
            return Nn.getLayer(layer_num);
        }

        //return all layers of Nn between startpos and endpos 
        public List<List<Nn.Neuron>> getLayersRange(int startpos, int endpos)
        {
            List<List<Nn.Neuron>> layers = new List<List<Nn.Neuron>>();
            for (int i = startpos; i < endpos; i++)
            {
                layers.Add(Nn.getLayer(i));
            }
            return layers;
        }

        //return one neuron of Nn
        public Nn.Neuron getNeuron(int layer_num, int Neuron_num)
        {
            return Nn.getNeuron(layer_num, Neuron_num);
        }


        public int getNbLayer()
        {
            return Nn.getNbLayer();
        }
        public int getNbNeuronAtLayer(int layer_num)
        {
            return Nn.getNbNeuronAtLayer(layer_num);
        }
        public double getEvalValue()
        {
            return evalValue;
        }
        public void setEvalValue(double eval)
        {
            this.evalValue = eval;
        }
        public override string ToString()
        {
            string dataString="";
            for(int i = 0; i < Nn.getNbLayer(); i++)
            {
                dataString += "Layer " + i.ToString() + " : \n";
                for(int y = 0; y < Nn.getNbNeuronAtLayer(i); y++)
                {
                    dataString += "\tNeuron "+ y.ToString() + " : \n\t\tweigth | input\n";
                    for(int j = 0; j < Nn.getNeuron(i, y).getNbWeigths(); j++)
                    {
                        dataString += "\t\t" + Nn.getNeuron(i, y).getWeigth(j).ToString() + " | " + Nn.getNeuron(i, y).getInput(j).ToString()+"\n";
                    }
                }
            }
            return("Neural_network : "+dataString+"\nevalValue : "+evalValue.ToString());
        }
    }
}
