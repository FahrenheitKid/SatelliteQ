using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    public GUISkin skin;
	// Use this for initialization
	void Start () {

    }

    void OnGUI()
    {
        GUI.skin = skin;

        if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 15, 200, 30), "Start Game"))
            Debug.Log("Start Game");
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 30), "Options"))
            Debug.Log("Options");
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 75, 200, 30), "Quit"))
            Debug.Log("Quit");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
