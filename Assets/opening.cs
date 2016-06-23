using UnityEngine;
using System.Collections;

public class opening : MonoBehaviour {
    Material[] botMaterial1;
    Material[] botMaterial2;
    Material[] mountMaterials;
    float botSlice = 0;
    float mountSlice = 0;
   public bool start = false;
   public bool done;
	// Use this for initialization
	void Start () {
        botMaterial1 = GameObject.Find("Y_Bot/Alpha_Joints").GetComponent<SkinnedMeshRenderer>().materials;
        botMaterial2 = GameObject.Find("Y_Bot/Alpha_Surface").GetComponent<SkinnedMeshRenderer>().materials;
        mountMaterials = GameObject.Find("terrain").GetComponent<MeshRenderer>().materials;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (start == true && botSlice < 1.0f)
        {
            mountSlice += Time.deltaTime;
            for (int i = 0; i < mountMaterials.Length; i++)
            {
                mountMaterials[i].SetFloat("_SliceAmount", mountSlice);
            }
            if (mountSlice > 0.3f)
            {
                botSlice += Time.deltaTime;
                for(int i = 0; i < botMaterial1.Length; i++)
                {
                    botMaterial1[i].SetFloat("_SliceAmount", botSlice);
                    botMaterial2[i].SetFloat("_SliceAmount", botSlice);
                }
            }
            if (botSlice >= 1.0f)
            {
                done = true;
            }
        }
	
	}
}
