using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScript : MonoBehaviour {
    [SerializeField] private GameObject[] targets;
    [SerializeField] private string scriptName;
    // Use this for initialization
    void Start () {
        if (scriptName == "ObjectMove")
        {
            for(int i = 0; i < targets.Length; i++)
            {
                targets[i].AddComponent<ObjectMove>();
            }
           
        }
        else
        {
            Debug.Log("附加脚本出错,未找到名为\""+ scriptName+"\"的类");
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
