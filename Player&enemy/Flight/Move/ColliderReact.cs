using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderReact : MonoBehaviour {
	[SerializeField]private Transform self;
	[SerializeField]private HP selfHP;

	[SerializeField]private AudioSource ausource;
	[SerializeField]private AudioClip hit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}
	void OnCollisionEnter(Collision other){
		Debug.Log ("hit" + other.collider.name);
		ausource.PlayOneShot (hit);
        selfHP.Hit(30);
	}

}
