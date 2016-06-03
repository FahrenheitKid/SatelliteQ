using UnityEngine;
using System.Collections;

public class BurnedTileScript : MonoBehaviour
{
    public float lifetime;

    private float current_time;

	// Use this for initialization
	void Start ()
    {
        current_time = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        current_time += Time.deltaTime;
        if(current_time >= lifetime)
        {
            Destroy(gameObject);
        }
	
	}
}
