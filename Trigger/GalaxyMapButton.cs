using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalaxyMapButton : MonoBehaviour
{
    public int toLevel;
    public int nowLevel;
    [SerializeField] List<int> linkedLevelIndex;
    // Use this for initialization
    void Start()
    {
        nowLevel = PlayerPrefs.GetInt("NowLevelIndex");
        //Button btn = this.GetComponent<Button>();
        //btn.onClick.AddListener(OnClick);
        GetComponentInChildren<Text>().text = "Level_0" + (toLevel-4).ToString();
        if (nowLevel == toLevel)
        {
            foreach(GameObject btn in GameObject.FindGameObjectsWithTag("GalaxyMapBtn"))
            {
                int i = 0;
                for (i = 0; i < linkedLevelIndex.Count; i++)
                {
                    if (linkedLevelIndex[i] == btn.GetComponent<GalaxyMapButton>().toLevel)
                    {
                        GetComponent<Image>().color = new Color(102f / 255f, 204f / 255f, 255f / 255f, 1f);
                        Debug.Log(btn.GetComponent<GalaxyMapButton>().toLevel + " purple");
                        break;
                    }
                }
                if(i== linkedLevelIndex.Count)
                {
                    btn.GetComponent<Image>().color = new Color(238f / 255f, 0f, 0f, 1f);
                    Debug.Log(btn.GetComponent<GalaxyMapButton>().toLevel + " red");
                }
            }
            
            GetComponent<Image>().color = new Color(102f / 255f, 204f / 255f, 255f / 255f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        nowLevel = PlayerPrefs.GetInt("NowLevelIndex");
    }

    public void OnClick()
    {
        Debug.Log("Switch Level");
        for (int i = 0; i < linkedLevelIndex.Count; i++)
        {
            if (linkedLevelIndex[i] == nowLevel)
            {
                GetComponent<SwitchLevel>().nowlevelIndex = PlayerPrefs.GetInt("GalaxyMapIndex");
                GetComponent<SwitchLevel>().targetLevelIndex = toLevel;

                GetComponent<SwitchLevel>().unloadSelf = true;
                GetComponent<SwitchLevel>().Switch();
                return;
            }
        }
    }

}
