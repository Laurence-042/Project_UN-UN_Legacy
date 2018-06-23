using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class StealEnemy : MonoBehaviour
{
    [SerializeField] Center center;
    [SerializeField] private GameObject mainCamera;//主摄像机
    [SerializeField] Vector3 camOffset = new Vector3(0, 4, -14);
    [SerializeField] float camRotSen = 10f;

    [SerializeField] List<GameObject> enemies;
    [SerializeField] Transform enemy;
    [SerializeField] GameObject playerPre;
    [SerializeField] string playerPreName;

    [SerializeField] float speed = 40f;
    [SerializeField] bool qte = false;

    [SerializeField] GameObject ausourcePre;
    [SerializeField] AudioClip successSound;
    [SerializeField] float auPlayTime = 3f;

    [SerializeField] bool stealFailed = false;

    [SerializeField] float scanDelay = 3f;
    [SerializeField] float scanDistance = 1000f;

    // Use this for initialization
    void Start()
    {
        if (center == null)
        {
            center = GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>();
        }
        center.isPlane = false;

        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            //camOffset = mainCamera.transform.position - transform.position;
        }
        if (ausourcePre == null)
        {
            ausourcePre = Resources.Load<GameObject>("Prefabs/Effect/ausourcePre");
        }

        StartCoroutine(ScanEnemy(scanDelay, scanDistance));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
                i--;
            }
        }

        if (!stealFailed)
        {
            CameraSet();
        }
        if (!qte)
        {
            if (Input.touchCount>0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    int i = 0;
                    for (i = 0; i < enemies.Count; i++)
                    {
                        if (Vector2.Distance(Input.GetTouch(0).position, Camera.main.WorldToScreenPoint(enemies[i].transform.position)) < 60f)
                        {
                            enemy = enemies[i].transform;
                            break;
                        }
                    }
                    if (i == enemies.Count)
                    {
                        enemy = null;
                    }
                }
                
            }
            if (enemy != null)
            {
                transform.Translate(Vector3.Normalize(enemy.position - transform.position) * Time.deltaTime * speed);
                if (Vector3.Distance(transform.position, enemy.position) < Time.deltaTime * speed)
                {
                    qte = true;
                    transform.parent = enemy;
                    transform.position = enemy.position;
                    center.gameObject.GetComponentInChildren<QTE>().ActiveQTE();
                }
            }
        }
        else
        {
            if (center.gameObject.GetComponentInChildren<QTE>().qteSuccess == -1)
            {
                if (!stealFailed)
                {
                    Debug.Log("steal fail");
                    center.stealSuccessfully = false;
                    Destroy(this.gameObject, 3f);
                    stealFailed = true;
                }


            }
            if (center.gameObject.GetComponentInChildren<QTE>().qteSuccess == 1)
            {
                Debug.Log("steal success");
                StealSuccess();
            }

        }

    }

    void CameraSet()
    {
        //滑动变更摄像机角度
        float x = CrossPlatformInputManager.GetAxis("Horizontal");
        float y = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 camRot = new Vector3(y, x, 0);

        camRot = Vector3.Normalize(camRot);

        camOffset = Quaternion.Euler(camRot * camRotSen * Time.deltaTime) * camOffset;


        //正常设置
        if (tag == "Player")
        {
            if (enemy != null && !qte)
            {
                mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, 120, 3 * Time.deltaTime);
            }
            else if (qte)
            {
                mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, 30, 3 * Time.deltaTime);
            }
            else
            {
                mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, 60, 3 * Time.deltaTime);
            }

            //mainCamera.transform.position = transform.position+ camOffset - Vector3.forward * distanceAway + (distanceY + reDistanceY) * Vector3.up;

            mainCamera.transform.position = transform.position + camOffset;
            mainCamera.transform.LookAt(transform.position);
        }
    }

    public void StealSuccess()
    {
        Destroy(enemy.gameObject);
        if (enemy.GetComponent<EnemyFlight>().enemyType != "")
        {
            playerPreName = enemy.GetComponent<EnemyFlight>().enemyType;
            playerPre = Resources.Load<GameObject>("Prefabs/Player/" + playerPreName);
        }
        else
        {
            Debug.Log("加载错误");
        }
        GameObject player = Instantiate(playerPre, enemy.position, enemy.rotation);
        player.transform.parent = GameObject.FindGameObjectWithTag("KeepPlayer").transform;
        this.gameObject.transform.localScale = Vector3.zero;

        StartCoroutine(SuccessEffect());
        Destroy(this.gameObject, auPlayTime + 1f);


    }

    IEnumerator SuccessEffect()
    {

        GameObject tempAudio = Instantiate(ausourcePre);
        tempAudio.GetComponent<AudioSource>().clip = successSound;
        tempAudio.GetComponent<AudioSource>().Play();
        Destroy(tempAudio, auPlayTime);
        yield return 0;
    }

    IEnumerator ScanEnemy(float scanDelay, float scanDistance)
    {
        while (true)
        {
            enemies.Clear();
            GameObject[] temp_enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp_enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < scanDistance)
                {
                    enemies.Add(enemy);
                }
            }
            yield return new WaitForSeconds(scanDelay);

        }
    }

}
