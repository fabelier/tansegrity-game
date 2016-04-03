using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using geneticAlgo;

// Récupére les stat

public class LinkTansegrity_IA : MonoBehaviour {
    public indiv neuroNet;
    public GameObject tansegrity;

    public bool isFinished;
    public double dist_arrival; // distance in between the tansegrity and the arrival

    // Spring
    private Spring toControl1;
    private Spring toControl2;
    private Spring toControl3;

    // Sticks
    private Stick[] sticks;
    private List<Vector3> posStick;
    

    // for the IA
    private List<double> nn_input;
    private List<bool> nn_output;
    private List<double> toFire;
    private List<bool> output;

    // code realated
    private bool isInit;
    private Vector3 arrival;
    private Vector3 posTans;
    private int increment;
    



    // Use this for a play with a trained Neural network
    void Start () {
        isFinished = false;
        isInit = false;
        //neuroNet = new indiv(saved_neuroNet); // use a saved and train Neural network
        Object loadedObject = Resources.Load("Tansegrity");
        //Debug.LogWarning("loaded object: " + loadedObject);
        tansegrity = Instantiate(loadedObject) as GameObject;
        
        arrival = GameObject.Find("Arrival").transform.position;
        toControl1 = tansegrity.GetComponentInChildren<Spring>();
        toControl2 = tansegrity.GetComponentInChildren<Spring>();
        toControl3 = tansegrity.GetComponentInChildren<Spring>();
        increment = 0;

        // Gestion des stick
        sticks = tansegrity.GetComponentsInChildren<Stick>();
        Debug.Log(sticks.Length);
       
    }

    public void Init(indiv saved_neuroNet)
    {
        neuroNet = new indiv(saved_neuroNet);

        //Error gestion
        if(saved_neuroNet.getNbNeuronAtLayer(0) != 19)
        {
            Debug.Log("Initialized with a Neural network with wrong first layer size");
            throw new System.Exception();
        }
        isInit = true;

    }
    // FAIRE un init avec limite de temps ??
	
	// Update is called once per frame
	void Update () {
        Vector3 slave;

        if (isInit)
        {

            // ============== Turn Initialisation ==================================
            List<double> toFire = new List<double>();
            increment += 1; // Count the number of turn




            // Get Environment statistics 
            // each stick end coordinates

            posStick = new List<Vector3>();
            posTans = new Vector3(0, 0, 0);
            for (int i = 0; i < sticks.Length; i++) // compute all stats related to the sticks
            {
                slave = sticks[i].ObjectA.transform.position; //  stick first end
                posStick.Add(slave);
                posTans += slave;                             //at the end compute the barycenter of the tansegrity
                toFire.Add((double)slave.x);
                toFire.Add((double)slave.y);
                toFire.Add((double)slave.z);

                slave = sticks[i].ObjectB.transform.position; //  stick second end
                posStick.Add(slave);
                posTans += slave;                             //at the end compute the barycenter of the tansegrity
                toFire.Add((double)slave.x);
                toFire.Add((double)slave.y);
                toFire.Add((double)slave.z);
            }


            // Get distances of the tansegrity from the finish
            posTans = posTans / (sticks.Length * 2);
            slave = posTans - arrival;
            dist_arrival = (double)slave.magnitude;
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
            if (increment >= 10000)
            {
                neuroNet.setEvalValue(dist_arrival);
                Destroy(tansegrity);
                Destroy(this);
            }
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
