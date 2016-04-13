using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using geneticAlgo;

// Récupére les stat

public class LinkTansegrity_IA : MonoBehaviour {
    public indiv neuroNet;
    public GameObject mySelf;
    public GameObject tansegrity;

    public bool isFinished;
    public double dist_arrival; // distance in between the tansegrity and the arrival
    public int max_increment;

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
    private Vector3 min;
    private Vector3 relMax;
    private Vector3 relMin;

    // code realated
    private bool isInit;
    private Vector3 arrival;
    private Vector3 posTans;
    private int increment;
    private bool color = false;

    // memory 
    private List<bool> memory;
    private Vector3 posMemory;
    private Vector3 currentPos;
    private double speed;
    private int sinceWhen;



    // Use this for a play with a trained Neural network
    void Start () {

        isFinished = false;

        max_increment = 1000;
        sinceWhen = 0;
        memory = new List<bool>();
        //isInit = true;
        Object loadedObject = Resources.Load("Tansegrity");
        tansegrity = Instantiate(loadedObject) as GameObject;

        springManager = tansegrity.GetComponent<SpringManager>() as SpringManager;

        // get the limits of the terrain

        setMinMax();

        // get the max relative coordinates for the structure
        relMax = new Vector3(tansegrity.transform.localPosition.x + 2, tansegrity.transform.localPosition.z + 2, tansegrity.transform.localPosition.z + 2);
        relMin = new Vector3(tansegrity.transform.localPosition.x - 2, tansegrity.transform.localPosition.z - 2, tansegrity.transform.localPosition.z - 2);

        //get the goal position
        arrival = GameObject.Find("Arrival").transform.position;
        arrival = dividVect(arrival - min, max-min);
      
        increment = 0;

        posMemory = new Vector3(0,0,0);

        // Gestion des stick
        sticks = tansegrity.GetComponentsInChildren<Stick>();
       
    }


    public void Init(indiv saved_neuroNet, GameObject mySelf, bool color = false)
    {
        neuroNet = saved_neuroNet;
        this.mySelf = mySelf;
        this.color = color;
        //Error gestion
        if(saved_neuroNet.getNbNeuronAtLayer(0) != 18)
        {
            Debug.Log("Initialized with a Neural network with wrong first layer size : "+ saved_neuroNet.getNbNeuronAtLayer(0));
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
            if (increment == 0)
            {
                if (color) // for the last best indiv.
                {
                    for (int i = 0; i < 3; i++)
                    {
                        sticks[i].GetComponent<MeshRenderer>().material.color = Color.black;
                    }
                }
                //Debug.Log("max : "+ max+"min : "+min);
            }
            // ============== Turn Initialisation ==================================
            List<double> toFire = new List<double>();
            increment += 1; // Count the number of turn


            // Get Environment statistics 
            // each stick end coordinates

            posStick = new List<Vector3>();
            posTans = new Vector3(0, 0, 0);
            for (int i = 0; i < sticks.Length; i++) // compute all stats related to the sticks
            {
                slave = sticks[i].transform.localPosition + sticks[i].ObjectA.transform.localPosition; //  stick first end
                slave = dividVect(slave-relMin, relMax-relMin);                 //  Normalise slave
                posStick.Add(slave);
                posTans += sticks[i].transform.position;                             //at the end compute the barycenter of the tansegrity
                toFire.Add((double)slave.x);
                toFire.Add((double)slave.y);
                toFire.Add((double)slave.z);
                slave = sticks[i].transform.localPosition + sticks[i].ObjectB.transform.localPosition; //  stick second end
                slave = dividVect(slave - relMin, relMax - relMin);                 //  Normalise slave
                posStick.Add(slave);
                toFire.Add((double)slave.x);
                toFire.Add((double)slave.y);
                toFire.Add((double)slave.z);
            }
            //for(int i = 0; i < toFire.Count; i++)
            //{
            //    if (toFire[i] > 1)
            //        Debug.Log(toFire[i]);
            //}
            // Get distances of the tansegrity from the finish
            posTans = posTans / (sticks.Length);
            posTans = dividVect((posTans - min), (max - min));
            slave = posTans - arrival;
            dist_arrival = slave.magnitude;
            //toFire.Add(dist_arrival);

            // =========  fire  =====================================================
            //Debug.Log("toFire : "+toFire.ToString());
            output = neuroNet.getNn().fire(toFire);

            // === Check if the simulation move ====================================
            if (increment >1)
                isTansMoving(output);
            currentPos = posTans;
            speed += (posMemory - currentPos).magnitude;
            posMemory = currentPos;


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
                eval = calcEval(dist_arrival, increment, speed);
                //Debug.Log("endEval : "+eval);
                if( !color)
                    neuroNet.setEvalValue(eval);
                Destroy(tansegrity);
                Destroy(mySelf);
            }
        }
    }

    private double calcEval(double dist_arrival, double nb_increment, double speed)
    {
        double val;

        //if (increment == max_increment)   // si le tansegrity n'est pas arrivé au pt d'arrivé, il ne peu pas obtenir plus de 0.5/1
        //{
        //    // it is already normalized
        //    // val [0,1] -> [0,0.5]  
        //    //val = 1 / (2 + 20 * System.Math.Exp(100 *  dist_arrival));
        //    val = (1 - dist_arrival) / 2;
        //}
        //else
        //{
        //    // normalisation
        //    val = 0.5 + (1 - increment / max_increment);
        //    // sigmoide val [0,1] -> [0.5, 1]
        //    //val = 0.5 + 0.5 / (1 + 0.01 * System.Math.Exp(10 * val));
        //}
        val = (1 - dist_arrival) * (speed / increment);// speed/increment represent the mean of the speed
        //val = (speed / increment);
        return val;
    }

    private Vector3 dividVect(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }


    // check if the 
    private void isTansMoving(List<bool> output, int identicalParam = 500)

    {

        if (output.SequenceEqual(memory))
        {
            sinceWhen += 1;

        }
        else sinceWhen = 0;

        if (sinceWhen == identicalParam)
        {
            increment = max_increment;
        }
        memory = output;
    }

    private void setMinMax()
    {
        Vector3 tmp1, tmp2, tmp3, tmp4;

        tmp1 = GameObject.Find("wall 1").transform.position;
        tmp2 = GameObject.Find("wall 2").transform.position;
        tmp3 = GameObject.Find("wall 3").transform.position;
        tmp4 = GameObject.Find("wall 4").transform.position;

        max = new Vector3(Mathf.Max(tmp1.x, tmp2.x, tmp3.x, tmp4.x),
                           10,
                          Mathf.Max(tmp1.z, tmp2.z, tmp3.z, tmp4.z));
        min = new Vector3(Mathf.Min(tmp1.x, tmp2.x, tmp3.x, tmp4.x), 0, Mathf.Min(tmp1.z, tmp2.z, tmp3.z, tmp4.z));
    }
}
