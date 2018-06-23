using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionRecorder : MonoBehaviour
{
    [SerializeField] private GameObject MissionListPanel;//任务列表的外框
    [SerializeField] private GameObject MissionPanelMiniPre;//任务列表的卡片
    [SerializeField] private float MissionPanelMiniPreHight = 75f;
    [SerializeField] private Vector3 MissionPanelMiniPreScale = new Vector3(1, 1, 1);

    public List<GameObject> missionList = new List<GameObject>();
    public List<GameObject> missionListMini = new List<GameObject>();

    private int count;
    float sliderValue;

    Slider posSlider;//任务列表上下拉动的滑条
    float sliderSen;


    // Use this for initialization
    void Start()
    {
        count = missionList.Count;

        posSlider = MissionListPanel.GetComponentInChildren<Slider>();
        sliderValue = posSlider.value;
        sliderSen = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMissionNum();

        if (PlayerPrefs.GetInt("LoadMain")==1)
        {
            MissionListPanel.SetActive(true);
        }
        else
        {
            MissionListPanel.SetActive(false);
        }


    }

    private void CheckMissionNum()
    {
        if (missionList.Count > count)
        {
            for (int i = 0; i < missionList.Count; i++)
            {
            }
            count = missionList.Count;
            AddMiniPanel(missionList[count - 1]);

            Debug.Log("Add panel, reset slider");
            if (missionListMini.Count > 4)
            {
                posSlider.value = posSlider.value * (missionListMini.Count - 1) / missionListMini.Count;
            }
        }
        else
        {
            if (missionList.Count < count)
            {
                count = missionList.Count;
                Debug.Log("SUb panel, reset slider");
                if (missionListMini.Count > 3)
                {
                    posSlider.value = posSlider.value * (missionListMini.Count) / (missionListMini.Count + 1);
                }
            }
        }

        if (missionList.Count > 3)
        {
            sliderSen = (missionList.Count - 3) * MissionPanelMiniPreHight;
            if (sliderValue != posSlider.value)
            {
                sliderValue = posSlider.value;

            }
        }
        else
        {
            if (posSlider.value != 0)
            {
                Debug.Log("卡片数过少,重置滑动条");
                posSlider.value = 0;
                sliderSen = 0;

            }
        }

        for (int i = 0; i < missionListMini.Count; i++)
        {
            missionListMini[i].GetComponent<RectTransform>().localPosition = new Vector3(0, MissionPanelMiniPreHight - MissionPanelMiniPreHight * i + sliderSen * sliderValue);
        }
    }

    public void MissionRemove(string missionName)
    {
        for (int i = 0; i < missionList.Count; i++)
        {
            if (missionList[i].GetComponent<MissionData>().missionName == missionName)
            {
                GameObject temp = missionList[i];
                missionList.Remove(missionList[i]);
                Destroy(temp);

                temp = missionListMini[i];
                missionListMini.Remove(missionListMini[i]);
                Destroy(temp);
                for (int j = i; j < missionListMini.Count; j++)
                {
                    missionListMini[j].GetComponent<RectTransform>().localPosition += new Vector3(0, MissionPanelMiniPreHight, 0);
                }
                count--;
                break;
            }

        }
    }

    public void MissionUpdate(string missionName, int newMissionPercentChange)
    {
        for (int i = 0; i < missionList.Count; i++)
        {
            if (missionList[i].GetComponent<MissionData>().missionName == missionName)
            {
                missionList[i].GetComponent<MissionData>().missionPercent += newMissionPercentChange;
                if (missionList[i].GetComponent<MissionData>().missionPercent >= missionList[i].GetComponent<MissionData>().missionPercentRequest)
                {
                    Debug.Log(missionList[i].GetComponent<MissionData>().missionName + " 已完成");
                    missionList[i].GetComponent<MissionData>().missionCompleted = true;
                    missionListMini[i].GetComponentsInChildren<Text>()[1].text = "[v]" + missionList[i].GetComponent<MissionData>().missionPercent.ToString() + " / " + missionList[i].GetComponent<MissionData>().missionPercentRequest.ToString();
                    missionList[i].GetComponent<MissionData>().missionPercent = missionList[i].GetComponent<MissionData>().missionPercentRequest;

                }
                else
                {
                    missionListMini[i].GetComponentsInChildren<Text>()[1].text = "[ ]" + missionList[i].GetComponent<MissionData>().missionPercent.ToString() + " / " + missionList[i].GetComponent<MissionData>().missionPercentRequest.ToString();
                }
            }

        }
    }

    private void AddMiniPanel(GameObject missionDataCube)
    {
        MissionData data = missionDataCube.GetComponent<MissionData>();

        GameObject MissonPanelMini = Instantiate(MissionPanelMiniPre);
        missionListMini.Add(MissonPanelMini);

        MissonPanelMini.GetComponent<RectTransform>().SetParent(MissionListPanel.transform);
        if (missionListMini.Count != 1)
        {
            MissonPanelMini.GetComponent<RectTransform>().localPosition = missionListMini[missionListMini.Count - 2].GetComponent<RectTransform>().localPosition - new Vector3(0, MissionPanelMiniPreHight);

        }
        else
        {
            MissonPanelMini.GetComponent<RectTransform>().localPosition = new Vector3(0, MissionPanelMiniPreHight);
        }

        MissonPanelMini.GetComponent<RectTransform>().localScale = MissionPanelMiniPreScale;

        Text[] labels = MissonPanelMini.GetComponentsInChildren<Text>();

        Text MissionNameText = labels[0];
        Text MissionRequestText = labels[0];
        for (int i = 0; i < labels.Length; i++)
        {
            if (labels[i].name == "MissionName")
            {
                MissionNameText = labels[i];
            }
            if (labels[i].name == "MissionRequest")
            {
                MissionRequestText = labels[i];
            }
        }
        MissionNameText.text = data.missionName;
        MissionRequestText.text = "[ ]" + data.missionPercent.ToString() + " / " + data.missionPercentRequest.ToString();
    }



}
