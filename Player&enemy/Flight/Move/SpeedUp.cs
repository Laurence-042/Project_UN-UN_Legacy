using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {
	public int reUsingTimer=6;
	public int reUsedTimer=12;
	public bool starting = false;

	[SerializeField] private float usingTimer=6f;
	[SerializeField] private float usedTimer=12f;
	[SerializeField] private bool used = false;

	private float temp_speed;
	private int temp_usingTimer=6;
	private int temp_usedTimer=12;
	private bool speedUpFinished=false;

	public UISprite CD;
	public UISprite LeftTime;


	public Flight flight;

	[SerializeField]private GameObject[] effect0;//0:halo
	[SerializeField]private GameObject[] effect1;

	[SerializeField]private float multiply=2f;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < effect0.Length; i++) {
			effect0[i].SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (starting&&!used) {
			temp_speed = flight.speed;
			flight.maxSpeed *= multiply;
			flight.speedUp = true;

			used = true;
			CD.fillAmount = 1;
			LeftTime.fillAmount = 1;
			usingTimer = reUsingTimer;
			usedTimer = reUsedTimer;
			temp_usingTimer = reUsingTimer;
			temp_usedTimer = reUsedTimer;

			starting = false; 

			for (int i = 0; i < effect0.Length; i++) {
				effect0[i].SetActive (true);
			}

		}else{
			starting = false;
		}

		if (used && usingTimer > 0) {
			//flight.speed = 20;
			flight.speed = Mathf.Lerp (flight.speed, flight.maxSpeed, 5 * Time.deltaTime);
			LeftTime.fillAmount = usingTimer / reUsingTimer;

			usingTimer -= 1 * Time.deltaTime;
			if(flight.speed>flight.maxSpeed-0.01){
				speedUpFinished = true;
			}
			/*
			if (usingTimer - temp_usingTimer < -1) {
				temp_usingTimer--;

				//Debug.Log ("加速仍可持续" + temp_usingTimer);
			}*/
		}

		if (usingTimer < 0||(flight.speed<flight.maxSpeed-0.2&&speedUpFinished)) {
			flight.maxSpeed /= multiply;
			flight.speed = flight.maxSpeed;
			flight.speedUp = false;

			usingTimer = 0;
			speedUpFinished = false;
			LeftTime.fillAmount = 0;
			for (int i = 0; i < effect0.Length; i++) {
				effect0[i].SetActive (false);
			}
		}

		if (used && usedTimer > 0) {
			CD.fillAmount = usedTimer / reUsedTimer;
			usedTimer -= 1 * Time.deltaTime;
			/*
			if (usedTimer - temp_usedTimer < -1) {
				temp_usedTimer--;

				//Debug.Log ("加速冷却还剩" + temp_usedTimer);
			}*/
		} 
		if (usedTimer <= 0) {
			used = false;
		}


	}

	void OnClick(){
		if (!used) {
			/*
			trail.SetActive (true);
			temp_speed = flight.speed;
			flight.maxSpeed = 20;
			used = true;
			CD.fillAmount = 1;
			LeftTime.fillAmount = 1;
			usingTimer = reUsingTimer;
			usedTimer = reUsedTimer;
			temp_usingTimer = reUsingTimer;
			temp_usedTimer = reUsedTimer;
			*/
			Debug.Log ("click SpeedUp");
		}

	}
}
