using UnityEngine;
using System.Collections;

public class smoothFollow : MonoBehaviour {


	public Transform target;
	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;


	// Use this for initialization
	void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
		// Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
		// transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

		if (target)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);
		}
	}
}
