using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFreeze : MonoBehaviour {
	[SerializeField]private Transform target;

	public float delay = 3.0f;
	public float decreaseFactor = 1.0f;

	public Vector3 pos=Vector3.zero;

	private float timer=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer < delay*Time.deltaTime) {
			timer += Time.deltaTime * decreaseFactor;
		}else{
			transform.localPosition = target.localPosition + pos;
			timer = 0;
		}
	}
}
