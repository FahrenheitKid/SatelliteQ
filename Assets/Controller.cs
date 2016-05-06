using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour {
    int hp = 3;
    bool dead = false;
    public Vector3 LastGlobalPlayerPos;
    public Vector3 ResetPosition;
	// Use this for initialization
	void Start () 
    {
        ResetPosition.Set(1000, 1000, 1000); //Posição contada como "neutra"
        LastGlobalPlayerPos = ResetPosition;
	    
    }
	
	// Update is called once per frame
	void Update () {
	if (hp <= 0)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	}

    public void takeDamage(int damageTaken)
    {
        hp -= damageTaken;
    }
}
