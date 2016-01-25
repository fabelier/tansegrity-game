
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

		if (Input.GetKey (KeyCode.S)) {
			upRaideur(toControl1);
			Debug.Log("KeyPress :s");
		}
		if (Input.GetKey (KeyCode.D)) {
			upRaideur(toControl2);;
			Debug.Log("KeyPress :d");
		}
		if (Input.GetKey (KeyCode.F)) {
			upRaideur(toControl3);;
			Debug.Log("KeyPress :f");
		}

	}

	public void upRaideur(Spring toControl){
		float tmp;

		if (toControl.GetComponent<Spring> ().raideur < 2000) { 
			toControl.GetComponent<Spring> ().raideur += toControl1.GetComponent<Spring> ().raideur * intensity;
		} 
	}
}
