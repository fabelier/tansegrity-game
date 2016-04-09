using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using geneticAlgo;

// Récupére les stat

public class LinkTansegrity_IA : MonoBehaviour {
    public indiv neuroNet;
    public GameObject tansegrity;

    public bool isFinished;
    public double dist_arrival; // distance in between the tansegrity and the arrival
    public int max_increment = 5000;

    //// Spring
    private SpringManager springManager;

    // Sticks
    private Stick[] sticks;
    private List<Vector3> posStick;
    

    // for the IA
    private List<double> nn_input;
    private List<bool> nn_output;
    private List<double> toFire;
    private List<bool> output;

    //get the limits of the terrain
    // usefull to normalize the pos values
    private Vector3 max;

    // code realated
    private bool isInit;
    private Vector3 arrival;
    private Vector3 posTans;
    private int increment;

    // memory 
    private List<bool> memory = new List<bool>();
    private int sinceWhen= 0;



    // Use this for a play with a trained Neural network
    void Start () {
        Vector3 tmp1,tmp2,tmp3,tmp4;

        isFinished = false;
        isInit = true;
        Object loadedObject = Resources.Load("Tansegrity");
        //Debug.LogWarning("loaded object: " + loadedObject);
        tansegrity = Instantiate(loadedObject) as GameObject;
        springManager = tansegrity.GetComponent<SpringManager>() as SpringManager;

        // get the limits of the terrain

        tmp1 = GameObject.Find("wall 1").transform.position;
        tmp2 = GameObject.Find("wall 2").transform.position;
        tmp3 = GameObject.Find("wall 3").transform.position;
        tmp4 = GameObject.Find("wall 4").transform.position;

        max = new Vector3( Mathf.Max(Mathf.Abs(tmp1.x),  Mathf.Abs(tmp2.x),  Mathf.Abs(tmp3.x),  Mathf.Abs(tmp4.x)),
                           10,
                          Mathf.Max(Mathf.Abs(tmp1.z),  Mathf.Abs(tmp2.z),  Mathf.Abs(tmp3.z), Mathf.Abs(tmp4.z)));
       


        //get the goal position
        arrival = GameObject.Find("Arrival").transform.position;
        arrival = dividVect(arrival, max);
      
        increment = 0;

        // Gestion des stick
        sticks = tansegrity.GetComponentsInChildren<Stick>();
       
    }

   
    public void Init(indiv saved_neuroNet)
    {
        neuroNet = saved_neuroNet;

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
        double eval;
        if (isInit)
        {
            //Debug.Log("increment : " + increment);
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
                slave = dividVect(slave, max);                 //  Normalise slave
                posStick.Add(slave);
                posTans += slave;                             //at the end compute the barycenter of the tansegrity
                toFire.Add((double)slave.x);
                toFire.Add((double)slave.y);
                toFire.Add((double)slave.z);

                slave = sticks[i].ObjectB.transform.position; //  stick second end
                slave = dividVect(slave, max);                 //  Normalise slave
                posStick.Add(slave);
                posTans += slave;                             //at the end compute the barycenter of the tansegrity
                toFire.Add((double)slave.x);
                toFire.Add((double)slave.y);
                toFire.Add((double)slave.z);
            }


            // Get distances of the tansegrity from the finish
            posTans = posTans / (sticks.Length * 2);
            slave = posTans - arrival;
            dist_arrival = slave.magnitude;
            toFire.Add(dist_arrival);



            // =========  fire  =====================================================
            //Debug.Log("toFire : "+toFire.ToString());
            output = neuroNet.getNn().fire(toFire);

            // === Check if the simulation move ====================================
            if(increment >1)
                isTansMoving(output, memory);


            // ==== APPLY output in the simulation ==================================
            if (output[0] == true)
            {
                springManager.setToControl(0);
            }
            if (output[1] == true)
            {
                springManager.setToControl(1);
            }
            if (output[2] == true)
            {
                springManager.setToControl(2);
            }


            //Debug.Log("Increment : "+ increment+"DistArrival : " + dist_arrival);
            // ====  STOP ==========================================================
            if (increment >= max_increment || dist_arrival <0.01 )
            {
                eval = calcEval(dist_arrival, increment);
                //Debug.Log("endEval : "+eval);
                neuroNet.setEvalValue(eval);
                Destroy(tansegrity);
                Destroy(this);
            }
        }
    }

    private double calcEval(double dist_arrival, double nb_increment)
    {
        double val;

        if(increment == max_increment)   // si le tansegrity n'est pas arrivé au pt d'arrivé, il ne peu pas obtenir plus de 0.5/1
        {
            // it is already normalized
            // val [0,1] -> [0,0.5]  
            //val = 1 / (2 + 20 * System.Math.Exp(100 *  dist_arrival));
            val = (1 - dist_arrival) / 2;
        }
        else
        {
            // normalisation
            val = increment / max_increment;
            // sigmoide val [0,1] -> [0.5, 1]
            val = 0.5 + 0.5 / (1 + 0.01 * System.Math.Exp(10 * val));
        }


        return val;
    }

    private Vector3 dividVect(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    // check if the 
    private void isTansMoving(List<bool> output, List<bool> memory, int identicalParam = 50)
    {

        if (output.SequenceEqual(memory))
            sinceWhen += 1;
        else sinceWhen = 0;

        if (sinceWhen == identicalParam)
        {
            increment = max_increment;
            Debug.Log(" JE NE BOUGE PAS JE ME FAIT SUPPRIMER !!!!");
        }
    }
}
