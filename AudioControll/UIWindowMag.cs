using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
    * 单例类，创建或者销毁窗体
     */
public class UIWindowMag {

	private static UIWindowMag windowMag;
	private static Dictionary<string, GameObject> windowCache = new Dictionary<string, GameObject>();

	private UIWindowMag() {}

	public static UIWindowMag GetInstance() {
		if (windowMag == null) {
			windowMag = new UIWindowMag();
		}
		return windowMag;
	}

	public GameObject OpenWindow(string windowName) {
		if (windowCache.ContainsKey(windowName)) {
			return windowCache[windowName];
		}
		GameObject windowObject = Resources.Load("Prefabs/"+windowName) as GameObject;
		GameObject prefabClone = GameObject.Instantiate(windowObject);

		prefabClone.transform.parent = UIRoot.list[0].transform;
		prefabClone.transform.localPosition = Vector3.zero;
		prefabClone.transform.localScale = Vector3.one;
		windowCache.Add(windowName, prefabClone);
		return prefabClone;
	}
	public void CloseWindow(string windowName) {
		Debug.Log ("close");
		if (windowCache.ContainsKey(windowName)) {
			GameObject.Destroy(windowCache[windowName]);
			windowCache.Remove(windowName);
		}
	}

}