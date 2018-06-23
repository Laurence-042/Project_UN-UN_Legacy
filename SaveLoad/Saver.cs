using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    [SerializeField] private string saveKey = "savedData";
    public string saveValue = "";

    private float reSaveTimer = 6f;
    public float saveTimer = 6f;
    /*
    saveValue格式：xxx_xxx_xxx
    以'_'分割，从左到右分别是：三位关卡代号_任务代号_任务完成百分比
    */
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (saveTimer > 0)
        {
            saveTimer -= Time.deltaTime;
        }
        else if(saveValue!="")
        {
            PlayerPrefs.SetString(saveKey, saveValue);
            saveTimer = reSaveTimer;
        }

    }

    void SaveTimer()
    {
        PlayerPrefs.SetString(saveKey, saveValue);
    }

}
