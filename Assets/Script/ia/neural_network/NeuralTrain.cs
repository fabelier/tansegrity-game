using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Nn;

public class NeuralTrain {

    private List<DataInputOutput> data;
    private Neural_network network;

    public NeuralTrain(List<DataInputOutput> data, Neural_network network)
    {
        this.data = data;
        this.network = network;
    }

    public NeuralTrain(List<DataInputOutput> data)
    {
        this.data = data;
        network = new Neural_network(new List<int>() { 19, 32, 16, 3 }); // construct a random weighted neural network
    }

    // ===== METHOD =============================================================

    public Neural_network Train()
    {
        List<bool> output;
        int count;
        for (int i = 0; i < data.Count; i++)
        {
            count = 0;
            do
            {
                output = network.fire(data[i].dataInput);
                network.gradientRetropropagation(data[i].dataOutput);
                count++;
            } while (output.SequenceEqual(data[i].dataOutput));
            Debug.Log(i * 100 / data.Count + "% retroPropagation finished in "+ count+ " iteration");
        }
        return network;
    }
}
