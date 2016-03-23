
using UnityEngine;
using System.Collections;

public class SpringManager : MonoBehaviour {

	public Spring toControl1;
	public Spring toControl2;
	public Spring toControl3;

	public float intensity; // le nombre divisé par 100 | advise = 0.


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKey (KeyCode.S)) {
//			Debug.Log("KeyPress :s");
//			upRaideur(toControl1);
//		}
//		if (Input.GetKey (KeyCode.D)) {
//			Debug.Log("KeyPress :d");
//			upRaideur(toControl2);
//		}
//		if (Input.GetKey (KeyCode.F)) {
//			Debug.Log("KeyPress :f");
//			upRaideur(toControl3);
//		}
		if(Input.GetButton("Fire1")) {
			Debug.Log("Fire1");
			upRaideur(toControl1);
		}
		if(Input.GetButton("Fire2")) {
			Debug.Log("Fire2");
			upRaideur(toControl2);
		}
		if(Input.GetButton("Fire3")) {
			Debug.Log("Fire3");
			upRaideur(toControl3);
		}

	}

	public void upRaideur(Spring toControl){


		if (toControl.GetComponent<Spring>().raideur < 2000) {
			toControl.GetComponent<Spring>().raideur += toControl.GetComponent<Spring>().raideur * intensity;
		}
	}
}
