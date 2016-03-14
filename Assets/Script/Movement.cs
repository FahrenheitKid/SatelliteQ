using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Animator anim;
    CharacterController controller;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public float speed = 30.0f;
    public float rotationSpeed = 80.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;

    private Vector3 translation = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    // Update is called once per frame
    void Update ()
    {
        rotation.y = Input.GetAxis("Mouse X") * rotationSpeed;

        //if (controller.isGrounded)
        //{
        //    if (Input.GetButton("Jump"))
        //    {
        //        anim.SetBool("isJumping", true);
        //        translation.y = jumpSpeed;
        //    }

        //}

        translation = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        translation = transform.TransformDirection(translation);
        translation *= speed;

        translation.y -= gravity * Time.deltaTime;
        controller.Move(translation * Time.deltaTime);
        controller.transform.Rotate(rotation * Time.deltaTime);

        //Run
        if (translation.z != 0 && Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isRunning", true);
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
