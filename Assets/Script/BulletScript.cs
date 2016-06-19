using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed = 50;
    public Vector3 deadDist;
	// Use this for initialization
	void Start ()
    {
        deadDist = new Vector3(350.0f, 350.0f, 350.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Translate(Vector3.forward * (Time.deltaTime * 150));
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
