using UnityEngine;
using System.Collections;

public class SatelliteMovement : MonoBehaviour
{
	public GameObject player;
    public Camera camera;
    private Vector3 pos = Vector3.zero;
	private Vector3 startpos = Vector3.zero;
	private float speed = 2.0f;
	private float zoomSpeed = 2.0f;
    //
    float maximumZoom = 2.0f;
   public  float currentFOV;// = camera.fieldOfView;
 

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
    float cameraDistance = 1.0f;
	// Use this for initialization
	void Start ()
	{
        player = GameObject.FindGameObjectWithTag("Player");
		pos.x = player.transform.localPosition.x;
		pos.y = player.transform.localPosition.y + 50;
		pos.z = player.transform.localPosition.z - 30;
        currentFOV = camera.fieldOfView;
		transform.localPosition = pos;
		startpos = pos;
      //  camera = GameObject.Find("TopViewCamera").camera;
	}



	//
	// UPDATE
	//

	void Update () 
	{
        
		float scroll = Input.GetAxis("Mouse ScrollWheel");
	//	transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

        if (Input.GetMouseButton(0))
        {
            if (camera.fieldOfView > 20f)
            {
                camera.fieldOfView -= 10 * Time.deltaTime;//Mathf.Lerp(currentFOV, maximumZoom, Time.deltaTime * zoomSpeed);

            }
            
        }

        if (camera.fieldOfView <= currentFOV && !Input.GetMouseButton(0))
        {
            camera.fieldOfView += 30 * Time.deltaTime;
        }
        float h = (horizontalSpeed * Input.GetAxis("Mouse X")) *speedratio;
        float v = (verticalSpeed * Input.GetAxis("Mouse Y")) *speedratio;
			transform.Translate(h,0,v, Space.World);
		Vector3 temp = transform.localPosition;

		float distance = Vector3.Distance(transform.position, pos);
        //Debug.Log(distance);
       // if (distance != 0.0f) 
		speedratio = 0.5f; // (distance * 0.2f);
		
	//	speedratio = speedratio * 5.0f;

        //temp = transform.position;
        //temp.x = Mathf.Clamp(transform.position.x, pos.x - 50, pos.x + 50);
        //temp.y = Mathf.Clamp(transform.position.y, pos.y - 50, pos.y + 50);
        //temp.z = Mathf.Clamp(transform.position.z, pos.z - 50, pos.z + 50);

        //transform.localPosition = temp;



	}
   public void PositionUpdate(Vector3 newPos)
    {
        Debug.Log("Updated");
        newPos.y = newPos.y + 50;
       // newPos.z = newPos.z + 50;
        transform.position = newPos;
        pos = newPos;
        speedratio = 0.5f;
       
    }

}
