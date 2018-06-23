using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionData : MonoBehaviour {
    public string missionCode= "000";//格式：xxx
    public string missionName= "test";//任务名字
    public string missionRequest= "test";//任务要求
    public int missionPercent= 0;//任务完成量（不一定用百分比）
    public int missionPercentRequest = 6;//任务需要完成量（不一定用百分比）
    public bool missionCompleted = false;//任务是否完成

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
