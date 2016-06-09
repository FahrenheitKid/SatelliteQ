using UnityEngine;
using System.Collections;

public class SatelliteMovement : MonoBehaviour
{
    int laserCharges = 3;
    public GameObject player;
    private Camera SatelliteCamera;
    private Vector3 pos = Vector3.zero;
    private Vector3 startpos = Vector3.zero;
    public GameObject laserPrefab;
    private float speed = 2.0f;
    private float zoomSpeed = 2.0f;
    float maximumZoom = 2.0f;
    float initialDistanceFromPlayer;
    public float currentFOV;// = camera.fieldOfView;

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

    public bool isOn = false;

    public Texture SatelliteUI;
    public Texture SatelliteAim;

    public AudioSource source;
    public bool justPressed = true;

    // Use this for initialization
    void Start()
    {
        SatelliteCamera = GetComponent<Camera>();
        source = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        pos.x = player.transform.localPosition.x;
        pos.y = player.transform.localPosition.y + 50;
        pos.z = player.transform.localPosition.z - 30;
        currentFOV = SatelliteCamera.fieldOfView;
        transform.localPosition = pos;
        startpos = pos;
        //  camera = GameObject.Find("TopViewCamera").camera;
    }

    void Update()
    {
        if (isOn)
        {
            if (Input.GetMouseButton(0) && laserCharges > 0)
            {
                if (justPressed)
                {
                    justPressed = false;
                    source.Play();
                }
                if (SatelliteCamera.fieldOfView > 20f)
                {
                    SatelliteCamera.fieldOfView -= 10 * Time.deltaTime;
                }
            }

            if (SatelliteCamera.fieldOfView <= 20f)
            {
                //Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
                if (Input.GetMouseButtonUp(0))
                {
                    source.Stop();
                    justPressed = true;
                    
                    RaycastHit hit;
                   // Ray ray = SatelliteCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                    Ray ray = new Ray();
                    ray.origin = transform.position;
                    ray.direction = transform.TransformDirection(Vector3.forward) * 100;
                    Vector3 laser_position = transform.position;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("Hit " + hit.collider.gameObject.name);
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                           // DestroyObject(hit.collider.gameObject);
                            hit.collider.gameObject.GetComponent<Inimigo>().Die();
                            laserCharges--; //Usa uma carga do Laser só quando mata o inimigo
                        }
                        laser_position.x = hit.point.x;
                        laser_position.y = transform.position.y;
                        laser_position.z = hit.point.z;
                    }

                    Instantiate(laserPrefab, laser_position, Quaternion.Euler(90, 0, 0));
                }
            }

            if (SatelliteCamera.fieldOfView <= currentFOV && !Input.GetMouseButton(0))
            {
                SatelliteCamera.fieldOfView += 30 * Time.deltaTime;
            }
            moveSatellite();
           
        }

        if (isOn == false)
        {
            justPressed = true;
            source.Stop();
        }
    }
    public void PositionUpdate(Vector3 newPos)
    {
        newPos.y = newPos.y + 50;
        newPos.z = newPos.z - 25;
        transform.position = newPos;
        pos = newPos;
        speedratio = 0.5f;
        initialDistanceFromPlayer = Vector3.Distance(pos, player.transform.position);
    }
    void moveSatellite()
    {
        //Debug.Log(speedratio);
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

    void OnGUI()
    {
        if (isOn)
        {
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), SatelliteUI);
            GUI.DrawTexture(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), SatelliteAim);
        }
    }

}
