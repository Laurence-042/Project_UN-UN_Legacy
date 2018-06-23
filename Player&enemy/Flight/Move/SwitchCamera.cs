using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {
	[SerializeField]private Transform target;
	[SerializeField]private EasyJoystick camJoystick;
	[SerializeField]private EasyJoystick rotJoystick1;
	[SerializeField]private EasyJoystick rotJoystick2;
	[SerializeField]private GameObject hud;
	[SerializeField]private GameObject camera0;
	[SerializeField]private GameObject camera1;
	[SerializeField]private GameObject keepCamera1;

	public float rotSpeed=1.5f;

	private bool roting=false;
	private float _rotY=0;

	// Use this for initialization
	void Start () {
		//camera1.SetActive (false);
		camera1=keepCamera1.GetComponentInChildren<Camera> ().gameObject;
		camera1.SetActive (false);

		rotJoystick2.isActivated = false;
		camJoystick.isActivated = false;

		_rotY = keepCamera1.transform.eulerAngles.y;
	}

	void OnEnable(){
		EasyJoystick.On_JoystickMove += On_JoystickMove;
		EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
	}

	// Update is called once per frame
	void Update () {
		//keepCamera1.transform.eulerAngles = new Vector3 (0, _rotY, 0);
		//camera1.transform.LookAt (target);

		//Quaternion rotation = Quaternion.Euler (target.transform.eulerAngles.x,target.transform.eulerAngles.y, target.transform.eulerAngles.z);
		//keepCamera1.transform.rotation = rotation;
		keepCamera1.transform.eulerAngles=new Vector3(0,_rotY, 0);
		//Debug.Log (keepCamera1.transform.eulerAngles+""+_rotY);
	}
			
		
	void OnClick(){
		if (camera0.activeSelf) {
			camera0.SetActive (false);
			camera1.SetActive (true);
			//camera1.SetActive (true);
			hud.SetActive (false);
			camJoystick.isActivated = true;

			rotJoystick1.isActivated = false;
			rotJoystick2.isActivated = true;

		} else {
			camera1.SetActive (false);
			camera0.SetActive (true);
			hud.SetActive (true);
			camJoystick.isActivated = false;

			rotJoystick1.isActivated = true;
			rotJoystick2.isActivated = false;
		}
	}

	void On_JoystickMove (MovingJoystick move)
	{
		
		if (move.joystickName == "CamJoystick") {
			Debug.Log ("roting");
			roting = true;
			//_offset = target.position - camera1.transform.position;
			float horInput = move.joystickAxis.x;

			_rotY += horInput * rotSpeed * Time.deltaTime;
			//_rotY -= target.transform.eulerAngles.y;
			if (_rotY >= 180) {
				_rotY -= 360;
			} else if (_rotY < -180) {
				_rotY += 360;
			}


			
		}

	}

	void On_JoystickMoveEnd(MovingJoystick move){
		if (move.joystickName == "CamJoystick") {
			//Debug.Log ("rotend");
			roting = false;
		}
	}

}
