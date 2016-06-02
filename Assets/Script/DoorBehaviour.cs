using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour {


    public bool isLocked = true;
    public float timeToLock = 10.0f; // tempo ate a porta fechar novamente
    public bool isButtonPressed = false;
    public bool isUnlockedForever = false; // setar como true caso queira que a porta fique aberta pra sempre dps de apertar o botao
    public bool simpleDoor = false;
    public bool timerDoor = false;
    public bool isOpen = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerDoor)
        {
            if (isUnlockedForever == false)
            {
                if (isButtonPressed == true)
                {
                    // isLocked = false;
                    timeToLock -= Time.deltaTime;
                }

                if (timeToLock <= 0)
                {
                    isLocked = true;
                    timeToLock = 30.0f;
                    isButtonPressed = false;
                    changeColor();
                }
            }
        }
	}

    public void pressButton()
    {
        isButtonPressed = true;
        isLocked = false;
        //openDoor();
        changeColor();
    }

     public void openDoor() // abre a porta
    {
        if (timerDoor)
        {
            if (isLocked == false)
            {
                if (transform.localEulerAngles.y > 0)
                {
                    transform.Rotate(new Vector3(0, -160, 0));
                }
                else
                {
                    transform.Rotate(new Vector3(0, 160, 0));
                }
            }
        }    
    }

    public void interactDoor()
    {
        if (simpleDoor)
        {
            if (isOpen)
            {
                transform.Rotate(Vector3.up * -160);
                isOpen = false;
            }
            else
            {
                transform.Rotate(Vector3.up * 160);
                isOpen = true;
            }
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }

    void changeColor()
    {

        GameObject parent;
        GameObject button;
        parent = transform.parent.gameObject;
        button = parent.transform.Find("DoorButton").gameObject;
        Renderer rend = button.GetComponent<Renderer>();
       // rend.material.shader = Shader.Find("Glass");

        if (isLocked == false)
        {
            rend.material.SetColor("_Color", Color.green);
            //rend.material.color = Color.green;
        }
        else
        {
            rend.material.SetColor("_Color", Color.red);
           // rend.material.color = Color.red;
        }

    }
}
