using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour {
	[SerializeField]private GameObject glory;
	[SerializeField]private AudioClip activeAudio;
	[SerializeField]private GameObject guideDialog;

	public bool actived=false;

	public float radius = 5.0F;  
	// Use this for initialization
	void Start () {
		guideDialog.SetActive(false);
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			actived = true;
			glory.SetActive (true);
			AudioSource.PlayClipAtPoint (activeAudio, transform.position);
			guideDialog.SetActive(true);
		} 
	}
}
