using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShaderControlDLL;

public class Alarm : MonoBehaviour
{
    public float fadeSpeed = 2.0f;
    public float highIntensity = 2.0f;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;
    public bool alarmOn;
    private float targetIntensity;
    public List<Light> lights;
    AudioSource alarmSound;

    // Use this for initialization
    void Start ()
    {
        alarmSound = GetComponent<AudioSource>();
        alarmOn = false;
        targetIntensity = highIntensity;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(alarmOn)
        {
            for(int i = 0; i < lights.Count; i++)
            {
                lights[i].intensity = Mathf.Lerp(lights[i].intensity, targetIntensity, fadeSpeed * Time.deltaTime);
                CheckTargetIntensity(lights[i]);
            }
        }
        else
        {
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].intensity = Mathf.Lerp(lights[i].intensity, 0.0f, fadeSpeed * Time.deltaTime);
            }
        }
	}

    void CheckTargetIntensity(Light light)
    {
        // If the difference between the target and current intensities is less than the change margin...
        if (Mathf.Abs(targetIntensity - light.intensity) < changeMargin)
        {
            // ... if the target intensity is high...
            if (targetIntensity == highIntensity)
                // ... then set the target to low.
                targetIntensity = lowIntensity;
            else
                // Otherwise set the targer to high.
                targetIntensity = highIntensity;
        }
    }

    public void StartAlarm()
    {
        alarmOn = true;
        alarmSound.Play();
    }

    public void StopAlarm()
    {
        alarmOn = false;
        alarmSound.Stop();
    }
}
