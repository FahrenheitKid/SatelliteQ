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
    private bool justActivated;
    private float targetIntensity;
    public List<Light> lights;

    [System.Serializable]
    public class LightObject
    {
        [System.Serializable]
        public class Objs
        {
            public GameObject objTarget;
            public bool rotate_x;
            public bool rotate_y;
            public bool rotate_z;
        }
        public float rotationSpeed;
        public List<Objs> obj;
    }
    public LightObject lightObject;
    AudioSource alarmSound;

    // Use this for initialization
    void Start ()
    {
        alarmSound = GetComponent<AudioSource>();
        alarmOn = false;
        justActivated = false;
        targetIntensity = highIntensity;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(alarmOn)
        {
            for (int i = 0; i < lightObject.obj.Count; i++)
            {
                //set intensity once
                if (justActivated)
                {
                    Light[] Children = lightObject.obj[i].objTarget.GetComponentsInChildren<Light>();
                    foreach (Light lt in Children)
                    {
                        lt.intensity = 8;
                    }
                }
                //rotate
                if (lightObject.obj[i].rotate_x)
                {
                    Vector3 rot;
                    rot.x = lightObject.rotationSpeed;
                    rot.y = 0;
                    rot.z = 0;
                    lightObject.obj[i].objTarget.transform.Rotate(rot);
                }
                if (lightObject.obj[i].rotate_y)
                {
                    Vector3 rot;
                    rot.x = 0;
                    rot.y = lightObject.rotationSpeed;
                    rot.z = 0;
                    lightObject.obj[i].objTarget.transform.Rotate(rot);
                }
                if (lightObject.obj[i].rotate_z)
                {
                    Vector3 rot;
                    rot.x = 0;
                    rot.y = 0;
                    rot.z = lightObject.rotationSpeed;
                    lightObject.obj[i].objTarget.transform.Rotate(rot);
                }
            }
        }
        else
        {
            if (justActivated)
            {
                for (int i = 0; i < lightObject.obj.Count; i++)
                {
                    Light[] Children = lightObject.obj[i].objTarget.GetComponentsInChildren<Light>();
                    foreach (Light lt in Children)
                    {
                        lt.intensity = 0;
                    }
                }
            }
        }
	}

    void RotateObjectWithLight(List<Light> lt, bool isOn)
    {

    }

    void PulseLight(List<Light> lt, bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < lt.Count; i++)
            {
                lt[i].intensity = Mathf.Lerp(lt[i].intensity, targetIntensity, fadeSpeed * Time.deltaTime);
                CheckTargetIntensity(lt[i]);
            }
        }
        else
        {
            for (int i = 0; i < lt.Count; i++)
            {
                lt[i].intensity = Mathf.Lerp(lt[i].intensity, 0.0f, fadeSpeed * Time.deltaTime);
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
        justActivated = true;
        alarmSound.Play();
    }

    public void StopAlarm()
    {
        alarmOn = false;
        justActivated = true;
        alarmSound.Stop();
    }
}
