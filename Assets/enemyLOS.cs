using UnityEngine;
using System.Collections;

public class enemyLOS : MonoBehaviour {
    //Angulo de campo de visão
    public float fovAngle = 110.0f;
    public bool playerSighted;
     //Ultima posição que o jogador foi visto
    public Vector3 lastSight;

    
    //NavMeshAgent nav;
    Controller gameControl;
    SphereCollider col;
    GameObject player;
    AudioSource alerta;
	// Use this for initialization
	void Start () {
        lastSight.Set(1000, 1000, 1000);
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        alerta = GetComponent<AudioSource>();
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {

        if (playerSighted == true)
        {
            if (!alerta.isPlaying)
            {
                alerta.Play();
            }
            
        }
        else
        {
            lastSight = gameControl.LastGlobalPlayerPos;
        }
	
	}
    
    void OnTriggerStay(Collider other)
    {
        //Se objeto que entrou no campo de atuação do inimigo for o Player
        if (other.gameObject == player)
        {
            playerSighted = false;
            //Direção do inimigo para jogador
            Vector3 direction = other.transform.position - transform.position;
            //Angulo entre jogador e vetor para "frente" do inimigo
            float angle = Vector3.Angle(direction, transform.forward);
            //Se o jogador estiver dentro do campo de visão do inimigo
            if (angle < fovAngle * 0.5f)
            {
                RaycastHit hit;
                Ray ray = new Ray();
                ray.origin = transform.position + transform.up;
                ray.direction = direction.normalized;
                Debug.DrawRay(transform.position + transform.up * 2, direction.normalized*100, Color.green);
                //Se o raycast acertar algo
                if (Physics.Raycast(ray, out hit)) 
                {
                    //E se esse algo for o jogador
                    if (hit.collider.gameObject == player)
                    {
                        playerSighted = true;
                        //Atualiza a ultima posição que o jogador foi visto
                        lastSight = player.transform.position;
                        gameControl.LastGlobalPlayerPos = player.transform.position;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerSighted = false;
        }
    }
}
