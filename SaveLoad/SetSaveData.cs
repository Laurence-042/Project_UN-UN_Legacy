using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSaveData : MonoBehaviour {
    private GameObject saver;
    private Saver saver_script;
    [SerializeField] private string saveData;
    // Use this for initialization
    void Start () {
        saver = GameObject.FindGameObjectWithTag("Saver");
        if (saver == null)
        {
            Debug.Log("Saver 丢失");
        }
        else
        {
            saver_script = saver.GetComponent<Saver>();
            if (saver_script == null)
            {
                Debug.Log("Saver 上的Saver脚本丢失");
            }
        }
	}
	
	// Update is called once per frame
	public void SaveNow()
    {
        if (saveData[3] == '_' && saveData[7] == '_')
        {
            saver_script.saveValue = saveData;
            saver_script.saveTimer = 0;
        }
        else
        {
            Debug.Log("存储数据格式错误");
        }
    }
}
