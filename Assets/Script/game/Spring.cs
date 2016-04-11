using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {

	public Transform join1;
	public Transform join2;

	public float raideur;
	public float raideurVisee;

	private Rigidbody cylinder1;
	private Rigidbody cylinder2;

	 
	// Use this for initialization
	void Start () {
		cylinder1 = join1.parent.GetComponent<Rigidbody> ();
		cylinder2 = join2.parent.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 diff;
		Vector3 force;
		float len;

		diff = join1.position - join2.position;

		len = diff.magnitude;

		force = raideur * Mathf.Pow (len, 2) * diff.normalized * 0.017f;

        // ajout des forces
		cylinder2.AddForceAtPosition (force, join2.position);
		cylinder1.AddForceAtPosition (- force, join1.position);

		// affichage Join

		transform.position = (join1.position + join2.position) * 0.5f;
		transform.LookAt (join1.position);
		transform.Rotate (90.0f, 0.0f, 0.0f);

		transform.localScale = new Vector3(0.1f,len*0.5f ,0.1f);

		raideur = Mathf.Lerp (raideur, raideurVisee, 0.051f);

	}

	//public float raideur{
	//	get; set;
	//}


}
