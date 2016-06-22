using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ShaderControlDLL;

public class ShaderController : MonoBehaviour
{
    [System.Serializable]
    public enum ControlMode
    {
        ByTag,
        MovingObject
    }

    [System.Serializable]
    public class ControlQ
    {
        public ControlMode controlMode;

        public string tag;
        public Shader ActivationShader;
        public Shader NormalShader;
        
        public class Parameters
        {
            public string param;
            public bool isColor;
            public bool isFloat;
            public Color color;
            public float value;
        }

        public List<Parameters> parameters;
    }

    public KeyCode ActivationKey;
    public KeyCode CancelKey;
    public List<ControlQ> tagList;
    ShaderControlQ controller = new ShaderControlQ();

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(ActivationKey) && !Input.GetKey(CancelKey))
        {
            for (int i = 0; i < tagList.Count; i++)
            {
                if (tagList[i].controlMode == ControlMode.ByTag)
                {
                    controller.SwitchShaderByTag(tagList[i].tag, tagList[i].ActivationShader);
                    //controller.SwitchShaderByTagCustomParam(tagList[i].tag, tagList[i].ActivationShader, tagList[i].param, tagList[i].color);
                }
            }
        }

        if (Input.GetKeyUp(ActivationKey) || Input.GetKey(CancelKey))
        {
            for (int i = 0; i < tagList.Count; i++)
            {
                if (tagList[i].controlMode == ControlMode.ByTag)
                {
                    controller.SwitchShaderByTag(tagList[i].tag, tagList[i].NormalShader);
                }
            }
        }
    }
}
