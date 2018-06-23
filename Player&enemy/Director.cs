using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] bool marchMode = false;
    [SerializeField] Text marchModeScore;
    [SerializeField] List<AudioClip> marchModeAuClips;
    [SerializeField] int createPerSec = 3;
    [SerializeField] float createRange = 20f;
    [SerializeField] float creatDistance = 100f;
    [SerializeField] float destinationDistance = 2000f;
    [SerializeField] List<GameObject> obstaclePres;
    [SerializeField] List<GameObject> obstacles;
    [SerializeField] GameObject destination;

    [SerializeField] AudioSource auSource;
    [SerializeField] List<AudioClip> auClips;
    [SerializeField] List<GameObject> friend;
    [SerializeField] List<DestroyerFlight> destroyers;

    [SerializeField] List<float> audioTimeWindows;
    [SerializeField] List<float> destroyerTimeWindows;


    bool sentenceOver = false;
    int nowAuWindow = 0;
    int mowDestroyerWindow = 0;
    float timer = 0f;

    [SerializeField] AudioClip sentenceClip;
    [SerializeField] string sentence = "text";
    [SerializeField] float sentenceTime = 2.5f;//1淡入字+2保持字+1淡出字+1淡出画面
    [SerializeField] Image sentenceImage;
    [SerializeField] Text sentenceText;

    [SerializeField] List<GameObject> activeWhenSentenceOver;

    // Use this for initialization
    void Start()
    {
        if (marchMode)
        {
            if (destination != null)
            {
                destination.SetActive(false);
            }
            else
            {
                Debug.Log("目的地缺失");
            }
        }


        sentenceImage.color = new Color(sentenceImage.color.r, sentenceImage.color.g, sentenceImage.color.b, 1f);
        sentenceText.color = new Color(sentenceText.color.r, sentenceText.color.g, sentenceText.color.b, 0f);
        sentenceText.text = sentence.Replace("\\n", "\n");
        foreach (DestroyerFlight destroyer in destroyers)
        {
            destroyer.enabled = false;
        }
        foreach (GameObject obj in activeWhenSentenceOver)
        {
            obj.SetActive(false);
        }

        if (GameObject.FindGameObjectWithTag("KeepPlayer"))
        {
            GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>().savePoint = Vector3.zero;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
        if (player == null || player.tag != "Player")
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
                Debug.Log("Player set to " + player);
                if (player.GetComponent<RealFlight>())
                {
                    player.GetComponent<RealFlight>().marchMode = marchMode;
                }
            }
        }
        
        timer += Time.deltaTime;
        if (sentenceOver)
        {
            if (nowAuWindow < audioTimeWindows.Count)
                if (timer > audioTimeWindows[nowAuWindow])
                {
                    auSource.PlayOneShot(auClips[nowAuWindow]);
                    nowAuWindow++;
                }
            if (mowDestroyerWindow < destroyerTimeWindows.Count)
                if (timer > destroyerTimeWindows[mowDestroyerWindow])
                {
                    destroyers[mowDestroyerWindow].enabled = true;
                    mowDestroyerWindow++;
                }
        }
        else
        {

            if (timer < sentenceTime / 5f)
            {
                if (timer < 1f && !auSource.isPlaying)
                {
                    auSource.PlayOneShot(sentenceClip);
                }
                float a = sentenceText.color.a + 5f * Time.deltaTime / sentenceTime;
                if (a > 1f)
                {
                    a = 1f;
                }
                sentenceText.color = new Color(sentenceText.color.r, sentenceText.color.g, sentenceText.color.b, a);
            }
            else if (timer < sentenceTime * 3f / 5f)
            {
                sentenceText.color = new Color(sentenceText.color.r, sentenceText.color.g, sentenceText.color.b, 1f);
            }
            else if (timer < sentenceTime * 4f / 5f)
            {
                float a = sentenceText.color.a - 5f * Time.deltaTime / sentenceTime;
                if (a < 0f)
                {
                    a = 0f;
                }
                sentenceText.color = new Color(sentenceText.color.r, sentenceText.color.g, sentenceText.color.b, a);
            }
            else if (timer < sentenceTime)
            {
                float a = sentenceImage.color.a - 5f * Time.deltaTime / sentenceTime;
                if (a < 0f)
                {
                    a = 0f;
                }
                sentenceImage.color = new Color(sentenceImage.color.r, sentenceImage.color.g, sentenceImage.color.b, a);
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("KeepPlayer"))
                {
                    GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>().stopPlane = false;
                }

                foreach (GameObject obj in activeWhenSentenceOver)
                {
                    obj.SetActive(true);
                }
                timer = 0f;
                sentenceOver = true;

                if (marchMode)
                {
                    StartCoroutine(MarchMode());
                }
            }


        }


    }

    IEnumerator MarchMode()
    {
        yield return new WaitForEndOfFrame();
        int savePoint = 0;
        while (player.transform.position.z < destinationDistance)
        {
            yield return new WaitForEndOfFrame();
            if (player != null)
                if (player.GetComponent<RealFlight>())
                {
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForSeconds(1f / createPerSec);
                    GameObject temp_obstacle = null;
                    if (player != null)
                        temp_obstacle = Instantiate<GameObject>(obstaclePres[Random.Range(0, obstaclePres.Count)], player.transform.position + creatDistance * Vector3.forward + createRange * Random.insideUnitSphere, new Quaternion(), GameObject.Find("NotStable").transform);
                    obstacles.Add(temp_obstacle);

                    yield return new WaitForSeconds(1f / createPerSec);
                    if (player != null)
                        temp_obstacle = Instantiate<GameObject>(obstaclePres[Random.Range(0, obstaclePres.Count)], player.transform.position + creatDistance * Random.onUnitSphere, new Quaternion(), GameObject.Find("NotStable").transform);
                    obstacles.Add(temp_obstacle);

                    for (int i = obstacles.Count - 1; i >= 0; i--)
                    {
                        if (player.transform.position.z - obstacles[i].transform.position.z > 20f)
                        {
                            if (GameObject.FindGameObjectWithTag("KeepPlayer"))
                            {
                                if (Vector3.Distance(GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>().savePoint, obstacles[i].transform.position) < createRange)
                                {
                                    Destroy(obstacles[i]);
                                    obstacles.Remove(obstacles[i]);
                                    i--;
                                }
                            }
                        }
                    }

                    if (player.transform.position.z > ((float)(savePoint + 1) / (float)marchModeAuClips.Count) * destinationDistance)
                    {
                        auSource.PlayOneShot(marchModeAuClips[savePoint]);
                        savePoint++;
                        if (GameObject.FindGameObjectWithTag("KeepPlayer"))
                        {
                            GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>().savePoint = player.transform.position;
                        }
                    }

                    marchModeScore.text = (100 * (int)player.transform.position.z / destinationDistance).ToString() + "%";
                }

        }
        marchModeScore.text = "100%";
        if (player != null)
            destination.transform.position = player.transform.position + 300f * Vector3.forward;
        destination.SetActive(true);
    }

}


