using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public Animator anim;
    public float flash = 3f;
    public float fadeTimer = 10f;
    public  float shootingAnimationPeak;
    bool shooting = false;
    Light shotLight;
    Controller hp;
    AudioSource sound;
    int damage = 1;
    public GameObject bulletPrefab;

	// Use this for initialization
	void Start () 
    {
        shotLight = GetComponentInChildren<Light>();
        hp = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        anim = transform.root.GetComponent<Animator>();
        shotLight.intensity = 0f;
        sound = GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update () 
    {
  
        shootingAnimationPeak = anim.GetFloat("shotCurve");
        if (shootingAnimationPeak <= 1.09f && shooting)
        {
            shooting = false;
        }
        shotLight.intensity = Mathf.Lerp(shotLight.intensity, 0f, fadeTimer * Time.deltaTime);
	}
    public void Shoot()
    {
        if (shootingAnimationPeak >= 1.09f && !shooting)
        {
            shooting = true;
            SFX();
            hp.takeDamage(damage); 
        }
    }
   void SFX()
   {
        if (!sound.isPlaying)
        {
            sound.Play();
        }

        Instantiate(bulletPrefab, shotLight.transform.position, transform.root.rotation);

        shotLight.intensity = flash;
   }
}
