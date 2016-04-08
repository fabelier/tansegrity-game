using UnityEngine;
using System.Collections;

public class ignoreCollision : MonoBehaviour {
    // Create 3 GameObject who contain all the stick for a precise tag
    private GameObject[] sticks;
    private GameObject[] sticks1;
    private GameObject[] sticks2;

    // Use this for initialization
    void Start () {
        // Push all the sticks from the same tag in one GameObject
        if (sticks == null) {
            sticks = GameObject.FindGameObjectsWithTag("stick");
        }
        if (sticks1 == null)
        {
            sticks1 = GameObject.FindGameObjectsWithTag("stick1");
        }
        if (sticks2 == null)
        {
            sticks2 = GameObject.FindGameObjectsWithTag("stick2");
        }

        // Browse all the sticks from one tag and ignore the collision with then if they do not have the same parent
        foreach (GameObject stick in sticks) {
            if (stick.transform.parent != this.gameObject.transform.parent){
                Physics.IgnoreCollision(stick.transform.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
        foreach (GameObject stick1 in sticks1) {
            if (stick1.transform.parent != this.gameObject.transform.parent){
                Physics.IgnoreCollision(stick1.transform.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
        foreach (GameObject stick2 in sticks2) {
            if (stick2.transform.parent != this.gameObject.transform.parent){
                Physics.IgnoreCollision(stick2.transform.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
    }
}
