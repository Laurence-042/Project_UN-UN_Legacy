using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MonoBehaviour {
	private Guide[] guide;
	[SerializeField]private GameObject flowChart;
	[SerializeField]private GameObject portal;
	private int count=0;

	private bool actived=false;

	private int linkTo=0;
	// Use this for initialization
	void Start () {
		flowChart.SetActive (false);
		portal.SetActive (false);

		guide = GetComponentsInChildren<Guide>();

		GetComponent<Light>().transform.position = guide [0].transform.position;

		for (int i = 0; i < guide.Length; i++) {
			guide [i].actived = false;
			ParticleSystem glory = guide [i].GetComponentInChildren<ParticleSystem> ();
			glory.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		count = 0;
		for (int i = 0; i < guide.Length; i++) {
			if (guide [i].actived)
				count++;
		}
		if (count == guide.Length) {
			GetComponent<Light>().transform.position = guide [linkTo].transform.position;
			linkTo++;
			if (linkTo == guide.Length) {
				linkTo = 0;
			}
			if (!actived) {
				actived = true;
				Congratulation ();
			}
		}
	}

	void Congratulation(){
		flowChart.SetActive (true);
		portal.SetActive (true);
	}
}
