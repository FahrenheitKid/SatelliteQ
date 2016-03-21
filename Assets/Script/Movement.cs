using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Animator anim;
    CharacterController controller;
    public Camera FirstPerson;
    public Camera TopView;

    public float speedWalk = 15.0f;
    public float speedRun = 28.0f;
    public float rotationSpeed = 80.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    private float speed = 30.0f;
    private Vector3 translation = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

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
        
        rotation.y = Input.GetAxis("Mouse X") * rotationSpeed;
        rotation.x = Input.GetAxis("Mouse Y") * rotationSpeed;

        if (controller.isGrounded)
        {
            //Jump
            if (Input.GetKey(KeyCode.W) && Input.GetButton("Jump"))
            {
                anim.SetBool("isJumping", true);
                translation.y = jumpSpeed;
            }
            else if(!Input.GetKey(KeyCode.W) && Input.GetButton("Jump"))
            {
                anim.SetBool("isJumping", true);
                translation.y = jumpSpeed;
            }

            //Running Jump
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Jump"))
            {
                anim.SetBool("isRunningJump", true);
                translation.y = jumpSpeed;
            }
        }

        translation = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        translation = transform.TransformDirection(translation);
        translation *= speed;

        translation.y -= gravity * Time.deltaTime;
        controller.Move(translation * Time.deltaTime);
        controller.transform.Rotate(0, rotation.y * Time.deltaTime, 0);

        //FirstPerson.transform.Rotate(-rotation.x * Time.deltaTime, 0, 0);

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

        //Crouch
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("isCrouching", true);
            anim.SetBool("isCrouch", true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
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
        else if(Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.LeftControl))
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

        //Turn Left
        if (translation.x < 0 && translation.z == 0)
        {
            anim.SetBool("isLeftStrafe", true);
        }
        else
        {
            anim.SetBool("isLeftStrafe", false);
        }

        //Turn Right
        if (translation.x > 0 && translation.z == 0)
        {
            anim.SetBool("isRightStrafe", true);
        }
        else
        {
            anim.SetBool("isRightStrafe", false);
        }
    }
}
