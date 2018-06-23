using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepOne : MonoBehaviour
{
    [SerializeField] float scanTime = 3f;
    [SerializeField] private GameObject[] pre;
    [SerializeField] private Vector3[] pos;
    [SerializeField] private Vector3[] rot;
    [SerializeField] GameObject[] inst;

    void Start()
    {
        
        StartCoroutine(Flush());
        
    }

    IEnumerator Flush()
    {
        while (true)
        {
            yield return new WaitForSeconds(scanTime);
            for (int i = 0; i < inst.Length; i++)
            {
                if (inst[i] == null)
                {
                    Debug.Log("creat "+ pre[i].name);
                    inst[i] = Instantiate<GameObject>(pre[i], pos[i], Quaternion.Euler(rot[i]));
                }
                else
                {
                    Debug.Log("not null " + i);
                }
            }
        }
    }

}
