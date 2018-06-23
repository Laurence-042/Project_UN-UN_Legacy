using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour {
	public GameObject target;
	public Vector3 pos;
	public float rotAngel;

	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = pos;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = target.transform.localPosition + offset;
		transform.RotateAround(target.transform.localPosition, Vector3.up, rotAngel * Time.deltaTime);
		offset = transform.localPosition - target.transform.transform.localPosition;
	}
}
