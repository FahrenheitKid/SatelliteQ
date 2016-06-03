using UnityEngine;
using System.Collections;

public class laserScript : MonoBehaviour
{
	private Vector3 end_position;
	LineRenderer laserLine;

    public float laser_time;
    private float current_time;
    private float end_time;

    public GameObject burn;
    private bool burnCreated = false;

    // Use this for initialization
    void Start ()
    {
        current_time = 0;
        end_time = laser_time;
		laserLine = GetComponent<LineRenderer>();
		laserLine.SetWidth (1.8f, 1.8f);

        end_position.x = transform.position.x;
        end_position.y = transform.position.y;
        end_position.z = transform.position.z;
    }
	
	// Update is called once per frame
	void Update ()
    {
        current_time += Time.deltaTime;
        Debug.Log(current_time);
        //UpdateLine();
        if(current_time >= 0.7f && !burnCreated)
        {
            Instantiate(burn, new Vector3(transform.position.x, 0.05f, transform.position.z), Quaternion.identity);
            burnCreated = true;
        }
        if(current_time >= end_time)
        {
            Destroy(gameObject);
        }
	}

    void UpdateLine()
    {
        laserLine.SetPosition(0, transform.position);
        laserLine.SetPosition(1, end_position);
        end_position.y -= 1.3f;
    }
}
