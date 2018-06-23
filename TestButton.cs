using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour {
	[SerializeField]private GameObject target1;
	[SerializeField]private GameObject target2;

	/*void Awake()  
	{  
		//获取需要监听的按钮对象  
		GameObject button= GameObject.Find("UI Root/Panel/Anchor_topRight/Test");  
		//设置这个按钮的监听，指向本类的ButtonClick方法中。  
		UIEventListener.Get(button).onClick= ButtonClick;  
	}  

	//计算按钮的点击事件  
	void ButtonClick(GameObject button)  
	{  
		Debug.Log ("收到点击");
		if (target1.shaking){
			Debug.Log ("取消效果");
			target1.shaking = false;
			target2.shaking = false;

		}else{
			target1.shaking = true;
			target2.shaking = true;
			Debug.Log ("应用效果");
		}
				

	}*/
	void OnClick()  
	{  
		Debug.Log("Button is Click!!!");
		if (target1.GetComponent<Shake>().shaking){
			target1.GetComponent<Shake>().shaking = false;
			target2.GetComponent<Shake>().shaking = false;
			target1.GetComponent<DelayFreeze> ().pos = new Vector3 (-4.3f, 0, -2);
			target2.GetComponent<DelayFreeze> ().pos = new Vector3 (4.3f, 0, -2);

			Debug.Log ("取消效果");

		}else{
			target1.GetComponent<Shake>().shaking = true;
			target2.GetComponent<Shake>().shaking = true;
			target1.GetComponent<DelayFreeze> ().pos = new Vector3 (-4.7f, 0, -3);
			target2.GetComponent<DelayFreeze> ().pos = new Vector3 (4.7f, 0, -3);
			Debug.Log ("应用效果");
		}
	}  
}
