using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using geneticAlgo;

// Récupére les stat

public class LinkTansegrity_IA : MonoBehaviour {
    indiv neuroNet;
    GameObject tansegrity;

    public bool isFinished;
    public double dist_arrival; // distance in between the tansegrity and the arrival

    private Spring toControl1;
    private Spring toControl2;
    private Spring toControl3;

    private List<double> nn_input;
    private List<bool> nn_output;
    private Vector3 arrival;
    private Vector3 posA, posB, posC, posD, posE, posF, posTans;
    //private double xA,xB,xC,xD,xE,xF ,yA,yB,yC,yD,yE,yF ,zA,zB,zC,zD,zE,zF;
    private List<double> toFire;
    private List<bool> output;
    private int increment;
    



    // Use this for a play with a trained Neural network
    void Start () {
        isFinished = false;
        //neuroNet = new indiv(saved_neuroNet); // use a saved and train Neural network
        tansegrity = Instantiate(Resources.Load("Tansegrity")) as GameObject;
        arrival = GameObject.Find("Arrival").transform.position;
        toControl1 = tansegrity.GetComponentInChildren<Spring>();
        toControl2 = tansegrity.GetComponentInChildren<Spring>();
        toControl3 = tansegrity.GetComponentInChildren<Spring>();

    }

    void Init(indiv saved_neuroNet)
    {
        neuroNet = new indiv(saved_neuroNet);

        //Error gestion
        if(saved_neuroNet.getNbNeuronAtLayer(0) != 19)
        {
            Debug.Log("Initialized with a Neural network with wrong first layer size");
            throw new System.Exception();
        }

    }
    // FAIRE un init avec limite de temps ??
	
	// Update is called once per frame
	void Update () {
        Vector3 slave;

        // ============== Turn Initialisation ==================================
        increment += 1; // Count the number of turn
        toFire.Clear(); // empty the input list
        
            
        // Get Environment statistics 
            // each stick end coordinates
        
        posA = tansegrity.GetComponentInChildren<Transform>().Find("a").position; 
        posB = tansegrity.GetComponentInChildren<Transform>().Find("b").position;
        posC = tansegrity.GetComponentInChildren<Transform>().Find("c").position;
        posD = tansegrity.GetComponentInChildren<Transform>().Find("d").position;
        posE = tansegrity.GetComponentInChildren<Transform>().Find("e").position;
        posF = tansegrity.GetComponentInChildren<Transform>().Find("f").position;
        toFire.Add((double) posA.x) ; toFire.Add((double) posA.y) ; toFire.Add((double) posA.z) ;
        toFire.Add((double) posB.x) ; toFire.Add((double) posB.y) ; toFire.Add((double) posB.z) ;
        toFire.Add((double) posC.x) ; toFire.Add((double) posC.y) ; toFire.Add((double) posC.z) ;
        toFire.Add((double) posD.x) ; toFire.Add((double) posD.y) ; toFire.Add((double) posD.z) ;
        toFire.Add((double) posE.x) ; toFire.Add((double) posE.y) ; toFire.Add((double) posE.z) ;
        toFire.Add((double) posF.x) ; toFire.Add((double) posF.y) ; toFire.Add((double) posF.z) ;
        
        // Get distances of the tansegrity from the finish
        posTans = (posA + posB + posC + posD + posE + posF)/7;
        slave =  posTans - arrival;
        dist_arrival = (double) slave.magnitude;
        toFire.Add(dist_arrival);

        // =========  fire  =====================================================
        output = neuroNet.getNn().fire(toFire);

        // ==== APPLY output in the simulation ==================================
        if (output[0] == true)
        {
            upRaideur(toControl1);
        }
        if (output[1] == true)
        {
            upRaideur(toControl2);
        }
        if (output[2] == true)
        {
            upRaideur(toControl3);
        }

        // ====  STOP ==========================================================
        if(increment >= 10000)
        {
            neuroNet.setEvalValue(dist_arrival);
            Destroy(tansegrity);
            Destroy(this);
        }
    }

    public void upRaideur(Spring toControl)
    {
        if (toControl.GetComponent<Spring>().raideur < 2000)
        {
            toControl.GetComponent<Spring>().raideur += toControl.GetComponent<Spring>().raideur * 0.1f;
        }
    }
}
