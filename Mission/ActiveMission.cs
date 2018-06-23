using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveMission : MonoBehaviour
{
    [SerializeField] private string MissionName = "test";//任务名称，同时也用于面板
    [SerializeField] private string MissionIntroduction = "test";//任务介绍，仅用于面板
    [SerializeField] private string missionCode = "000";//格式：xxx
    [SerializeField] private string missionRequest = "test";//任务要求
    [SerializeField] public int missionPercent = 0;//任务完成量（不一定用百分比）
    [SerializeField] public int missionPercentRequest = 6;//任务需要完成量（不一定用百分比）
    [SerializeField] private GameObject MissionPanelPre;
    [SerializeField] private Vector3 MissionPanelPreScale=new Vector3(1,1);
    [SerializeField] private GameObject MissionDataPre;

    private Text MissionNameText;
    private Text MissionIntroductionText;

    private MissionData missionData;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivePanel()
    {
        GameObject MissonPanel = Instantiate(MissionPanelPre);
        MissonPanel.GetComponent<RectTransform>().SetParent(GameObject.Find("TempCanvas").transform);
        MissonPanel.GetComponent<RectTransform>().localPosition = Vector3.zero;
        MissonPanel.GetComponent<RectTransform>().localScale = MissionPanelPreScale;

    Text[] labels = MissonPanel.GetComponentsInChildren<Text>();
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].name == "MissionName")
            {
                MissionNameText = labels[i];
            }
            if (labels[i].name == "MissionIntroduction")
            {
                MissionIntroductionText = labels[i];
            }
        }
        MissionNameText.text = MissionName;
        MissionIntroductionText.text = MissionIntroduction;


        GameObject missionDataCube = Instantiate(MissionDataPre, Vector3.zero, Quaternion.Euler(Vector3.zero));
        missionDataCube.transform.parent = GameObject.Find("MissionRecorder").transform;
        if (missionDataCube.GetComponent<MissionData>() == null)
        {
            missionDataCube.AddComponent<MissionData>();
        }

        missionData = missionDataCube.GetComponent<MissionData>();

        missionData.missionCode = missionCode;
        missionData.missionName = MissionName;
        missionData.missionRequest = missionRequest;
        missionData.missionPercent = missionPercent;

        Debug.Log(missionData.missionName);

        int missionNum = GameObject.Find("MissionRecorder").GetComponent<MissionRecorder>().missionList.Count;
        missionDataCube.name = "missionDataCube_" + missionNum.ToString();

        GameObject.Find("MissionRecorder").GetComponent<MissionRecorder>().missionList.Add(missionDataCube);



    }

}
