using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    public GameObject player;
    [SerializeField] GameObject playerPre;
    [SerializeField] ParticleSystem playerResetEffect;
    [SerializeField] Camera mainCam;
    [SerializeField] AudioCenter auCenter;
    public Vector3 savePoint=Vector3.zero;
    public bool isPlane = true;
    public bool stealSuccessfully = true;
    public bool stopPlane = false;
    private bool lastIsPlane;
    private bool edge = false;
    [SerializeField] GameObject[] activeWhileIsPlane;
    [SerializeField] List<GameObject> temps;
    [SerializeField] List<float> tempsLeftTime;

    private void Start()
    {
        playerPre = Resources.Load<GameObject>("Prefabs/Player/Player");
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = Camera.main;
        auCenter = GetComponentInChildren<AudioCenter>();
        lastIsPlane = isPlane;

    }
    private void Update()
    {
        AutoSwitch();
        if (player == null || player.tag != "Player")
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }
        if (stealSuccessfully&& (player == null||GameObject.FindGameObjectsWithTag("Player").Length==0))
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }
        if (!stealSuccessfully)
        {
            StartCoroutine(ResetPlayer());
            stealSuccessfully = true;
            Debug.Log("reset begin");
        }
        if(isPlane!= lastIsPlane)
        {
            edge = true;
        }
    }

    public IEnumerator ResetPlayer()
    {
        Debug.Log("Enter reset");
        player = Instantiate(playerPre, savePoint, new Quaternion(), GameObject.FindGameObjectWithTag("KeepPlayer").transform);
        auCenter.GetComponent<AudioSource>().PlayOneShot(auCenter.stealFailed);
        yield return new WaitForEndOfFrame();
        /*
        while (mainCam.fieldOfView < 115f)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 120f, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        */
        stealSuccessfully = true;
        Debug.Log("reset");
        
        playerResetEffect.transform.position = player.transform.position;
        playerResetEffect.Play();
    }

    void AutoSwitch()
    {
        if (edge)
        {
            for (int i = 0; i < activeWhileIsPlane.Length; i++)
            {
                activeWhileIsPlane[i].transform.localScale = isPlane ? Vector3.one : Vector3.zero;
            }
            edge = false;
            lastIsPlane = isPlane;
        }
    }


}
