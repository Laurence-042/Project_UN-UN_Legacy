using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockButton : MonoBehaviour {
	[SerializeField] private DockGuide ship;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnClick(){
		if (ship.docking) {
			ship.docking = false;
		} else {
			ship.docking = true;
		}
	}

}
