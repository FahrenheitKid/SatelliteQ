using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Animator anim;
    CharacterController controller;
    public Camera FirstPerson;
    public Camera TopView;

    public float speedWalk = 15.0f;
    public float speedWalkBackwards = 5.0f;
    public float speedRun = 28.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 9.8F;
    private float speed = 30.0f;
    private bool jumping = false;

    private Vector3 cameraRotation = Vector3.zero;
    public float mouseSpeed = 100.0f;

    private Vector3 translation = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

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
        translation.y = 0;
        translation.z = Input.GetAxis("Vertical");
        translation = transform.TransformDirection(translation);
        translation *= speed;

        AnimationControl();
        translation.y -= gravity;

        controller.Move(translation * Time.deltaTime);
        controller.transform.Rotate(0, rotation.y, 0);

        cameraRotatedX -= rotation.x;
        cameraRotatedX = Mathf.Clamp(cameraRotatedX, -60, 25);
        cameraRotation.x = cameraRotatedX;
        FirstPerson.transform.localEulerAngles = cameraRotation;
    }

    //Set Animation Flags
    void AnimationControl()
    {
        //Running Jump
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("isRunningJump");
                translation.y = 500;
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
            anim.SetBool("isCrouching", true);
            anim.SetBool("isCrouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
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

        //Walk Backwards
        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isWalkingBackwards", true);
            //speed = speedWalkBackwards;
        }
        else
        {
            anim.SetBool("isWalkingBackwards", false);
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

        //Run Left Strafe
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isLeftStrafe", true);
        }
        else
        {
            anim.SetBool("isLeftStrafe", false);
        }

        //Run Right Strafe
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isRightStrafe", true);
        }
        else
        {
            anim.SetBool("isRightStrafe", false);
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
