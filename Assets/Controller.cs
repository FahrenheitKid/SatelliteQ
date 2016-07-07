using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour
{
    int hp = 3;
    bool dead = false;
    public Vector3 LastGlobalPlayerPos;
    public Vector3 ResetPosition;
    // Use this for initialization
    static bool hasSatellite = false;
    public GameObject SatelliteWatch;
    AudioSource music;
    AudioClip music_clip;

    public GameObject tutorialPrefab;

	void Start () 
    {
        ResetPosition.Set(1000, 1000, 1000); //Posição contada como "neutra"
        LastGlobalPlayerPos = ResetPosition;

        music_clip = Resources.Load<AudioClip>("Sounds/soundtrack1");
        float vol = 0.45f;
        music = AddAudio(music_clip, true, true, vol);
        music.Play();
        SatelliteWatch.SetActive(hasSatellite);
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

    public void pickSatellite()
    {
        hasSatellite = true;
        SatelliteWatch.SetActive(hasSatellite);

        GameObject tutorial = (GameObject)Instantiate(tutorialPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        TutorialScript ts = tutorial.GetComponent<TutorialScript>();
        ts.tutorialText = "Nice, Now you have access to the satellite! Hold Q to activate the satellite vision. With the satellite you can see enemies through walls, their field of view and kill them. With the mouse you have control over the satellite, Left Button you can charge the laser and shoot to kill enemies (Note that there's a limit of charges you can use per level).";
        ts.boxTimer = 30;
        ts.showText = true;
    }

    public bool satellitePicked()
    {
        return hasSatellite;
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
