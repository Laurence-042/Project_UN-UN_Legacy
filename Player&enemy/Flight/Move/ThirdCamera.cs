using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamera : MonoBehaviour {
	[SerializeField]private Transform target;
	[SerializeField]private float distanceX=36.0f;
	[SerializeField]private float distanceY=5.0f;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		transform.position = target.position - distanceX * Vector3.forward + distanceY * Vector3.up;
	}
}
