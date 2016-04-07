using UnityEngine;
using System.Collections;

public class Inimigo : MonoBehaviour {
    NavMeshAgent navigator;
    GameObject player;
	// Use this for initialization
    void Start()
    {
        navigator = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () 
    {
        navigator.SetDestination(player.transform.position);
	}
}
