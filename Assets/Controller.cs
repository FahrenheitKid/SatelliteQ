using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour {
    int hp = 3;
    bool dead = false;
    public Vector3 LastGlobalPlayerPos;
    public Vector3 ResetPosition;
    // Use this for initialization

    AudioSource music;
    AudioClip music_clip;
	void Start () 
    {
        ResetPosition.Set(1000, 1000, 1000); //Posição contada como "neutra"
        LastGlobalPlayerPos = ResetPosition;

        music_clip = Resources.Load<AudioClip>("Sounds/soundtrack1");
        float vol = 0.45f;
        music = AddAudio(music_clip, true, true, vol);
        music.Play();

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


    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {

        AudioSource newAudio = gameObject.AddComponent<AudioSource>();

        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

        return newAudio;

    }
}
