using UnityEngine;
using System.Collections;

public class fpsMeter : MonoBehaviour {

    float deltaTime = 0.0f;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        //Debug.Log(Time.deltaTime);
        //debug.log(qualitysettings.vsynccount);
        //Time.fixedDeltaTime = deltaTime;
        //Debug.Log(0.02f / deltaTime);
        if (Input.GetKey(KeyCode.T))
        {
            QualitySettings.vSyncCount = 1;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            QualitySettings.vSyncCount = 0;
        }
        if (QualitySettings.vSyncCount == 1)
        {
            Time.timeScale = 1;
        }
        if (QualitySettings.vSyncCount == 0)
        {
            Time.timeScale = 0.017f / deltaTime;
        }
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(w*0.90f, h*0.98f, w-w*0.90f, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
