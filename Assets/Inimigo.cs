using UnityEngine;
using System.Collections;

public class Inimigo : MonoBehaviour
{
    public Animator anim;
    public float patrolSpeed = 10f;
    public float chaseSpeed = 30f;
    public Transform[] waypoints;
    int waypointIndex;
    enemyLOS LOS;
    NavMeshAgent nav;
    Weapon gun;
    Controller gameControl;

    // Use this for initialization
    void Start()
    {
        gun = GetComponent<Weapon>();
        LOS = GetComponent<enemyLOS>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        waypointIndex = 0;
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (LOS.playerSighted == true)
        {
            Shoot();
        }
        if(LOS.lastSight != gameControl.ResetPosition && LOS.playerSighted == false)
        {
            Chase(LOS.lastSight);
        }
        else if (LOS.playerSighted == false)
        {
            Patrol();
        }
    }
    void Shoot()
    {
        anim.SetBool("isShooting", true);
        nav.Stop();
        gun.Shoot();
    }
    void Chase(Vector3 Destination)
    {
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
        nav.Resume();
        nav.speed = patrolSpeed;
        anim.SetBool("isShooting", false);
        anim.SetBool("isRunning", false);
        if (nav.remainingDistance < nav.stoppingDistance)
        {
            waypointIndex++;
        }
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
      //  Debug.Log(waypointIndex);
        nav.destination = waypoints[waypointIndex].position;
        
    }
    
}


