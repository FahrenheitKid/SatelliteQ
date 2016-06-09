using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyLOS : MonoBehaviour {
    //Angulo de campo de visão
    public float fovAngle = 110.0f;
    public float meshResolution;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    
    public bool playerSighted;
     //Ultima posição que o jogador foi visto
    public Vector3 lastSight;

    
    //NavMeshAgent nav;
    Controller gameControl;
    SphereCollider col;
    GameObject player;
  
	// Use this for initialization
	void Start () {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        lastSight.Set(1000, 1000, 1000);
        col = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
      
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {

        DrawFOV();
        if (playerSighted == true)
        {

            
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
                Debug.DrawRay(transform.position + transform.up * 2, direction.normalized*col.radius, Color.green);
                //Se o raycast acertar algo
                if (Physics.Raycast(ray,out hit))
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

    public Vector3 dirFromAngle(float angleDegrees)
    {
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad)); //Polar Coordinates
    }

    void DrawFOV()
    {
        List<Vector3> points = new List<Vector3>();
        int rayCount = Mathf.RoundToInt(fovAngle * meshResolution);
        float stepAngleSize = fovAngle / rayCount;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = transform.eulerAngles.y - fovAngle / 2 + stepAngleSize * i;
           // Debug.DrawLine(transform.position, transform.position+ dirFromAngle(angle) * col.radius, Color.red);
            points.Add(transform.position + dirFromAngle(angle) * col.radius);
        }
        int vertexCount = points.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangulos = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1;i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(points[i]);
            if (i < vertexCount - 2)
            {
                triangulos[i * 3] = 0;
                triangulos[i * 3 + 1] = i + 1;
                triangulos[i * 3 + 2] = i + 2;
            }
        }
            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangulos;
    }
}
