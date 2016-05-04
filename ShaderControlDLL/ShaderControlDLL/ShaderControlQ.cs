using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ShaderControlDLL
{
    public class ShaderControlQ
    {
        public void SwitchShaderByTag(string tag, Shader shader)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag(tag);

            foreach(GameObject objTag in obj)
            {
                Renderer[] mat = objTag.GetComponentsInChildren<Renderer>();
                foreach(Renderer objMat in mat)
                {
                    objMat.material.shader = shader;
                }
            }
        }

        public void SwitchShaderByTagCustomParam(string tag, Shader shader, string param, Color color)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject objTag in obj)
            {
                Renderer[] mat = objTag.GetComponentsInChildren<Renderer>();
                foreach (Renderer objMat in mat)
                {
                    objMat.material.shader = shader;
                    objMat.material.SetColor(param, color);
                }
            }
        }

        public void SwitchShaderByTagCustomParam(string tag, Shader shader, string param, float value)
        {
            GameObject[] obj = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject objTag in obj)
            {
                Renderer[] mat = objTag.GetComponentsInChildren<Renderer>();
                foreach (Renderer objMat in mat)
                {
                    objMat.material.shader = shader;
                    objMat.material.SetFloat(param, value);
                }
            }
        }

        public void SwitchShaderByMovement(Shader shader)
        {

        }
    }
}
