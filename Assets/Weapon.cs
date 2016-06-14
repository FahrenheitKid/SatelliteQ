using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    int damage = 1;
    public Animator anim;
    public AudioClip shootingSound;
    public float flash = 3f;
    public float fadeTimer = 10f;
    float timer = 0f;
  public  float shootingAnimationPeak;
    bool shooting = false;
    LineRenderer shotLine;
    Light shotLight;
    SphereCollider col;
    Transform player;
    Controller hp;
    AudioSource audio;
    Transform aim;
	// Use this for initialization
	void Start () 
    {
        aim = transform.Find("aim");
        shotLine = GetComponent<LineRenderer>();
        shotLight = GetComponentInChildren<Light>();
        col = transform.root.GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hp = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        anim = transform.root.GetComponent<Animator>();
        shotLine.enabled = false;
        shotLight.intensity = 0f;
        audio = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () 
    {
  
        shootingAnimationPeak = anim.GetFloat("shotCurve");
        if (shootingAnimationPeak <= 1.09f && shooting)
        {
          
            shooting = false;
            timer = 0f;
            shotLine.enabled = false;
        }
        shotLight.intensity = Mathf.Lerp(shotLight.intensity, 0f, fadeTimer * Time.deltaTime);
	}
    public void Shoot()
    {
        if (shootingAnimationPeak >= 1.09f && !shooting)
        {
            shooting = true;
            SFX();
          //  hp.takeDamage(damage); // Comentado só para testes, remover depois.
        }
    }
   void SFX()
   {
       if (!audio.isPlaying)
       {
           audio.Play();
       }
      shotLine.SetPosition(0, aim.position);   
      shotLine.SetPosition(1, player.position + Vector3.up * 10f);
      shotLine.enabled = true;
      shotLight.intensity = flash;

  
   }
}
