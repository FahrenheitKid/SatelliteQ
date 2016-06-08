using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MenuLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

           
        }

    }

    public void startGame()
    {

        SceneManager.LoadScene("Clone2Test");
    }

    public void quitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
