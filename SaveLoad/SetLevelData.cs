using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLevelData : MonoBehaviour {
    [SerializeField] private string[] levelNum;//格式：xxx “xxx”为三位关卡代码
    [SerializeField] private string[] setting;
    /*
    第一位：
        0：无效
        1：有效
    第二位：
        0:不加载U.N.
        1:加载U.N.
    */
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < levelNum.Length; i++)
        {

            PlayerPrefs.SetString(levelNum[i], setting[i]);
        }
		
	}
}
