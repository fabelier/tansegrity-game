
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpringManager : MonoBehaviour {

    public Spring toControl1;
    public Spring toControl2;
    public Spring toControl3;

    public float intensity; // le nombre divisé par 100 | advise = 0.

    private List<bool> toControl = new List<bool>() {false, false, false};

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetButton("Fire1") || toControl[0] == true) {
			//Debug.Log("Fire1");
			upRaideur(toControl1);
            toControl[0] = false;
		}
		if(Input.GetButton("Fire2") || toControl[1] == true) {
			//Debug.Log("Fire2");
			upRaideur(toControl2);
            toControl[1] = false;
        }
		if(Input.GetButton("Fire3") || toControl[2] == true) {
			//Debug.Log("Fire3");
			upRaideur(toControl3);
            toControl[2] = false;
        }


	}

    public void setToControl(int i)
    {
        toControl[i] = true;
    }

	public void upRaideur(Spring toControl){


		if (toControl.GetComponent<Spring>().raideur < 1000) {  // il y avais 2000
			toControl.GetComponent<Spring>().raideur += toControl.GetComponent<Spring>().raideur * intensity;
		}
	}
}
