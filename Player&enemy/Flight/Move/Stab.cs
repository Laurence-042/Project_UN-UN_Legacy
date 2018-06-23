using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : MonoBehaviour {
	[SerializeField]private Flight target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClick(){
		target.stabZ = true;

		target.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
	}

}
