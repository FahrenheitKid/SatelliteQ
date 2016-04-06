using UnityEngine;
using System.Collections;

public class SatelliteMovement : MonoBehaviour
{
	public GameObject player;
	//public GameObject topcamera = GameObject.Find("TopViewCamera");
    private Vector3 pos = Vector3.zero;
	private Vector3 startpos = Vector3.zero;
	private float speed = 2.0f;
	private float zoomSpeed = 2.0f;

	public float minX = -360.0f;
	public float maxX = 360.0f;

	public float minY = -45.0f;
	public float maxY = 45.0f;

	public float sensX = 100.0f;
	public float sensY = 100.0f;

	float rotationY = 0.0f;
	float rotationX = 0.0f;

	float horizontalSpeed = 4.0f;
	float verticalSpeed = 4.0f;

	float speedratio = 1.0f;

	// Use this for initialization
	void Start ()
	{

		pos.x = player.transform.localPosition.x;
		pos.y = player.transform.localPosition.y + 50;
		pos.z = player.transform.localPosition.z - 30;

		transform.localPosition = pos;
		startpos = pos;
	}



	//
	// UPDATE
	//

	void Update () 
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

		// If Right Button is clicked Camera will move.


			float h = horizontalSpeed * Input.GetAxis ("Mouse X") * speedratio;
			float v = verticalSpeed * Input.GetAxis ("Mouse Y") * speedratio;
			transform.Translate(h,0,v, Space.World);
		Vector3 temp = transform.localPosition;

		float distance = Vector3.Distance(temp, startpos);

		if(distance != 0.0f)
		speedratio = 1.0f / distance;
		
		speedratio = speedratio * 5.0f;

		temp.x = Mathf.Clamp(transform.position.x, startpos.x - 50, startpos.x + 50);
		temp.y = Mathf.Clamp(transform.position.y, startpos.y - 50, startpos.y + 50);
		temp.z = Mathf.Clamp(transform.position.z, startpos.z - 50, startpos.z + 50);

		transform.localPosition = temp;



	}

}
