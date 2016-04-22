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

    private Vector3 cameraPos = new Vector3(0, 13.0f, 2);
    private Vector3 crouchCameraPos = new Vector3(0, 7.5f, 2);
    private Vector3 crouchCapsuleCenter = new Vector3(0, 3.5f, 0);
    private Vector3 capsuleCenter = new Vector3(0, 6.25f, 0);
    private bool onDuct = false;

    public Font razerFont;
    private bool onDoor = false;
    private Collider targetDoor;

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
                controller.height = 7;
                controller.center = crouchCapsuleCenter;
                onDuct = true;
            }
            else
            {
                controller.height = 12.5f;
                controller.center = capsuleCenter;
                onDuct = false;
            }
        }

        if (obj.tag == "Door")
        {
            onDoor = true;
            targetDoor = obj;
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ladder")
        {
            onLadder = false;
            gravity = 70.0f;
        }

        if (obj.tag == "Duct")
        {
            
        }

        if (obj.tag == "Door")
        {
            onDoor = false;
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
    }

    //Set Animation Flags
    void AnimationControl()
    {
	    if(onDoor && Input.GetKeyDown(KeyCode.F))
        {
            if(targetDoor.transform.localEulerAngles.y > 0)
            {
                targetDoor.transform.Rotate(new Vector3(0, -160, 0));
            }
            else
            {
                targetDoor.transform.Rotate(new Vector3(0, 160, 0));
            }
            onDoor = false;
        }
        //Running Jump
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("isRunningJump");
                translation.y = 1000;
                Debug.Log("Jumped");
            }
        }
        else if(!controller.isGrounded)
        {
            Debug.Log("Not on Ground");
        }

        //Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            FirstPerson.transform.localPosition = crouchCameraPos;
            controller.height = 7;
            controller.center = crouchCapsuleCenter;
            anim.SetBool("isCrouching", true);
            anim.SetBool("isCrouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            FirstPerson.transform.localPosition = cameraPos;
            controller.height = 12.5f;
            controller.center = capsuleCenter;
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

    //Switch camera to Satellite
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

			FirstPerson.transform.localPosition = crouchCameraPos;
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

			FirstPerson.transform.localPosition = cameraPos;
			controller.height = 12.5f;
			controller.center = capsuleCenter;
			anim.SetBool("isCrouchWalking", false);
			anim.SetBool("isCrouching", false);
			anim.SetBool("isCrouch", false);

			satelliteMode = false;

            
        }
    }
}
