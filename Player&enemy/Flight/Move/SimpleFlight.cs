using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlight : MonoBehaviour
{

    [SerializeField] private GameObject camera;//主摄像机
    [SerializeField] private GameObject hud;//中央圆圈
    [SerializeField] private GameObject hor;//水平仪
    [SerializeField] private GameObject ver;//抬升指示器
    [SerializeField] private UILabel speedometer;//速度计

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

    public bool stabZ = false;//是否重置Z周旋转 并 清除rigibody的物理模拟导致的速度

    public float minSpeed = -0.5f;//最低速度
    public float maxSpeed = 10f;//最高速度


    private void Start()
    {
        distanceY = reDistanceY;
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

    void Update()
    {
        CameraSet();

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


        EffectSet();

    }

    void FixedUpdate()
    {

        //Debug.Log (transform.rotation);
    }

    void CameraSet()
    {
        if (camera != null)
        {
            camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 60, 3 * Time.deltaTime);

            distanceAway = reDistanceAway + (float)System.Math.Atan(speed);
            camera.transform.localPosition = -Vector3.forward * distanceAway + (distanceY + reDistanceY) * Vector3.up;
        }
    }


    void EffectSet()
    {
        Vector3 rotation = transform.localEulerAngles;

        if (hor != null)
        {
            //水平仪与转向指示保存当前物体角度
            hor.transform.localEulerAngles = new Vector3(0, 0, -rotation.z);
        }
        if (ver != null)
        {
            if (rotation.x > 90)
            {
                rotation.x -= 360;
            }
            ver.transform.localPosition = new Vector3(0, rotation.x, 0);
        }



        //速度计
        if (speedometer != null)
        {
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            //speedometer.text = "Speed: " + speed;
            speedometer.text = System.Math.Round((double)speed, 1).ToString();
        }


    }
}


