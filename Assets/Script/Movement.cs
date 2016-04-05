using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Animator anim;
    CharacterController controller;
    public Camera FirstPerson;
    public Camera TopView;

    public float speedWalk = 20.0f;
    public float speedWalkBackwards = 15.0f;
    public float speedRun = 40.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 70.0f;
    private float speed = 20.0f;
    private bool ladderTrigger = false;

    private Vector3 cameraRotation = Vector3.zero;
    public float mouseSpeed = 100.0f;

    private Vector3 translation = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    private Vector3 cameraPos = new Vector3(0, 13.0f, 2);
    private Vector3 crouchCameraPos = new Vector3(0, 7.5f, 2);
    private Vector3 crouchCapsuleCenter = new Vector3(0, 3.5f, 0);
    private Vector3 capsuleCenter = new Vector3(0, 6.25f, 0);
    private bool onDuct = false;

    float cameraRotatedX = 0;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        FirstPerson.enabled = true;
        TopView.enabled = false;
    }

    // Update is called once per frame
    void Update ()
    {
        rotation.y = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        rotation.x = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        translation.x = Input.GetAxis("Horizontal");
        if (ladderTrigger == true)
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
        controller.transform.Rotate(0, rotation.y, 0);

        cameraRotatedX -= rotation.x;
        cameraRotatedX = Mathf.Clamp(cameraRotatedX, -60, 25);
        cameraRotation.x = cameraRotatedX;
        FirstPerson.transform.localEulerAngles = cameraRotation;
    }

    void OnTriggerEnter(Collider obj)
    {
        if(obj.tag == "Ladder")
        {
            ladderTrigger = true;
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
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Ladder")
        {
            ladderTrigger = false;
            gravity = 70.0f;
        }

        if (obj.tag == "Duct")
        {
            
        }
    }

    //Set Animation Flags
    void AnimationControl()
    {
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

    //Change to Satellite
    void SwitchToSatellite()
    {
        //Satellite
        if (Input.GetKeyDown(KeyCode.Q))
        {
            speed = 0;
            FirstPerson.enabled = false;
            TopView.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            speed = speedWalk;
            FirstPerson.enabled = true;
            TopView.enabled = false;
        }
    }
}
