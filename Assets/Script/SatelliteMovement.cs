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
    float maximumZoom = 2.0f;
    float initialDistanceFromPlayer;
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
        
	    if (Input.GetMouseButton(0))
        {
            if (camera.fieldOfView > 20f)
            {
                camera.fieldOfView -= 10 * Time.deltaTime;
            
             
                    
            }
           
            
        }
        if (camera.fieldOfView <= 20f)
        {
            Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = transform.forward * 100;
                //  Debug.DrawRay(transform.position, transform.forward, Color.red);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Hit " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.tag == "Enemy" )
                    {
                        DestroyObject(hit.collider.gameObject);
                    }
                }
            }
        }
        if (camera.fieldOfView <= currentFOV && !Input.GetMouseButton(0))
        {
            camera.fieldOfView += 30 * Time.deltaTime;
        }
        moveSatellite();  
	
	}
   public void PositionUpdate(Vector3 newPos)
    {
        Debug.Log("Updated");
        newPos.y = newPos.y+50;
        newPos.z = newPos.z - 25;
        transform.position = newPos;
        pos = newPos;
        speedratio = 0.5f;
        initialDistanceFromPlayer = Vector3.Distance(pos, player.transform.position);
    }
    void moveSatellite()
   {
       Debug.Log(speedratio);
       if (speedratio > 0.07f)
       {
           float h = (horizontalSpeed * Input.GetAxis("Mouse X")) * speedratio;
           float v = (verticalSpeed * Input.GetAxis("Mouse Y")) * speedratio;
           transform.Translate(h, 0, v, Space.World);
           Vector3 temp = transform.localPosition;
           float actualDistance = Vector3.Distance(transform.position, player.transform.position);
           speedratio = 0.5f * ((initialDistanceFromPlayer / actualDistance) / 5);
       }
   }

}
