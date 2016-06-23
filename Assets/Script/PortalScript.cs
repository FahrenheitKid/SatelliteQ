using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PortalScript : MonoBehaviour
{
    public int SceneID;

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
        if (obj.tag == "Player")
        {
            SceneManager.LoadScene(SceneID);
        }
    }
}
