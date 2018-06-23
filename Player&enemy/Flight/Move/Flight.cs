using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;//主摄像机
    [SerializeField] Vector3 camOffset = Vector3.zero;
    [SerializeField] private GameObject hud;//中央圆圈
    [SerializeField] private GameObject hor;//水平仪
    [SerializeField] private GameObject ver;//抬升指示器
    [SerializeField] private UILabel speedometer;//速度计
    [SerializeField] private GameObject TrailFlame;//速度计

    [SerializeField] private AudioSource ausource;//飞行音效播放者
    [SerializeField] private AudioClip flying;//飞行音效

    [SerializeField] private DockGuide dockGuide;//挂载于自身的脚本

    public float speed = 0f;//当前速度
    private float posY = 0f;//摇杆Y轴位置（-1~1）
    private float posX = 0f;//摇杆X轴位置（-1~1）
                            //private float rotX=0;
    public float senSpeed = 0.1f;//加速敏感度
                                 //public float senRot=1f;

    public float reDistanceAway = 12.0f;//静止时摄像机距飞机的水平距离
    public float reDistanceY;//静止时摄像机距飞机的垂直距离

    private float distanceAway = 12.0f;//当前摄像机距飞机的水平距离
    private float distanceY;//当前摄像机距飞机的垂直距离

    private float temp_posY;//临时存储摇杆Y轴位置（-1~1），用于复位

    public bool stabZ = false;//是否重置Z周旋转 并 清除rigibody的物理模拟导致的速度
    public bool stabXYZ = false;//未使用

    public float minSpeed = -0.5f;//最低速度
    public float maxSpeed = 10f;//最高速度

    public bool speedUp = false;//是否处于加速状态

    private void Start()
    {
        distanceY = reDistanceY;
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
    }

    void On_JoystickMove(MovingJoystick move)
    {

        if (move.joystickName == "RotJoystick1")
        {
            posX = move.joystickAxis.x;
            posY = move.joystickAxis.y;

            //roting =true;

            if (posX != 0 || posY != 0)
            {
                distanceY = Mathf.Lerp(0, reDistanceY - posY * reDistanceY, 2);
                temp_posY = posY;

                //Debug.Log (transform.rotation);
                if (hud != null)
                {
                    hud.transform.rotation = Quaternion.Euler(new Vector3(0, 0, posX * 30));
                    hud.transform.localPosition = new Vector3(-posX * 30, posY * 30, hud.transform.localPosition.z);
                }


            }
        }
        if (move.joystickName == "SpeedJoystick")
        {
            speed += move.joystickAxis.y * senSpeed;
            if (speed < minSpeed)
            {
                speed = minSpeed;
            }
            else if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            //transform.Translate (Vector3.forward * speed);
        }
    }
    void On_JoystickMoveEnd(MovingJoystick move)
    {
        //if (move.joystickName == "RotJoystick") {
        //	distanceY = Mathf.Lerp (1 - temp_posY, 1, 2);
        //}
        //Debug.Log("end");

    }


    void FixedUpdate()
    {
        

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Vector3 rotation = transform.localEulerAngles;
        if (stabZ)
        {
            if (transform.localEulerAngles.z < 180)
                transform.localEulerAngles = Vector3.Lerp(rotation, new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0), 20 * Time.deltaTime);
            else
                transform.localEulerAngles = Vector3.Lerp(rotation, new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 360), 20 * Time.deltaTime);
            if (Mathf.Abs(transform.localEulerAngles.z) < 0.1 || Mathf.Abs(360 - transform.localEulerAngles.z) < 0.1)
                stabZ = false;
        }

        if (stabXYZ)
        {
            transform.transform.LookAt(Vector3.forward);
            stabXYZ = false;
        }

        EffectSet();

    }

    private void LateUpdate()
    {
        CameraSet();
    }


    void CameraSet()
    {
        if (speedUp)
        {
            mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, 120, 3 * Time.deltaTime);
        }
        else
        {
            mainCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(mainCamera.GetComponent<Camera>().fieldOfView, 60, 3 * Time.deltaTime);
        }
        //distanceAway = reDistanceAway + (float)System.Math.Atan(speed);\
        distanceAway = camOffset.z - (float)System.Math.Atan(speed);

        //mainCamera.transform.position = transform.position+ camOffset - Vector3.forward * distanceAway + (distanceY + reDistanceY) * Vector3.up;
        Vector3 camPos = Vector3.zero;
        camPos = this.transform.forward * distanceAway + this.transform.up * (camOffset.y+ distanceY) + this.transform.right * camOffset.x;
        mainCamera.transform.position = transform.position + camPos;
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
            if (!TrailFlame.activeSelf)
                TrailFlame.SetActive(true);
        }
        else
        {
            if (TrailFlame.activeSelf)
                TrailFlame.SetActive(false);
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

            if (speedUp)
            {
                ausource.volume = 1;
            }
            else
            {
                ausource.volume = speed / maxSpeed / 2;
            }

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
}
