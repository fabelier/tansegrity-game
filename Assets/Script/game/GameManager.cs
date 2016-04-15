using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;                     //Store a reference to our BoardManager which will set up the level.
    private int level = 0;                                         //Current level number, expressed in game as "Day 1".
    private bool load;
    public bool getLoad() {
        return load;
    }
    private bool guiIsAvailable = true;
    public bool simu = true;

    void Update() {
        //Debug.Log(load);
    }

    void OnGUI() {
        if(guiIsAvailable){
            GUIStyle style = new GUIStyle();

            int w = Screen.width, h = Screen.height;

            Vector3 toggleSize = new Vector3(130,20,0), 
                simuButtonSize = new Vector3(150,20,0), 
                playButtonSize = new Vector3(150,20,0);

            style.fontSize = 30;

            simu = GUI.Toggle(new Rect(300, 250, 130, 20), simu, "load", style);
            GUI.enabled = true;
            if (GUI.Button(new Rect(300, 300, 150, 20), "SIMULATION", style)) {
                load = simu;
                guiIsAvailable = false;
                ChangeToScene(1);
            }
            if (GUI.Button(new Rect(50, 300, 150, 20), "PLAY", style)) {
                Debug.Log("menu play");
            }
        }
    }

    //Awake is always called before any Start functions
    void Awake() {

        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }


        //If instance already exists and it's not this:
        else if (instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeToScene(int sceneToChangeTo) {
        Application.LoadLevel(sceneToChangeTo);
    }
}
