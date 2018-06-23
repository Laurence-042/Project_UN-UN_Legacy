using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    public Slider m_Slider;
    public Slider m_MainSlider;
    public int selfIndex;
    public int mainIndex;

    private bool levelLoaded = false;

    // Use this for initialization
    void Start()
    {
        selfIndex = PlayerPrefs.GetInt("LoadingSceneIndex");
        mainIndex = PlayerPrefs.GetInt("MainSceneIndex");

        int loadIndex = PlayerPrefs.GetInt("LoadIndex");
        int loadMain = PlayerPrefs.GetInt("LoadMain");
        if (loadMain == 1)
        {
            StartCoroutine(LoadScene(mainIndex, true));
        }
        else
        {
            m_MainSlider.value = 1f;
        }
        StartCoroutine(LoadScene(loadIndex, false));

    }

    IEnumerator LoadScene(int loadIndex, bool loadMain)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("loadIndex=" + loadIndex + ",loadMain=" + loadMain);
        int displayProgress = 0;
        int toProgress = 0;
        //AsyncOperation op = Application.LoadLevelAsync(Global.GetInstance().loadName);
        AsyncOperation op = SceneManager.LoadSceneAsync(loadIndex, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)(op.progress * 100f);
            while (displayProgress <= toProgress)
            {
                //Debug.Log("loadIndex=" + loadIndex + ",opProgress:" + op.progress);
                ++displayProgress;
                SetLoadingPercentage(displayProgress, loadMain);
                //yield return new WaitForEndOfFrame();
            }
            //Debug.Log("loadIndex=" + loadIndex + ",out opProgress:" + op.progress);
        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {

            //Debug.Log("1:" + displayProgress);
            ++displayProgress;
            SetLoadingPercentage(displayProgress, loadMain);
            yield return new WaitForEndOfFrame();
        }

        if (loadIndex != mainIndex)
        {
            op.allowSceneActivation = true;
            levelLoaded = true;

        }
        else
        {
            while (!Application.CanStreamedLevelBeLoaded(PlayerPrefs.GetInt("LoadIndex")))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3f);
            op.allowSceneActivation = true;
        }

        Scene scenesLoaded = SceneManager.GetSceneAt(0);

        for (int i = 0; scenesLoaded != null; i++)
        {
            if (scenesLoaded.name != "temp" && scenesLoaded.name != "Loader"&& scenesLoaded.name != "U.N.'")
            {
                while (!scenesLoaded.isLoaded)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                SceneManager.SetActiveScene(scenesLoaded);

                Debug.Log("set " + scenesLoaded.name + " active");
                break;
            }
            else
            {
                scenesLoaded = SceneManager.GetSceneAt(i + 1);
            }

        }

        Debug.Log("activing " + loadIndex + ",UnLoading " + selfIndex);
        PlayerPrefs.SetInt("LoadIndex", -1);
        //PlayerPrefs.SetInt("LoadMain", -1);
        SceneManager.UnloadSceneAsync(selfIndex);

    }
    public void SetLoadingPercentage(int DisplayProgress, bool loadMain)
    {
        if (loadMain)
        {
            m_MainSlider.value = (float)DisplayProgress / 100;
        }
        else
        {
            m_Slider.value = (float)DisplayProgress / 100;
        }

    }
}