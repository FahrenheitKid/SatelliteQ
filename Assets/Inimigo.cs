using UnityEngine;
using System.Collections;

 
public class Inimigo : MonoBehaviour
{
    public Animator anim;
    public float patrolSpeed = 10f;
    public float chaseSpeed = 30f;
    public float animationCurve;
    public Transform[] waypoints;
    int waypointIndex;
    SphereCollider sphereCollider;
    float initialColliderRadius;
    enemyLOS LOS;
    float LOSsizeIncrement = 70.0f;
    float initialLOS;
    NavMeshAgent nav;
    Weapon gun;
    Controller gameControl;
    AudioSource footsteps;
    AudioSource footsteps_run;

    AudioClip footsteps_clip;
    AudioClip footsteps_run_clip;

    bool enteredFoot = true;
    bool enteredRun = true;
    bool isPatroling = true;
    bool isChasing = false;
    float idleTimer;

    float disintegrationAmount = 0;
    Material[] materials;

    public bool behaviourLineRoute = false;
    // Use this for initialization
    void Start()
    {
        gun = GetComponentInChildren<Weapon>();
        LOS = GetComponent<enemyLOS>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        initialColliderRadius = sphereCollider.radius;
        waypointIndex = 0;
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();

        footsteps_clip = Resources.Load<AudioClip>("Sounds/footstepsSentinel");
        float vol = 1.0f;
        footsteps = AddAudio(footsteps_clip, true, false, vol);

        footsteps_run_clip = Resources.Load<AudioClip>("Sounds/footstepsSentinel_run");
        float vol1 = 0.8f;
        footsteps_run = AddAudio(footsteps_run_clip, true, false, vol1);

        footsteps.maxDistance = 150;
        footsteps_run.maxDistance = 150;

        idleTimer = 0;
        initialLOS = LOS.fovAngle;

        materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;

       
    }

    // Update is called once per frame
    void Update()
    {

        animationCurve = anim.GetFloat("deadCurve");
      

        if(Time.timeScale == 0)
        {
            footsteps.Pause();
            footsteps_run.Pause();
        }
        else
        {
            footsteps.UnPause();
            footsteps_run.UnPause();
        }
     
        if (LOS.playerSighted == true)
        {
            Shoot();

        }
        if(LOS.lastSight != gameControl.ResetPosition && LOS.playerSighted == false)
        {
            Chase(LOS.lastSight);
            idleTimer = 0;
        }
        else if (LOS.playerSighted == false)
        {
            if (idleTimer < 3.4f)
            {
                idle();
            }
            else
            {
                Patrol();
            }
           // Patrol();
        }
        //Se a animação estiver acabando
        if (animationCurve > 0.09f)
        {
            DestroyObject(this.gameObject);
        }
        //Enquanto a animação não acaba desintegra o inimigo
        if (animationCurve > 0.01f)
        {
            for (int i = 0; i < materials.Length; i++)
            {
           
                materials[i].SetFloat("_SliceAmount", animationCurve*10);
              
            }
        }


        if (isPatroling)
        {
            if (enteredFoot)
            {
                enteredFoot = false;
                
                footsteps.Play();

            }
            
        }
        else
        {
            enteredFoot = true;
            footsteps.Stop();
        }

        if (isChasing)
        {
            if (enteredRun)
            {
                enteredRun = false;
                footsteps_run.Play();

            }

        }
        else
        {
            enteredRun = true;
            footsteps_run.Stop();
        }


    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Door")
        {
            Debug.Log("Enemy Open Door");
            DoorBehaviour doorScript = col.GetComponent<DoorBehaviour>();
            doorScript.interactDoor();
        }
    }

    void Shoot()
    {
        LOS.fovAngle = initialLOS;
        sphereCollider.radius = initialColliderRadius * 2; //Aumenta o campo de visão do inimigo
        isChasing = false;
        anim.SetBool("idle", false);
        anim.SetBool("isShooting", true);
        anim.SetBool("isRunning", false);
        nav.Stop();
        transform.LookAt(LOS.lastSight);
        gun.Shoot();
    }

    void Chase(Vector3 Destination)
    {
        LOS.fovAngle = initialLOS;
        sphereCollider.radius = initialColliderRadius * 2; //Aumenta o campo de visão do inimigo
        isPatroling = false;
        isChasing = true;
        nav.Resume();
        anim.SetBool("isShooting", false);
        anim.SetBool("isRunning", true);
        nav.speed = chaseSpeed;
        nav.destination = Destination;
        if (nav.remainingDistance < nav.stoppingDistance)
        {
            LOS.lastSight = gameControl.ResetPosition;
            gameControl.LastGlobalPlayerPos = gameControl.ResetPosition;
        }
    }

    void Patrol()
    {
        LOS.fovAngle = initialLOS;
        isPatroling = true;
        isChasing = false;
        nav.Resume();
        nav.speed = patrolSpeed;
        anim.SetBool("idle", false);
        anim.SetBool("isShooting", false);
        anim.SetBool("isRunning", false);
        if (waypoints.Length > 1)
        {
            if (nav.remainingDistance < nav.stoppingDistance)
            {
                waypointIndex++;
            }
            if (waypointIndex == waypoints.Length)
            {
                waypointIndex = 0;
                if (behaviourLineRoute)
                {
                    System.Array.Reverse(waypoints);
                }

            }
            //  Debug.Log(waypointIndex);
            nav.destination = waypoints[waypointIndex].position;
        }
        if (waypoints.Length <= 1)
        {
           
            nav.destination = waypoints[waypointIndex].position;

            if (nav.remainingDistance > nav.stoppingDistance)
            {
             
                nav.destination = waypoints[waypointIndex].position;
            }
            else
            {
           
                anim.SetBool("idle", true);
                nav.Stop();
                
            }
        }
        
    }
    
    public  void Die()
    {
        
        isPatroling = false;
        isChasing = false;
        anim.SetBool("dead", true);
        nav.Stop();
       
        
        
    }

    void idle()
  {
      LOS.fovAngle = initialLOS + LOSsizeIncrement; // Aumenta o angulo de visão do inimigo (olhando para os lados)
      idleTimer += Time.deltaTime;
      anim.SetBool("isShooting", false);
      anim.SetBool("isRunning", false);
      anim.SetBool("idle", true);
      nav.Stop();
      footsteps.Stop();
         
  }

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {

        AudioSource newAudio = gameObject.AddComponent<AudioSource>();

        newAudio.pitch = 1.25f;
        newAudio.rolloffMode = AudioRolloffMode.Linear;
        newAudio.spatialBlend = 1.0f; // 3d 0.0 = 2d
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

        return newAudio;

    }

}


