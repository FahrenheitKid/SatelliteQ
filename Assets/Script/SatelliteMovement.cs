using UnityEngine;
using System.Collections;

public class SatelliteMovement : MonoBehaviour
{
    public GameObject player;
 
    private Vector3 pos = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        pos.x = player.transform.localPosition.x;
        pos.y = player.transform.localPosition.y + 50;
        pos.z = player.transform.localPosition.z - 30;

        transform.localPosition = pos;
	}
}
