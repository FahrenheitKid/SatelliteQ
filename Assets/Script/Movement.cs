using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Animator anim;
    CharacterController controller;
    public Camera FirstPerson;
    public Camera TopView;
	CursorLockMode wantedMode; // variavel pra trancar e/ou esconder o cursor

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
    private Vector3 idleCameraPos = new Vector3(0, 10f, 4.0f);
    private Vector3 crouchCameraPos = new Vector3(0, 6, 5);
    private Vector3 walkCameraPos = new Vector3(0, 10, 3.5f);
    private Vector3 runCameraPos = new Vector3(0, 11.5f, 2.5f);

    private Vector3 crouchCapsuleCenter = new Vector3(0, 3.5f, 0);
    private Vector3 capsuleCenter = new Vector3(0, 6.25f, 0);
    private bool onDuct = false;

    public Font razerFont;
    private bool onDoor = false;
    private bool onButton = false;
    private Collider targetDoor;
    private Collider targetDoorButton;

    float cameraRotatedX = 0;

    SatelliteMovement SatMov;
    bool posUpdated = false;
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SatMov = GameObject.Find("TopViewCamera").GetComponent<SatelliteMovement>();
        FirstPerson.enabled = true;
        FirstPerson.transform.localPosition = idleCameraPos;
        TopView.enabled = false;

		// esconde e tranca inicialmente
		Cursor.visible = false;
		wantedMode = CursorLockMode.Locked;
		Cursor.lockState = wantedMode;
    }

    // Update is called once per frame
    void Update ()
    {
        rotation.y = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        rotation.x = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

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

		//quando aperta esc, mostra o cursor e destrava
		if (Input.GetKeyDown (KeyCode.Escape))
			Cursor.lockState = wantedMode = CursorLockMode.None;
				Cursor.visible = (CursorLockMode.Locked != wantedMode);
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
    }

    void OnGUI()
    {
        if (onDoor && targetDoor.transform.localEulerAngles.y > 0)
        {
            GUI.skin.font = razerFont;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F - Close Door");
        }
        else if(onDoor)
        {
            GUI.skin.font = razerFont;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F - Open Door");
        }


        if (onButton)
        {
            GUI.skin.font = razerFont;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 30), "Press F to Press Button");
        }
    }

    void cameraControl(string key)
    {
        switch (key)
        {
            case "Crouch":
                FirstPerson.transform.localPosition = crouchCameraPos;
                break;
            case "Walk":
                FirstPerson.transform.localPosition = walkCameraPos;
                break;
            case "Run":
                FirstPerson.transform.localPosition = runCameraPos;
                break;
            case "Idle":
                FirstPerson.transform.localPosition = idleCameraPos;
                break;
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
        //TESTE------------------------------------------------------------
        if (onDoor && Input.GetKeyDown(KeyCode.F))
        {
            if (targetDoor.tag == "Door")
            {
                if (targetDoor.transform.localEulerAngles.y > 0)
                {
                    targetDoor.transform.Rotate(new Vector3(0, -160, 0));
                }
                else
                {
                    targetDoor.transform.Rotate(new Vector3(0, 160, 0));
                }
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


        //Running Jump
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("isRunningJump");
                translation.y = 1000;
                //Debug.Log("Jumped");
            }
        }
        else if(!controller.isGrounded)
        {
            //Debug.Log("Not on Ground");
        }

        //Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controller.height = 7;
            controller.center = crouchCapsuleCenter;
            cameraControl("Crouch");
            anim.SetBool("isCrouching", true);
            anim.SetBool("isCrouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            controller.height = 12.5f;
            controller.center = capsuleCenter;
            cameraControl("Idle");
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouch", false);
        }

        //Crouch Walking
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouchWalking", true);
            cameraControl("Crouch");
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouch", false);
        }
        else if (Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
        {
            cameraControl("Crouch");
            anim.SetBool("isCrouchWalking", false);
            anim.SetBool("isCrouching", false);
            anim.SetBool("isCrouch", true);
        }

        //Walk
        if (translation.z != 0 && Input.GetKey(KeyCode.W))
        {
            cameraControl("Walk");
            anim.SetBool("isWalking", true);
            speed = speedWalk;
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        //Run
        if (translation.z != 0 && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            cameraControl("Run");
            anim.SetBool("isRunning", true);
            speed = speedRun;
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }

    void CharacterJump()
    {
        //translation
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
			satelliteMode = true;
            speed = 0;
            FirstPerson.enabled = false;
            TopView.enabled = true;

			controller.height = 7;
			controller.center = crouchCapsuleCenter;
			anim.SetBool("isCrouching", true);
			anim.SetBool("isCrouch", true);



        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            speed = speedWalk;
            FirstPerson.enabled = true;
            TopView.enabled = false;

			controller.height = 12.5f;
			controller.center = capsuleCenter;
			anim.SetBool("isCrouchWalking", false);
			anim.SetBool("isCrouching", false);
			anim.SetBool("isCrouch", false);

			satelliteMode = false;

            
        }
    }
}
