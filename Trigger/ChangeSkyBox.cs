using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyBox : MonoBehaviour {
    [SerializeField] private Material SkyBox;
	// Use this for initialization
	void Start () {
        RenderSettings.skybox = SkyBox;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
