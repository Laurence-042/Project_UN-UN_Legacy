using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableEnemy : MonoBehaviour {
	public float HP=3;
	public bool toSlowDown;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Destroying(){
        int enemy_num = PlayerPrefs.GetInt("enemy");
        PlayerPrefs.SetInt("enemy", enemy_num + 1);
		Destroy (this.gameObject);
	}

}
