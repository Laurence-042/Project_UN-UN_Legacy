using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock_SpeedUp : MonoBehaviour {
	[SerializeField] private UISprite CD;
	[SerializeField] private UISprite LeftTime;
	// Use this for initialization
	void Start () {
		CD = GameObject.Find ("/UI Root/Panel/Anchor_bottom/Battle/SpeedUp/Sprite").GetComponent<UISprite> ();
		LeftTime = GameObject.Find ("/UI Root/Panel/Anchor_center/MP/MP_time").GetComponent<UISprite> ();

		gameObject.GetComponent<SpeedUp> ().flight = GameObject.Find("Player").GetComponent<Flight> ();
		gameObject.GetComponent<SpeedUp> ().CD = CD;
		gameObject.GetComponent<SpeedUp> ().LeftTime = LeftTime;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider target){
		if (target.gameObject.tag == "Player") {
			
			target.gameObject.GetComponent<DockGuide> ().docking = false;

			gameObject.GetComponent<SpeedUp> ().starting = true;

		}
	}

}
