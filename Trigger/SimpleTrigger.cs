using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrigger : MonoBehaviour
{
    [SerializeField] private bool runIfActive;
    [SerializeField] private bool activeGameObject = true;
    [SerializeField] private GameObject[] toActive;
    [SerializeField] private bool destroyGameObject = true;
    [SerializeField] private string[] toDestroyTag;
    [SerializeField] private bool switchLecel = false;
    [SerializeField] private GameObject LevelSwitcher;
    [SerializeField] private bool ForFungus = false;
    [SerializeField] private GameObject flowChart0;
    [SerializeField] private GameObject flowChart1;
    [SerializeField] private string[] validEnterTag=new string[] { "Player", "MoveBlock" };



    // Use this for initialization
    void Start()
    {
        if (runIfActive)
        {

            DIY_Trigger();


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        for (int i = 0; i < validEnterTag.Length; i++)
        {
            if (other.gameObject.tag == validEnterTag[i])
            {
                DIY_Trigger();
                break;
            }
        } 
        
    }

    void DIY_Trigger()
    {
        if ((activeGameObject && switchLecel) || (!activeGameObject && !switchLecel))
        {
            Debug.Log("触发器参数错误");
        }
        else
        {
            if (activeGameObject)
            {
                for (int i=0; i < toActive.Length; i++)
                {
                    toActive[i].SetActive(true);
                }
            }

            if (switchLecel)
            {
                LevelSwitcher.GetComponent<SwitchLevel>().Switch();
            }
        }
    }

    void Load()
    {
        if (PlayerPrefs.GetInt("enemy") >= 2)
        {
            flowChart0.SetActive(true);
        }
        else
        {
            flowChart1.SetActive(true);
        }
    }

    public void Destroy()
    {
        if (destroyGameObject)
        {
            for (int i = 0; i < toDestroyTag.Length; i++)
            {
                GameObject[] toDestroy = GameObject.FindGameObjectsWithTag(toDestroyTag[i]);
                if (toDestroy.Length > 0)
                {
                    for (int j = 0; j < toDestroy.Length; j++)
                    {
                        Destroy(toDestroy[j]);
                        Debug.Log(toDestroy[j].name + "is destroyed");
                    }
                }
            }
        }
        else
        {
            Debug.Log("error, 此脚本不具有摧毁物体权限");
        }
        
    }

}
