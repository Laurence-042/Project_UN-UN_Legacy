using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class RealFlight : MonoBehaviour
{
    [SerializeField] Center center;
    [SerializeField] private GameObject mainCamera;//主摄像机
    [SerializeField] Vector3 camOffset = new Vector3(0, 4, -14);
    [SerializeField] GameObject indicator;
    private GameObject hud;//中央圆圈
    private GameObject hor;//水平仪
    private GameObject ver;//抬升指示器
    [SerializeField] private Text speedometer;//速度计

    [SerializeField] private ParticleSystem[] TrailFlame;
    [SerializeField] private ParticleSystem leftTrailFlame;
    [SerializeField] private ParticleSystem rightTrailFlame;

    [SerializeField] private AudioSource ausource;//飞行音效播放者
    [SerializeField] private AudioClip flying;//飞行音效

    [SerializeField] private DockGuide dockGuide;//挂载于自身的脚本
    [SerializeField] NewHp hp;

    public float speed = 0f;//当前速度
    public float senSpeed = 0.1f;//加速敏感度
    public float minSpeed = -5f;//最低速度
    public float maxSpeed = 20f;//最高速度
    [SerializeField] float rotSen = 60f;
    [SerializeField] Toggle easyControl;
    [SerializeField] float maxEasyAngle = 80f;
    bool stabZ = false;
    public bool marchMode = false;

    private void Start()
    {
        if (center == null)
        {
            center = GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>();
        }
        center.isPlane = true;

        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            //camOffset = mainCamera.transform.position - transform.position;
        }

        if (dockGuide == null)
        {
            dockGuide = GetComponent<DockGuide>();
        }
        if (ausource == null)
        {
            Debug.Log(GameObject.Find("PlayerModel").transform.position);
            Debug.Log(GameObject.Find("PlayerModel"));
            ausource = GameObject.Find("PlayerModel").GetComponent<AudioSource>();
        }
        if (hp == null)
        {
            hp = GetComponent<NewHp>();
        }

        if (speedometer == null)
        {
            speedometer = GameObject.FindGameObjectWithTag("Speedometer").GetComponent<Text>();
        }

        if (indicator == null)
        {
            indicator = GameObject.FindGameObjectWithTag("Indicator");
        }
        hud = GameObject.Find(indicator.name + "/hud");
        hor = GameObject.Find(indicator.name + "/hor");
        ver = GameObject.Find(indicator.name + "/ver");
        if (easyControl == null)
        {
            easyControl = GameObject.Find("EasyControlButton").GetComponent<Toggle>();
        }

        easyControl.onValueChanged.AddListener((bool value) => StabZ(value));

    }

    void FixedUpdate()
    {
        if (stabZ)
        {
            if (transform.localEulerAngles.z < 180)
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), 20 * Time.deltaTime);
            else
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 360), 20 * Time.deltaTime);
            if (Mathf.Abs(transform.localEulerAngles.z) < 0.1 || Mathf.Abs(360 - transform.localEulerAngles.z) < 0.1)
                stabZ = false;
        }
        if (!hp.finalBurst && !center.stopPlane)
        {
            if (hp.hp <= 0)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                return;
            }
            Flight();
            EffectSet();
            CameraSet();
        }
        else
        {
            if (ausource != null)
                ausource.Stop();
        }
    }

    private void LateUpdate()
    {
        if (hp.hp <= 0)
        {
            return;
        }
        //CameraSet();
    }

    void Flight()
    {
        float x = CrossPlatformInputManager.GetAxis("Horizontal");
        float y = CrossPlatformInputManager.GetAxis("Vertical");
        float speedTo = CrossPlatformInputManager.GetAxis("Speed");

        if (speed > 0)
        {
            speed = Mathf.Lerp(speed, speedTo * maxSpeed, 0.1f);
        }
        else
        {
            speed = Mathf.Lerp(speed, -speedTo * minSpeed, 0.1f);
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!marchMode)
        {
            if (!easyControl.isOn)
            {
                if (x != 0 || y != 0)
                {
                    /*
                    transform.Rotate(Vector3.forward, -x * rotSen);
                    transform.Rotate(Vector3.right, y * rotSen);
                    */

                    if (Mathf.Abs(x) > 0.4f)
                    {
                        transform.Rotate(Vector3.forward, -x * rotSen * Time.deltaTime);
                    }
                    if (Mathf.Abs(y) > 0.4f)
                    {
                        transform.Rotate(Vector3.right, y * rotSen * Time.deltaTime);
                    }
                }
            }
            else
            {

                transform.Rotate(Vector3.up, x * rotSen * Time.deltaTime, Space.World);

                Vector3 temp_rot = transform.localEulerAngles + (-y * rotSen * Time.deltaTime) * Vector3.right;
                if (temp_rot.x <= maxEasyAngle || temp_rot.x >= 360f - maxEasyAngle)
                {
                    transform.localEulerAngles = temp_rot;
                }

                if (x != 0 || y != 0)
                {
                    if (x > 0)
                    {
                        if (leftTrailFlame != null)
                        {
                            leftTrailFlame.Play();
                            rightTrailFlame.Stop();
                        }
                    }
                    else if (x < 0)
                    {
                        if (leftTrailFlame != null)
                        {
                            leftTrailFlame.Stop();
                            rightTrailFlame.Play();
                        }
                    }
                }
                else
                {
                    if (leftTrailFlame != null)
                    {
                        leftTrailFlame.Stop();
                        rightTrailFlame.Stop();
                    }
                }
                /*
                transform.Rotate(Vector3.up, x * rotSen);
                transform.Rotate(Vector3.right, -y * rotSen);
                */
            }
        }
        else
        {
            if (x != 0 || y != 0)
            {
                transform.Translate(Vector3.up * y * speed * rotSen * Time.deltaTime / 50f);
                transform.Translate(Vector3.right * x * speed * rotSen * Time.deltaTime / 50f);
                if (x > 0)
                {
                    if (leftTrailFlame != null)
                    {
                        leftTrailFlame.Play();
                        rightTrailFlame.Stop();
                    }
                    
                }else if (x < 0)
                {
                    if (leftTrailFlame != null)
                    {
                        leftTrailFlame.Stop();
                        rightTrailFlame.Play();
                    }
                }
            }
            else
            {
                if (leftTrailFlame != null)
                {
                    leftTrailFlame.Stop();
                    rightTrailFlame.Stop();
                }
            }
        }
    }


    void CameraSet()
    {

        mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, 60, 3 * Time.deltaTime);

        //distanceAway = reDistanceAway + (float)System.Math.Atan(speed);\
        float distanceAway = camOffset.z - (float)System.Math.Atan(speed);

        //mainCamera.transform.position = transform.position+ camOffset - Vector3.forward * distanceAway + (distanceY + reDistanceY) * Vector3.up;
        Vector3 camPos = Vector3.zero;
        camPos = transform.forward * distanceAway*(marchMode?0.4f:1f) + Vector3.up * camOffset.y * (marchMode ? 0.4f : 1f) + transform.right * camOffset.x;

        //mainCamera.transform.position = transform.position + camPos;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, transform.position + camPos, 0.1f);
        mainCamera.transform.LookAt(transform.position + transform.forward * 100f);



    }


    void EffectSet()
    {
        Vector3 rotation = transform.localEulerAngles;
        //水平仪与转向指示保存当前物体角度
        hor.transform.localEulerAngles = new Vector3(0, 0, -rotation.z);
        if (rotation.x > 90)
            rotation.x -= 360;
        ver.transform.localPosition = new Vector3(0, rotation.x, 0);


        //速度计
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        //speedometer.text = "Speed: " + speed;
        if (dockGuide.docking)
        {
            speed = 0;
            //speedometer.text = "Speed: " + dockGuide.speed;
            speedometer.text = System.Math.Round((double)dockGuide.speed, 1).ToString();
        }
        else
        {
            //speedometer.text = "Speed: " + speed;
            speedometer.text = System.Math.Round((double)speed, 1).ToString();
        }

        //尾焰
        if (speed > 0.05f)
        {
            for (int i = 0; i < TrailFlame.Length; i++)
            {
                if (!TrailFlame[i].isPlaying)
                {
                    TrailFlame[i].Play();
                }
            }
        }
        else
        {
            for (int i = 0; i < TrailFlame.Length; i++)
            {
                if (TrailFlame[i].isPlaying)
                {
                    TrailFlame[i].Stop();
                }
            }
        }


        //飞行音效
        if (!dockGuide.docking)
        {
            if (speed > 0.05f && !ausource.isPlaying)
            {
                //ausource.Play ();
                ausource.PlayOneShot(flying);
            }
            else if (speed <= 0.05F && ausource.isPlaying)
            {
                ausource.Stop();
            }

            ausource.volume = speed / maxSpeed;


        }
        else
        {
            if (dockGuide.speed > 0.05f && !ausource.isPlaying)
            {
                //ausource.Play ();
                ausource.PlayOneShot(flying);
            }
            else if (dockGuide.speed <= 0.05F && ausource.isPlaying)
            {
                ausource.Stop();
            }

            ausource.volume = dockGuide.speed / 20;
        }

    }

    void StabZ(bool value)
    {
        stabZ = value;
    }

}

