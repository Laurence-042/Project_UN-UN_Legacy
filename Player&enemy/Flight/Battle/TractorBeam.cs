using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour {

	[SerializeField] private float force = 1f;//基本牵引力大小
	[SerializeField] private float radius=20f;//寻找对接物体范围
	[SerializeField] private GameObject lightning;

	public bool active = false;

	private GameObject[] targets=null;
	private List<GameObject> tractive;
	private List<GameObject> lightnings;
	private int targetIndex=0;

	private Vector3 _offset;
	private float distance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!active) {
			if (lightnings != null) {
				for (int i = 0; i < lightnings.Count; i++) {
					Destroy (lightnings [i]);
				}
			}
			targets = null;
			tractive = null;
			lightnings = null;
			targetIndex = 0;
		} else {
			if (tractive == null) {
				tractive = new List<GameObject> ();
				lightnings = new List<GameObject> ();
				Collider[] targets = Physics.OverlapSphere (transform.position, radius);//获取范围内可能可牵引物体
				for (int i = 0; i < targets.Length; i++) {
					if (targets [i].gameObject.tag == "Tractive") {//可牵引物体的tag应设为"Tractive"
						tractive.Add (targets [i].gameObject);
						lightnings.Add (Instantiate (lightning, gameObject.transform));
					} 
				}
			}


			if (tractive.Count > 0) {//如果有可牵引物体
				for (int i = 0; i < tractive.Count; i++) {
					_offset = gameObject.transform.position - tractive [i].transform.position;
					distance = Vector3.Distance (gameObject.transform.position, tractive [i].transform.position);
					tractive [i].GetComponent<Rigidbody> ().AddForce (_offset * force * (float)System.Math.Atan (distance - 0.8 * radius));
					lightnings [i].GetComponent<Transform> ().Find ("LightningEnd").gameObject.transform.position = tractive [i].transform.position;

					/*
					//引力方案失败
					if (distance > 0.5 * radius) {
						float temp = (float)(distance - 0.5 * radius);
						tractive [i].GetComponent<Rigidbody> ().AddForce (_offset * force /(temp*temp));
					} else {
						float temp = (float)(distance - 0.5 * radius);
						tractive [i].GetComponent<Rigidbody> ().AddForce (-1*_offset * force /(temp*temp));
					}
					*/
					//Debug.Log ("正在牵引 "+tractive [i].name);
				}
			} else {
				active = false;
			}

		}
	}
		
	void TractorButtonDown(){
		active = !active;
	}
		
}
