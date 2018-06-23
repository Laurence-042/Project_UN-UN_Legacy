using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    [SerializeField] GameObject qteCanvas;
    [SerializeField] GameObject[] qteButtons;
    [SerializeField] GameObject[] qteCircles;
    [SerializeField] float circleOffsetScale = 1.2f;
    [SerializeField] int[] randomIndex;
    [SerializeField] int nowIndex = 0;
    public bool active = false;
    public int qteSuccess = 0;

    [SerializeField] float timer = 0f;
    [SerializeField] float[] timeWindow = { 0 };
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < randomIndex.Length; i++)
        {
            randomIndex[i] = i;
        }
        HideQTE();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < qteCircles.Length; i++)
            {
                if ((1 + timeWindow[randomIndex[i]] - timer) >= 0)
                {
                    if (qteCircles[i].activeInHierarchy)
                        qteCircles[i].GetComponent<RectTransform>().localScale = circleOffsetScale * (1 + timeWindow[randomIndex[i]] - timer) * qteButtons[i].GetComponent<RectTransform>().localScale;
                }
                else if (nowIndex == randomIndex[i])
                {
                    Debug.Log("Run out of time");
                    HideQTE();
                    qteSuccess = -1;
                }


            }
        }
    }

    public void HideQTE()
    {
        qteCanvas.SetActive(false);
        active = false;
        for (int i = 0; i < qteCircles.Length; i++)
        {
            qteCircles[i].SetActive(false);
        }
        timer = 0;
    }

    public void ActiveQTE()
    {
        RandIndex();
        nowIndex = 0;
        qteCanvas.SetActive(true);
        for (int i = 0; i < randomIndex.Length; i++)
        {
            qteButtons[i].SetActive(true);
            qteButtons[i].GetComponentInChildren<QTEButton>().index = randomIndex[i];
            qteButtons[i].GetComponentInChildren<Text>().text = randomIndex[i].ToString();
        }
        for (int i = 0; i < qteCircles.Length; i++)
        {
            qteCircles[i].SetActive(true);
            qteCircles[randomIndex[i]].GetComponent<RectTransform>().localPosition = qteButtons[randomIndex[i]].GetComponent<RectTransform>().localPosition;
        }
        active = true;
        qteSuccess = 0;
    }

    private void RandIndex()
    {
        for (int i = 0; i < randomIndex.Length; i++)
        {
            int rand = Random.Range(i, randomIndex.Length - 1);
            int temp = randomIndex[i];
            randomIndex[i] = randomIndex[rand];
            randomIndex[rand] = temp;
        }
    }

    public void ClickButton(int index,int buttonIndex)
    {
        if (active)
        {
            Debug.Log(index + " clicked");
            if (index == nowIndex)
            {
                if (Mathf.Abs(timer - timeWindow[index]) < 0.8f)
                {
                    nowIndex++;
                    qteButtons[buttonIndex].SetActive(false);
                    qteCircles[buttonIndex].SetActive(false);
                    Debug.Log(index + " success");
                    if (nowIndex == randomIndex.Length)
                    {
                        qteSuccess = 1;
                        HideQTE();
                        Debug.Log("qte success");
                    }
                }
                else
                {
                    Debug.Log("Miss");
                    Debug.Log("Abs " + timer + " - " + timeWindow[index] + " <0.8f");
                    HideQTE();
                    qteSuccess = -1;
                    Debug.Log(nowIndex + " " + index);
                }
            }
            else
            {
                Debug.Log("Not This");
                HideQTE();
                qteSuccess = -1;
                Debug.Log(nowIndex + " " + index);
            }
        }
    }


}
