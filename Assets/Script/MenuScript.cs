using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    opening op;
    public GUISkin skin;
    public GUITexture logo;
	// Use this for initialization
	void Start () {
        op = GameObject.Find("op").GetComponent<opening>();
    }

    void OnGUI()
    {
        GUI.skin = skin;

        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 30), "Start Game"))
        {
           // SceneManager.LoadScene(1);
            op.start = true;
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 30, 200, 30), "Options"))
            Debug.Log("Options");
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 75, 200, 30), "Quit"))
        {
            Application.Quit();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
        if (op.done)
        {
            SceneManager.LoadScene(1);
        }
	}
}
