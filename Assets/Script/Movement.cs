using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Movement : MonoBehaviour
{
    public GUISkin skin;

    Animator anim;
    CharacterController controller;
    public Camera FirstPerson;
    public Camera TopView;
	CursorLockMode wantedMode; // variavel pra trancar e/ou esconder o cursor

    public bool hasSatellite;
	public bool satelliteMode = false;
    public float speedWalk = 20.0f;
    public float speedWalkBackwards = 15.0f;
    public float speedRun = 40.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 70.0f;
    private float speed = 20.0f;
    private bool onLadder = false;

    private Vector3 cameraRotation = Vector3.zero;
    public float mouseSpeed = 100.0f;

    private Vector3 translation = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    //different camera position to animations
    //private Vector3 idleCameraPos = new Vector3(0, 10f, 4.0f);
    //private Vector3 crouchCameraPos = new Vector3(0, 6, 5);
    //private Vector3 walkCameraPos = new Vector3(0, 10, 3.5f);
    //private Vector3 runCameraPos = new Vector3(0, 11.5f, 2.5f);

    //private Vector3 crouchCapsuleCenter = new Vector3(0, 3.5f, 0);
    //private Vector3 capsuleCenter = new Vector3(0, 6.25f, 0);
    private bool onDuct = false;

    private bool onDoor = false;
    private bool onButton = false;
    private Collider targetDoor;
    private Collider targetDoorButton;

    private bool onWatch = false;

    float cameraRotatedX = 0;

    SatelliteMovement SatMov;
    bool posUpdated = false;

    AudioSource footsteps1;
    AudioClip footsteps1_clip;

    AudioSource footsteps_run1;
    AudioClip footsteps_run1_clip;
    bool footsteps1_pressed = true;
    bool footsteps_run1_pressed = true;
    // Use this for initialization
    GameObject menu;
    bool isPaused = false;

    void Awake()
    {
        Cursor.visible = false;
        wantedMode = CursorLockMode.Locked;
        Cursor.lockState = wantedMode;

    }

    void Start ()
    {
         menu = GameObject.Find("Canvas");
        //menu = menu.transform.FindChild("MainMenu").gameObject;

        footsteps1_clip = Resources.Load<AudioClip>("Sounds/footsteps1");
        float vol = 0.7f;
        footsteps1 = AddAudio(footsteps1_clip, true, true, vol);

        footsteps_run1_clip = Resources.Load<AudioClip>("Sounds/footsteps_run1");
        float vol1 = 0.9f;
        footsteps_run1 = AddAudio(footsteps_run1_clip, true, true, vol1);

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SatMov = GameObject.Find("TopViewCamera").GetComponent<SatelliteMovement>();
        FirstPerson.enabled = true;
        //  FirstPerson.transform.localPosition = idleCameraPos;
        TopView.enabled = false;

		//esconde e tranca inicialmente
		Cursor.visible = false;
		wantedMode = CursorLockMode.Locked;
		Cursor.lockState = wantedMode;

        GameObject cont = GameObject.Find("GameController");
        Controller contScript = cont.GetComponent<Controller>();
        hasSatellite = contScript.satellitePicked();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!satelliteMode)
        {
            rotation.y = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            rotation.x = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        }

        translation.x = Input.GetAxis("Horizontal");
        if (onLadder == true)
        {
            translation.y = Input.GetAxis("Vertical");
        }
        else
        {
            translation.y = 0;
        }
        translation.z = Input.GetAxis("Vertical");
        translation = transform.TransformDirection(translation);
        translation *= speed;

        AnimationControl();
        if(hasSatellite)
        SwitchToSatellite();
        translation.y -= gravity;

        controller.Move(translation * Time.deltaTime);

		if(satelliteMode == false)
        controller.transform.Rotate(0, rotation.y, 0);

        cameraRotatedX -= rotation.x;
        //Max rotação da câmera
        cameraRotatedX = Mathf.Clamp(cameraRotatedX, -60, 60);
        cameraRotation.x = cameraRotatedX;
        FirstPerson.transform.localEulerAngles = cameraRotation;
        //TopView.transform.Rotate(0, rotation.y, 0, Space.World);
        //quando aperta esc, mostra o cursor e destrava
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!Cursor.visible)
            Cursor.lockState = wantedMode = CursorLockMode.None;
            else
            {
                Cursor.visible = false;
		        wantedMode = CursorLockMode.Locked;
		        Cursor.lockState = wantedMode;
            }

            if (isPaused)
                isPaused = false;
            else isPaused = true;
            
            /*
            if (menu.gameObject.activeSelf == true)
                menu.gameObject.SetActive(false);
            else
                menu.gameObject.SetActive(true);
                */

        }

		Cursor.visible = (CursorLockMode.Locked != wantedMode);

        if(isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Ladder")
        {
            onLadder = true;
            gravity = 0;
        }

        if (obj.tag == "Duct")
        {
            if(!onDuct)
            {
                onDuct = true;
            }
            else
            {
                onDuct = false;
            }
        }

        if (obj.tag == "Door" || obj.tag == "DoorTimer")
        {
            onDoor = true;
            targetDoor = obj;
        }

        if(obj.tag == "DoorButton")
        {
            onButton = true;
            targetDoorButton = obj;
        }

        if(obj.tag == "Watch")
        {
            onWatch = true;
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ladder")
        {
            onLadder = false;
            gravity = 70.0f;
        }

        if (obj.tag == "Door" || obj.tag == "DoorTimer")
        {
            onDoor = false;
        }

        if (obj.tag == "DoorButton")
        {
            onButton = false;
        }

        if (obj.tag == "Watch")
        {
            onWatch = false;
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;
        if (onDoor && targetDoor.transform.localEulerAngles.y > 0)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F - Close Door");
        }
        else if(onDoor)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F - Open Door");
        }

        if(onWatch)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F - Get Watch");
        }

        if (onButton)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F to Press Button");
        }
    }

    //Set Animation Flags
    void AnimationControl()
    {
        //TESTE------------------------------------------------------------
        if(Input.GetKeyDown(KeyCode.H))
        {
            Alarm alarm_script = GameObject.Find("Alarm").GetComponent<Alarm>();
            if(!alarm_script.alarmOn)
            {
                alarm_script.StartAlarm();
            }
            else
            {
                alarm_script.StopAlarm();
            }
        }
        //------------------------------------------------------------
        //Open Door
        if (onDoor && Input.GetKeyDown(KeyCode.F))
        {
            if (targetDoor.tag == "Door")
            {
                DoorBehaviour doorScript = targetDoor.GetComponent<DoorBehaviour>();
                doorScript.interactDoor();
                onDoor = false;
            }

            if(targetDoor.tag == "DoorTimer")
            {
                DoorBehaviour script;
                GameObject parent;
                parent = targetDoorButton.transform.parent.gameObject;
                script = parent.transform.Find("Door").GetComponent<DoorBehaviour>();
                script.openDoor();
            }

        }
        if (onButton && Input.GetKeyDown(KeyCode.F))
        {
            DoorBehaviour script;
            GameObject parent;
           // Debug.Log(targetDoorButton.name);
            parent = targetDoorButton.transform.parent.gameObject;
            Debug.Log(parent.name);

            script = parent.transform.Find("Door").GetComponent<DoorBehaviour>();

            script.pressButton();
        }
        //Get Watch
        if(onWatch && Input.GetKeyDown(KeyCode.F))
        {
            GameObject watch = GameObject.FindGameObjectWithTag("Watch");
            Destroy(watch);
            onWatch = false;
            GameObject cont = GameObject.Find("GameController");
            Controller script = cont.GetComponent<Controller>();
            script.pickSatellite();
            hasSatellite = true;
        }

        //Running Jump
        //if (controller.isGrounded)
        //{
        //    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
        //    {
        //        anim.SetTrigger("isRunningJump");
        //        translation.y = 1000;
        //        //Debug.Log("Jumped");
        //    }
        //}
        //else if (!controller.isGrounded)
        //{
        //    //Debug.Log("Not on Ground");
        //}

        //Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
           // controller.height = 7;
            //controller.center = crouchCapsuleCenter;
            anim.SetBool("isCrouching", true);
            anim.SetBool("isCrouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controller.height = 12.5f;
            //controller.center = capsuleCenter;
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouch", false);
        }

        //Crouch Walking
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouchWalking", true);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouch", false);
        }
        else if (Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouch", true);
        }

        //Walk
        if (translation.z != 0 && Input.GetKey(KeyCode.W))
        {
            if(footsteps1_pressed)
            {

                footsteps1.Play();
                footsteps1_pressed = false;

            }

            anim.SetBool("isWalking", true);
            speed = speedWalk;
        }
        else
        {
            
                footsteps1.Stop();
                footsteps1_pressed = true;

            
            anim.SetBool("isWalking", false);
        }

        //Run
        if (translation.z != 0 && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            if(footsteps_run1_pressed)
            {
                // footsteps1.clip = footsteps_run1_clip;
                footsteps1.Pause();
                footsteps_run1.Play();
                footsteps_run1_pressed = false;

             }
            anim.SetBool("isRunning", true);
            speed = speedRun;
        }
        else
        {
            footsteps_run1.Stop();
            footsteps1.UnPause();
            footsteps_run1_pressed = true;
            anim.SetBool("isRunning", false);
        }

    }

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {

        AudioSource newAudio = gameObject.AddComponent<AudioSource>();

        newAudio.spatialBlend = 1.0f;
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

        return newAudio;

    }

    //Switch satelliteCamera to Satellite
    void SwitchToSatellite()
    {
        //Satellite
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (satelliteMode == false)
            {
                SatMov.PositionUpdate(transform.position);
            }
            SatMov.isOn = true;
			satelliteMode = true;
            speed = 0;
            FirstPerson.enabled = false;
            TopView.enabled = true;

            //capsule player
		//	controller.height = 7;
            //controller.center = crouchCapsuleCenter;
			anim.SetBool("isCrouching", true);
			anim.SetBool("isCrouch", true);
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            speed = speedWalk;
            SatMov.isOn = false;
            satelliteMode = false;
            FirstPerson.enabled = true;
            TopView.enabled = false;

            //capsule player
            controller.height = 12.5f;
            //controller.center = capsuleCenter;
			anim.SetBool("isCrouchWalking", false);
			anim.SetBool("isCrouching", false);
			anim.SetBool("isCrouch", false);
        }
    }
}
