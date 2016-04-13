using UnityEngine;
using System.Collections;

public class toogleManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnLevelWasLoaded(int level) {
        if (level == 1)
            Destroy(this.gameObject);

    }

}
