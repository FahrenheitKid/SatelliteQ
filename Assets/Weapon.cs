using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    int damage = 1;
    public AudioClip shootingSound;
    public float flash = 3f;
    public float fadeTimer = 10f;
    float timer = 0f;
    bool shooting = false;
    LineRenderer shotLine;
    Light shotLight;
    SphereCollider col;
    Transform player;
    Controller hp;
	// Use this for initialization
	void Start () 
    {
        shotLine = GetComponent<LineRenderer>();
        shotLight = GetComponentInChildren<Light>();
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hp = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();

        shotLine.enabled = false;
        shotLight.intensity = 0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        if (timer > 1.5f && shooting)
        {
            shooting = false;
            timer = 0f;
            shotLine.enabled = false;
        }
        if (timer > 1.0f && !shooting)
        {
            Shoot();

        }
        shotLight.intensity = Mathf.Lerp(shotLight.intensity, 0f, fadeTimer * Time.deltaTime);
	}
   public void Shoot()
    {
        shooting = true;
        Debug.Log("Pew!" + timer);
       SFX();
        //hp.takeDamage(damage);
    }
   void SFX()
   {

      shotLine.SetPosition(0, transform.position + Vector3.up * 5 + transform.forward);   
      shotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
      shotLine.enabled = true;
      shotLight.intensity = flash;

  
   }
}
