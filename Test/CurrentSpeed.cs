using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSpeed : MonoBehaviour {
	public float speed;

	private float lastTime;
	private Vector3 lastPos;
	// Use this for initialization
	void Start () {
		lastPos = transform.position;
		lastTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		speed = Vector3.Distance (lastPos, transform.position) / (Time.time - lastTime);
		lastPos = transform.position;
		lastTime = Time.time;
	}
}
