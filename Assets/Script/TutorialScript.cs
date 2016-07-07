using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour
{
    public GUISkin skin;

    public float boxTimer = 5;
    private float boxTimerElapsed = 0;

    [TextArea(1, 10)]
    public string tutorialText;

    public bool showText = false;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Player")
        {
            Debug.Log("Tutorial Hit");
            showText = true;
        }
    }

    void OnGUI()
    {
        if (showText)
        {
            if (boxTimerElapsed <= boxTimer && !Input.GetKey(KeyCode.Escape))
            {
                boxTimerElapsed += Time.deltaTime;

                GUI.skin = skin;
                GUI.Box(new Rect(Screen.width / 2 - 500, Screen.height - 110, 1000, 100), tutorialText);
            }
            else
            {
                Debug.Log("Tutorial Object Destroyed");
                Destroy(gameObject);
            }
        }
    }
}
