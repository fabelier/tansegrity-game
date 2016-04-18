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
    public GUISkin menuSkin = null;

    void Update() {
        //Debug.Log(load);
    }

    void Start() {
        menuSkin = Resources.Load("menuSkin") as GUISkin;
    }

    void OnGUI() {
        if(guiIsAvailable){
            GUIStyle style = new GUIStyle();
            if(menuSkin != null){
                GUI.skin = menuSkin;
            }
            // GUI.color = Color.yellow;
            // GUI.backgroundColor = Color.black;

            int w = Screen.width, h = Screen.height;

            Vector3 toggleSize = new Vector3(150,40,0), 
                simuButtonSize = new Vector3(150,20,0), 
                playButtonSize = new Vector3(150,20,0),
             backgroundBoxSize = new Vector3(500,200,0);

            // style.fontSize = 30;

            GUI.Box(new Rect(w/2 - backgroundBoxSize.x/2, h/2 - backgroundBoxSize.y/2, backgroundBoxSize.x, backgroundBoxSize.y), "TansegrityBot");

            simu = GUI.Toggle(new Rect(w/2 - toggleSize.x/2 + 100, h/2 - toggleSize.y/2 - 20, toggleSize.x, toggleSize.y), simu, "load");
            GUI.enabled = true;
            if (GUI.Button(new Rect(w/2 - simuButtonSize.x/2 + 100, h/2 - simuButtonSize.y/2 + 30, simuButtonSize.x, simuButtonSize.y), "SIMULATION")) {
                load = simu;
                guiIsAvailable = false;
                ChangeToScene(1);
            }
            if (GUI.Button(new Rect(w/2 - playButtonSize.x/2 - 150, h/2 - playButtonSize.y/2, playButtonSize.x, playButtonSize.y), "PLAY")) {
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
