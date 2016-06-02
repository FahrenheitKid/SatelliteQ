using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour
{
	private Vector3 end_position;
	LineRenderer laserLine;

    public float laser_time;
    private float current_time;
    private float end_time;

    // Use this for initialization
    void Start ()
    {
        current_time = Time.time;
        end_time = current_time + laser_time;
		laserLine = GetComponent<LineRenderer>();
		laserLine.SetWidth (1.8f, 1.8f);

        end_position.x = transform.position.x;
        end_position.y = transform.position.y - 100;
        end_position.z = transform.position.z;
    }
	
	// Update is called once per frame
	void Update ()
    {
        current_time += Time.deltaTime;
		laserLine.SetPosition (0, transform.position);
		laserLine.SetPosition (1, end_position);
        if(current_time >= end_time)
        {
            Destroy(gameObject);
        }
	}
}
