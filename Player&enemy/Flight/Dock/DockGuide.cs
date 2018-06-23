using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockGuide : MonoBehaviour
{
    public float speed = 1f;//对接时飞行速度
    [SerializeField] private float radius = 20f;//寻找对接物体范围
    [SerializeField] private GameObject dockButton;
    [SerializeField] private GameObject tractorBeamPre;

    public bool docking = false;

    public GameObject target = null;
    private int wpsIndex = 0;

    private GameObject tractorBeam;


    // Use this for initialization
    void Start()
    {
        if (dockButton != null)
        {
            dockButton.SetActive(false);
        }
        StartCoroutine(scanDock());
    }

    // Update is called once per frame
    void Update()
    {
        if (!docking)
        {
            if (tag == "Player")
            {
                target = null;
                if (tractorBeam != null)
                {
                    Destroy(tractorBeam);
                    tractorBeam = null;
                }
            }
            wpsIndex = 0;
            if (transform.parent != null)
            {
                if (transform.parent.gameObject.tag != "KeepPlayer" && tag == "Player")
                {
                    transform.parent = GameObject.Find("KeepPlayer").transform;
                }
            }
        }
        if (docking)
        {
            if (!target)//docking第一次为true时执行
            {
                Collider[] targetsColliders = Physics.OverlapSphere(transform.position, radius);//获取范围内可能可对接物体
                float temp_distance = radius;
                for (int i = 0; i < targetsColliders.Length; i++)
                {
                    if (targetsColliders[i].gameObject.tag == "Dock" || targetsColliders[i].gameObject.tag == "Part")//对接物体的tag应设为"Dock"
                    {
                        if (Vector3.Distance(transform.position, targetsColliders[i].transform.position) < temp_distance)//获取最近Dock
                        {
                            target = targetsColliders[i].gameObject;
                            temp_distance = Vector3.Distance(transform.position, targetsColliders[i].transform.position);
                        }
                    }
                }
                if (target)
                {
                    //清除惯性导致的速度
                    if (this.gameObject.GetComponent<Rigidbody>() != null)
                        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                }
            }

            if (target)
            {//如果有可对接物体
                if (gameObject.tag == "Player" && tractorBeam == null)
                {
                    tractorBeam = Instantiate(tractorBeamPre, gameObject.transform);
                }
                if (tractorBeam != null)
                {
                    tractorBeam.GetComponent<Transform>().Find("LightningEnd").gameObject.transform.position = target.transform.position;
                }
                GameObject thisTarget = target.GetComponent<Dock>().wps[wpsIndex];//当前驶向的路径点
                if (thisTarget.GetComponent<WayPoint>() == null)
                {
                    Debug.Log("好吧，你忘记给路径点加脚本了，现在看下到底漏了哪个");
                    Debug.Log("null:    " + thisTarget);
                    Debug.Log("index:   " + wpsIndex);
                    Debug.Log("target:   " + target);
                }

                if (!thisTarget.GetComponent<WayPoint>().turn)//若WayPoint的turn为false，平移至此路径点
                {

                    if (gameObject.tag == "Player")
                    {
                        gameObject.GetComponent<Flight>().stabZ = true;
                        if (target.GetComponent<DockGuide>() != null)
                        {
                            //transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, target.transform.eulerAngles,3*Time.deltaTime);
                            //transform.eulerAngles = target.transform.eulerAngles;
                            //transform.localEulerAngles = Vector3.Slerp (transform.localEulerAngles, target.transform.localEulerAngles,);
                            //DIY_Movement.SoftLookAt(this.gameObject,thisTarget.transform.position,30);

                            float angle = Vector3.Angle(transform.forward, target.transform.forward);
                            float needTime = angle / 30;
                            float v = 1;
                            if (needTime > Mathf.Epsilon)
                                v = Time.deltaTime / needTime;
                            Quaternion t = Quaternion.Euler(0, 0, 0);
                            transform.localRotation = Quaternion.Slerp(transform.localRotation, t, v);

                        }

                        transform.Translate(Vector3.Normalize(thisTarget.transform.position - transform.position) * speed * Time.deltaTime);
                        //Debug.Log("docking to" + thisTarget);
                    }
                    else
                    {
                        Vector3 offset = (thisTarget.transform.position - transform.position).normalized;
                        float angle = Vector3.Angle(transform.forward, offset);
                        if (angle > Mathf.Epsilon)
                            DIY_Movement.SoftLookAt(this.gameObject, thisTarget.transform.position, 30);
                        else
                            transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    }
                }
                else
                {//之前的其他路径点则先转向，后直行
                    Vector3 offset = (thisTarget.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, offset);
                    if (angle > Mathf.Epsilon)
                    {
                        if (thisTarget.transform.position - transform.position != Vector3.zero)
                            DIY_Movement.SoftLookAt(this.gameObject, thisTarget.transform.position, 30);
                    }
                    else
                    {
                        transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    }
                }



                if (Vector3.Distance(transform.position, thisTarget.transform.position) < 0.5f)
                {//到达路径点后执行
                    if (wpsIndex < target.GetComponent<Dock>().wps.Length - 1)
                    {
                        wpsIndex++;
                    }
                    else
                    {
                        transform.position = thisTarget.transform.position;
                        if (target.tag == "Part")
                        {
                            TestPart part = target.GetComponentInChildren<TestPart>();
                            part.gameObject.transform.parent = transform;
                            part.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                            part.gameObject.transform.localPosition = Vector3.zero;
                            Destroy(target);
                            docking = false;
                            target = null;
                            wpsIndex = 0;
                            thisTarget = null;
                            if (part.tag == "Laser")
                            {
                                this.GetComponent<Fire>().lasterGun = true;
                            }
                        }
                        else
                        {
                            transform.parent = target.transform;
                            if (target.GetComponent<DockGuide>() != null)
                            {
                                target.GetComponent<DockGuide>().docking = true;
                            }
                            if (target.gameObject.name == "Portal")
                            {
                                transform.parent = GameObject.Find("KeepPlayer").transform;
                                target = null;
                                wpsIndex = 0;
                                docking = false;

                            }
                        }
                        //docking = false;
                        //target = null;
                        //Debug.Log ("docked");
                    }
                }
            }
            else
            {
                docking = false;
                Debug.Log("范围内无目标");
            }
        }
    }


    IEnumerator scanDock()
    {
        int i = 0;
        while (true)
        {

            yield return new WaitForSeconds(1f);
            Collider[] targets = Physics.OverlapSphere(transform.position, radius);
            for (i = 0; i < targets.Length; i++)
            {
                if (targets[i].gameObject.tag == "Dock" || targets[i].gameObject.tag == "Part")
                {
                    if (targets[i].gameObject.GetComponent<Dock>().radius == 0 || targets[i].gameObject.GetComponent<Dock>().radius > Vector3.Distance(transform.position, targets[i].transform.position))
                        break;
                }
            }
            if (dockButton != null)
            {
                if (i != targets.Length)
                {
                    dockButton.SetActive(true);
                }
                else
                {
                    dockButton.SetActive(false);
                }
            }
        }
    }

}
