using UnityEngine;
using System.Collections;

public class SatelliteMovement : MonoBehaviour
{
    //Glitch Control
    Kino.AnalogGlitch analogGlitch;
    Kino.DigitalGlitch digitalGlitch;
    float timer;
    float effectsValue;
    //
    public GameObject player;
    private Camera SatelliteCamera;
    private Vector3 pos = Vector3.zero;
    private Vector3 startpos = Vector3.zero;
    public GameObject laserPrefab;
    public int laserCharges = 3;
    public float laserChargesCooldown = 10;
    private float laserChargesTimer = 0;
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
    public Texture SatelliteLightningBolt;
    public Texture SatelliteLaserChargesIcon;
    public Texture SatelliteEnergyBar;
    private float SatelliteEnergyBarProgress = 0;
    private float SatelliteCooldownProgress = 0;
    public Texture SatelliteAim;

    public AudioSource source;
    public bool justPressed = true;
    bool gotTempPos;
    Vector3 tempStarterPos;
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

        analogGlitch = GetComponent<Kino.AnalogGlitch>();
        digitalGlitch = GetComponent<Kino.DigitalGlitch>();
        timer = 0;
        effectsValue = 0;

    }

    void Update()
    {
        analogGlitch.scanLineJitter = effectsValue;
        digitalGlitch.intensity = effectsValue;
        if (isOn)
        {

            if (Input.GetMouseButton(0) && laserCharges > 0 && laserChargesTimer >= laserChargesCooldown)
            {
                if (justPressed)
                {
                    justPressed = false;
                    source.Play();
                }
                if (SatelliteCamera.fieldOfView > 20f)
                {
                    SatelliteCamera.fieldOfView -= 20 * Time.deltaTime;
                    //increase size of bar when charging
                    if (SatelliteEnergyBarProgress <= SatelliteEnergyBar.width)
                    {
                        SatelliteEnergyBarProgress += (SatelliteEnergyBar.width / 2) * Time.deltaTime;
                    }
                    if (SatelliteEnergyBarProgress > SatelliteEnergyBar.width)
                    {
                        SatelliteEnergyBarProgress = SatelliteEnergyBar.width;
                    }
                }
                Debug.Log(SatelliteEnergyBarProgress);
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
                            laserChargesTimer = 0;
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
                SatelliteEnergyBarProgress -= SatelliteEnergyBar.width * Time.deltaTime;
                if (SatelliteEnergyBarProgress < 0)
                {
                    SatelliteEnergyBarProgress = 0;
                }
            }
            //decrease size of bar when not charging
            moveSatellite();

        }
        //Efeito da camera quando liga o satélite
        timer += Time.deltaTime;
        if (timer < 0.2f)
        {
            effectsValue = 1.0f;

        }
        if (timer > 0.2f & effectsValue > 0.2f)
        {
            effectsValue -= 0.2f;

        }
        //Fim do efeito da camera
        if (isOn == false)
        {
            justPressed = true;
            source.Stop();
            effectsValue = 0;
            timer = 0;
            gotTempPos = false;
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
        //  if (speedratio > 0.07f)

        if (gotTempPos == false)
        {
            tempStarterPos = transform.position;
            gotTempPos = true;
        }
        float h = (horizontalSpeed * Input.GetAxis("Mouse X")) * speedratio;
        float v = (verticalSpeed * Input.GetAxis("Mouse Y")) * speedratio;
        transform.Translate(h, 0, v, Space.World);
        Vector3 temp = transform.localPosition;
        float actualDistance = Vector3.Distance(transform.position, tempStarterPos);
        print(actualDistance);
        speedratio = 0.5f;// *((initialDistanceFromPlayer / actualDistance) / 5);
        if (actualDistance > 50.0f)
        {
            effectsValue = actualDistance / 150.0f;
        }
        else
        {
            effectsValue = 0;
        }


    }

    void OnGUI()
    {
        if (laserChargesTimer < laserChargesCooldown)
        {
            laserChargesTimer += Time.deltaTime * 0.5f;
            SatelliteCooldownProgress = (230 / laserChargesCooldown) * laserChargesTimer;
        }



        if (isOn)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - SatelliteUI.width / 2, Screen.height - 50, SatelliteUI.width, SatelliteUI.height), SatelliteUI);
            //barra de cooldown para próximo tiro
            GUI.DrawTexture(new Rect(Screen.width / 2 - SatelliteUI.width / 2 + 10, Screen.height - 38, SatelliteCooldownProgress, SatelliteEnergyBar.height), SatelliteEnergyBar);
            GUI.DrawTexture(new Rect(Screen.width / 2 - SatelliteUI.width / 2 + 117.725f, Screen.height - 33, SatelliteLightningBolt.width, SatelliteLightningBolt.height), SatelliteLightningBolt);
            //barra carregando tiro
            GUI.DrawTexture(new Rect(Screen.width / 2 - SatelliteUI.width / 2 + 407.6f, Screen.height - 38, SatelliteEnergyBarProgress, SatelliteEnergyBar.height), SatelliteEnergyBar);
            //laser charges
            for (int i = 0; i < laserCharges; i++)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - SatelliteUI.width / 2 + 890 + (i * SatelliteLaserChargesIcon.width), Screen.height - 40, SatelliteLaserChargesIcon.width, SatelliteLaserChargesIcon.height), SatelliteLaserChargesIcon);
            }
            GUI.DrawTexture(new Rect(Screen.width / 2 - 10, Screen.height / 2 - 10, 20, 20), SatelliteAim);
        }
    }

}
