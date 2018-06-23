using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    Scene scene;

    public bool unloadSelf = true;
    [SerializeField] bool unloadSelfF = false;
    public int nowlevelIndex;
    public int thisLevelIndex;
    public int targetLevelIndex;
    [SerializeField] bool isMapSwitcher = false;
    [SerializeField] private bool resetPlayer = true;
    [SerializeField] private Vector3 pos;
    [SerializeField] private Vector3 rot;

    public void Switch()
    {
        PlayerPrefs.SetInt("LoadIndex", targetLevelIndex);
        PlayerPrefs.SetInt("NowLevelIndex", nowlevelIndex);

        SceneManager.LoadScene(PlayerPrefs.GetInt("LoadingSceneIndex"), LoadSceneMode.Additive);


        if (resetPlayer)
        {
            TempController controller = GameObject.FindGameObjectWithTag("TempController").GetComponent<TempController>();
            controller.resetPos = pos;
            controller.resetRot = rot;
            controller.reset = true;
            Debug.Log("已调用" + controller);
        }
        string levelName = "Level_0" + (targetLevelIndex > 9 ? targetLevelIndex.ToString() : "0" + targetLevelIndex.ToString());
        Debug.Log("Loading " + levelName);
        if (isMapSwitcher)
        {
            if (PlayerPrefs.GetString(levelName)[1] == '0')
            {
                PlayerPrefs.SetInt("LoadMain", 0);

                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    if (SceneManager.GetSceneAt(i).name == "U.N.'")
                    {
                        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
                    }
                }

            }
            else
            {
                nowlevelIndex = GetComponent<GalaxyMapButton>().nowLevel;
                levelName = "Level_0" + (nowlevelIndex > 9 ? nowlevelIndex.ToString() : "0" + nowlevelIndex.ToString());
                PlayerPrefs.SetInt("LoadMain", (PlayerPrefs.GetString(levelName)[1] == '0') ? 1 : 0);

            }

            SceneManager.UnloadSceneAsync(PlayerPrefs.GetInt("GalaxyMapIndex"));

        }
        else
        {
            PlayerPrefs.SetInt("LoadMain", 0);

            if (GameObject.FindGameObjectWithTag("KeepPlayer"))
            {
                GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>().stopPlane = true;
            }
            if (unloadSelf)
            {
                Debug.Log("unloadSelf");
                SceneManager.UnloadSceneAsync(thisLevelIndex);
            }
        }

        if (unloadSelfF)
        {
            SceneManager.UnloadSceneAsync(thisLevelIndex);
        }
    }
}

