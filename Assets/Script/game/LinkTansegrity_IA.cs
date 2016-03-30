using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using geneticAlgo;

// Récupére les stat

public class LinkTansegrity_IA : MonoBehaviour {
    indiv neuroNet;
    GameObject tansegrity;

    public bool isFinished;

    private List<double> nn_input;
    private List<bool> nn_output;


    

    // Use this for a play with a trained Neural network
       void Start (indiv saved_neuroNet) {
        isFinished = false;
        neuroNet = new indiv(saved_neuroNet); // use a saved and train Neural network
        tansegrity = Instantiate(Resources.Load("Tansegrity")) as GameObject;


        //tansegrity.AddComponent<SpringManager>(); //useless
    }

    // FAIRE un start avec la limite de temps
	
	// Update is called once per frame
	void Update () {




        // Get Environment statistics 
        // Get distances of the tansegrity from the finish

        // each stick end coordinates
  
        Vector3 posA = tansegrity.GetComponentInChildren<Transform>().Find("a").position; 
        Vector3 posB = tansegrity.GetComponentInChildren<Transform>().Find("b").position;
        Vector3 posC = tansegrity.GetComponentInChildren<Transform>().Find("c").position;
        Vector3 posD = tansegrity.GetComponentInChildren<Transform>().Find("d").position;
        Vector3 posE = tansegrity.GetComponentInChildren<Transform>().Find("e").position;
        Vector3 posF = tansegrity.GetComponentInChildren<Transform>().Find("f").position;
        double xA = (double)posA.x; double yA = (double)posA.y; double zA = (double)posA.z;
        double xB = (double)posB.x; double yB = (double)posB.y; double zB = (double)posB.z;
        double xC = (double)posC.x; double yC = (double)posC.y; double zC = (double)posC.z;
        double xD = (double)posD.x; double yD = (double)posD.y; double zD = (double)posD.z;
        double xE = (double)posE.x; double yE = (double)posE.y; double zE = (double)posE.z;
        double xF = (double)posF.x; double yF = (double)posF.y; double zF = (double)posF.z;
        


        // =========  fire  ===========================================================


    }
}
