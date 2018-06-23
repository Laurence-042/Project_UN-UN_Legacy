using UnityEngine;
using System.Collections;

public class OperationWindow : MonoBehaviour {

	private UIButton[] btn;

	// Use this for initialization
	void Start () {
		btn = GetComponentsInChildren<UIButton>();

		for (int i = 0; i < btn.Length; i++) {
			UIEventListener.Get(btn[i].gameObject).onClick = CallBack;
		}
	}

	void CallBack(GameObject obj) {
		string name = obj.name;
		switch (name) {
		case "SettingBtn" : {
				UIWindowMag.GetInstance().OpenWindow("audioSettingwindow");
				break;
			}
		case "..." : {
				break;
			}
		}
	}

}