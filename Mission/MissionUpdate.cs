using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionUpdate : MonoBehaviour {
    [SerializeField] private MissionRecorder missionRecorder;

    [SerializeField]private string missionName;
    [SerializeField] private int newMissionPercentChange;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpDateMission()
    {
        missionRecorder.MissionUpdate(missionName, newMissionPercentChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!GameObject.Find("MissionRecorder"))
        {
            Debug.Log("未找到 MissionRecorder");
        }
        else
        {
            missionRecorder = GameObject.Find("MissionRecorder").GetComponent<MissionRecorder>();
            UpDateMission();
        }
        
    }


}
