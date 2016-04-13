using UnityEngine;
using System.Collections.Generic;
using Nn;

public class DataTrain : MonoBehaviour{

 
    public List<DataInputOutput> data;
    private GameObject tansRec;
    private TansRecorder tansRecScipt;
    private NeuralTrain neuralTrain;
    public string state;


	// Use this for initialization
	void Start () {

        // for Get Exemple state  ===============================================
        Object loadedObject = Resources.Load("Tansegrity_recorder");
        tansRec =  Instantiate(loadedObject) as GameObject;
        tansRecScipt = tansRec.GetComponent<TansRecorder>();
        tansRecScipt.Init(this);

        // For Neuronal training state ===========================================

        state = "Get exemple";
    }
	
	// Update is called once per frame
	void Update () {
        // ======= Get EXEMPLE ======================================
        if (state == "Get exemple") // wait that TansRecoder finish his work
        {
            if (tansRecScipt.finished == true) state = "Neuronal training";
        }

        else if (state == "Neuronal training")
        {
            neuralTrain = new NeuralTrain(data);
            neuralTrain.Train();
            state = "Check the learning";
        }
        else if(state == "Check the learning")
        {
            GameObject link = Instantiate(Resources.Load("Link")) as GameObject;
            link.GetComponent<LinkTansegrity_IA>().Init(this, link);
        }


    }
}
