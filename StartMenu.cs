using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject camera0;
    //[SerializeField]private GameObject camera1;
    [SerializeField] private GameObject target;
    [SerializeField] private int LevelSceneIndex = 2;
    [SerializeField] private int MainSceneIndex = 4;
    [SerializeField] private int GalaxyMapIndex=6;
    [SerializeField] private int tempSceneIndex = 5;
    [SerializeField] private int self_index = 0;
    [SerializeField] private int LoadingSecne_index = 7;
    [SerializeField] private bool loadMain;

    [SerializeField] private UILabel LevelNum;//仅供测试使用，以后用PlayerPrefs获取字符串代替

    [SerializeField] private GameObject[] FX;

    public Vector3 switchTo = Vector3.zero;

    private UIButton[] btn;
    private bool started = false;

    private float timer = 2;

    private int LevelSetting;

    // Use this for initialization
    void Start()
    {
        //camera1.SetActive (false);
        if (!SceneManager.GetSceneByBuildIndex(tempSceneIndex).IsValid())
        {
            SceneManager.LoadScene(tempSceneIndex, LoadSceneMode.Additive);
        }
        PlayerPrefs.SetInt("LoadingSceneIndex", LoadingSecne_index);
        PlayerPrefs.SetInt("MainSceneIndex", MainSceneIndex);
        PlayerPrefs.SetInt("GalaxyMapIndex", GalaxyMapIndex);
        btn = GetComponentsInChildren<UIButton>();
        for (int i = 0; i < btn.Length; i++)
        {
            UIEventListener.Get(btn[i].gameObject).onClick = CallBack;
        }

        PlayerPrefs.SetInt("LoadMain", 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (started)
        {

            camera0.transform.position = Vector3.Lerp(camera0.transform.position, switchTo, 2 * Time.deltaTime);
            camera0.transform.LookAt(target.transform.position + new Vector3(0, 4 - 2 * timer, 0));
            if (Vector3.Distance(camera0.transform.position, switchTo) < 0.1)
            {
                //camera0.SetActive (false);
                //camera1.SetActive (true);
                timer -= 2f * Time.deltaTime;
                if (timer < 0 && timer != -1)
                {
                    PlayerPrefs.SetInt("LoadIndex", LevelSceneIndex);
                    if (loadMain)
                    {
                        PlayerPrefs.SetInt("LoadMain", 1);
                    }
                    else
                    {
                        PlayerPrefs.SetInt("LoadMain", 0);
                    }
                    SceneManager.LoadScene(LoadingSecne_index, LoadSceneMode.Additive);

                    SceneManager.UnloadSceneAsync(self_index);
                    timer = -1;
                }
            }
        }
    }


    void CallBack(GameObject obj)
    {
        string name = obj.name;
        switch (name)
        {
            case "Start":
                {
                    Debug.Log("press Start");
                    started = true;

                    LoadData("");
                    StartText();
                    break;
                }
            case "Continue":
                {
                    Debug.Log("press Continue");
                    started = true;
                    LoadData(PlayerPrefs.GetString("savedData"));
                    StartText();
                    break;
                }
            case "...":
                {
                    break;
                }
        }
    }

    void StartText()
    {
        for (int i = 0; i < FX.Length; i++)
        {
            FX[i].SetActive(true);
        }
        //GameObject text = UIWindowMag.GetInstance ().OpenWindow ("audioSettingwindow");
    }

    void LoadData(string savedData)
    {
        if (LevelNum.text != "NA")
        {
            bool imputCheck = true;

            int temp = int.Parse(LevelNum.text);
            string key;
            if (temp < 10)
            {
                key = "00" + temp.ToString();

            }
            else if (temp < 100)
            {
                key = "0" + temp.ToString();
            }
            else if (temp < 1000)
            {
                key = "" + temp.ToString();
            }
            else
            {
                imputCheck = false;
                key = "";
                Debug.Log("levelNum Error");
            }
            if (imputCheck)
            {
                string levelData = PlayerPrefs.GetString("Level_" + key);
                if (levelData[0] == '1')
                {
                    LevelSceneIndex = temp;
                    if (levelData[1] == '1')
                    {
                        loadMain = true;
                    }
                    else
                    {
                        loadMain = false;
                    }
                }
                else
                {
                    Debug.Log("Unable to load " + "Level_" + key);
                    Debug.Log("Level_" + key + " data: " + levelData);
                }
            }
        }
        else if (savedData != "")
        {
            LevelSceneIndex = int.Parse(savedData.Split('_')[0]);
            string levelData = PlayerPrefs.GetString("Level_" + savedData.Split('_')[0]);

            if (levelData[1] == '1')
            {
                loadMain = true;
            }
            else
            {
                loadMain = false;
            }
            Debug.Log("Level_" + savedData.Split('_')[0] + " data: " + levelData);

        }
        else
        {
            Debug.Log("读取存档出错: "+ savedData);
        }
    }


}
