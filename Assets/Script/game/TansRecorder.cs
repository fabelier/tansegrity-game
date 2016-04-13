using UnityEngine;
using System.Collections.Generic;

public class TansRecorder : MonoBehaviour {

  
    public GameObject myself; 
    public Stick stick0;
    public Stick stick1;
    public Stick stick2;
    public bool finished;
    private List<DataInputOutput> data;
    private DataTrain parent;

    private int increment;
    private Vector3 max;
    private Vector3 arrival;
    private Vector3 posTans;

    // Use this for initialization
    void Start () {


        data = new List<DataInputOutput>();

        setMax();

        //get the goal position
        arrival = GameObject.Find("Arrival").transform.position;
        arrival = dividVect(arrival, max);
        finished = false;
        increment = 0;
    }

    public void Init(DataTrain parent)
    {
        this.parent = parent;
    }


	// Update is called once per frame
	void Update () {
        DataInputOutput tmp;
        List<double> input = new List<double>();
        List<bool> output= new List<bool>() { false, false, false };

        increment += 1;
        // get Output
        if (Input.GetButton("Fire1"))
        {
            output[0] = true;
        }
        if (Input.GetButton("Fire2"))
        {
            output[1] = true;
        }
        if (Input.GetButton("Fire3"))
        {
            output[2] = true;
        }

        // Get Inputs
        input.Add(stick0.ObjectA.transform.position.x / max.x); input.Add(stick1.ObjectA.transform.position.x / max.x); input.Add(stick2.ObjectA.transform.position.x / max.x);
        input.Add(stick0.ObjectA.transform.position.y / max.y); input.Add(stick1.ObjectA.transform.position.y / max.y); input.Add(stick2.ObjectA.transform.position.y / max.y);
        input.Add(stick0.ObjectA.transform.position.z / max.z); input.Add(stick1.ObjectA.transform.position.z / max.z); input.Add(stick2.ObjectA.transform.position.z / max.z);
        input.Add(stick0.ObjectB.transform.position.x / max.x); input.Add(stick1.ObjectB.transform.position.x / max.x); input.Add(stick2.ObjectB.transform.position.x / max.x);
        input.Add(stick0.ObjectB.transform.position.y / max.y); input.Add(stick1.ObjectB.transform.position.y / max.y); input.Add(stick2.ObjectB.transform.position.y / max.y);
        input.Add(stick0.ObjectB.transform.position.z / max.z); input.Add(stick1.ObjectB.transform.position.z / max.z); input.Add(stick2.ObjectB.transform.position.z / max.z);
        input.Add(meanList(input));

      

        tmp.dataInput = input;
        tmp.dataOutput = output;
        data.Add(tmp);

        // ====== STOP  ==========
        if(increment == 1000)
        {
            finished = true;
            parent.data = data;
            Destroy(myself);
        }
    }

    private void setMax()
    {
        Vector3 tmp1, tmp2, tmp3, tmp4;

        tmp1 = GameObject.Find("wall 1").transform.position;
        tmp2 = GameObject.Find("wall 2").transform.position;
        tmp3 = GameObject.Find("wall 3").transform.position;
        tmp4 = GameObject.Find("wall 4").transform.position;

        max = new Vector3(Mathf.Max(Mathf.Abs(tmp1.x), Mathf.Abs(tmp2.x), Mathf.Abs(tmp3.x), Mathf.Abs(tmp4.x)),
                           10,
                          Mathf.Max(Mathf.Abs(tmp1.z), Mathf.Abs(tmp2.z), Mathf.Abs(tmp3.z), Mathf.Abs(tmp4.z)));
    }

    private Vector3 dividVect(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    private double meanList(List<double> list)
    {
        double val =0;
        for (int i = 0; i < list.Count; i++)
        {
            val += list[i];
        }
        return val / list.Count;
    }
}

public struct DataInputOutput
{
    public List<double> dataInput;
    public List<bool> dataOutput;
}

