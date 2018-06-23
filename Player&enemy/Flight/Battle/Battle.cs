using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour {
	[SerializeField]private UIButton[] btn;

	// Use this for initialization
	void Start () {
		btn = GetComponentsInChildren<UIButton> ();
		for (int i = 0; i < btn.Length; i++) {
			Debug.Log ("btn " + i + " name: " + btn [i].name);
			UIEventListener.Get(btn[i].gameObject).onClick = CallBack;
		}
	}

	void CallBack(GameObject obj) {
		string name = obj.name;
		switch (name) {

		case "SpeedUp" : {
				SpeedUp execution = obj.GetComponent<SpeedUp> ();
				execution.starting = true;
				break;
			}
		case "Fire" : {
				Fire();
				break;
			}
		}
	}

	void Update(){

			
	}

	void Fire(){
		
	}

}
