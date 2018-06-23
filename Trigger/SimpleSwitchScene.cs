using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSwitchScene : MonoBehaviour {
    [SerializeField] private string SwitchTo_Name;
	// Use this for initialization
	void Start () {
        if (SwitchTo_Name != null)
        {
            SceneManager.LoadSceneAsync(SwitchTo_Name);
        }
        else
        {
            Debug.Log("待加载场景名为空");
        }
	}

}
