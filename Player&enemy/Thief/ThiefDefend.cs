using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class ThiefDefend : MonoBehaviour {

    // 设置枪击带来的伤害值
    public int gunDamage = 1;

    // 设置两次枪击的间隔时间
    public float fireRate = 0.25f;

    // 设置玩家可以射击的Unity单位
    public float weaponRange = 50f;

    // GunEnd游戏对象
    public Transform gunEnd;

    // 设置射击轨迹显示的时间
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    // 枪击音效
    private AudioSource gunAudio;

    // 射击轨迹
    private LineRenderer laserLine;

    // 玩家上次射击后的间隔时间
    private float nextFire;

    public bool defend = false;

    void Start()
    {
        // 获取LineRenderer组件
        laserLine = GetComponent<LineRenderer>();

        // 获取AudioSource组件
        gunAudio = GetComponent<AudioSource>();

    }


    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Defend"))
        {
            defend = !defend;
        }
        if (defend)
        {
            Vector3 shootAt = Vector3.zero;

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(mouseRay, out mouseHit))
            {

                shootAt = mouseHit.point;
                shootAt.x = transform.parent.position.x;
                transform.LookAt(shootAt);
            }

            Debug.DrawLine(gunEnd.position, gunEnd.position + (transform.forward * weaponRange));
            // 检测是否按下射击键以及射击间隔时间是否足够
            if (Input.GetMouseButton(0) && Time.time > nextFire&& !EventSystem.current.IsPointerOverGameObject())
            {

                // 射击之后更新间隔时间
                nextFire = Time.time + fireRate;

                // 启用ShotEffect携程控制射线显示及隐藏
                StartCoroutine(ShotEffect());

                // 在相机视口中心创建向量
                Vector3 rayOrigin = gunEnd.position;

                // 声明RaycastHit存储射线射中的对象信息
                RaycastHit hit;

                // 将射击轨迹起点设置为GunEnd对象的位置
                laserLine.SetPosition(0, gunEnd.position);

                

                //一点点偏移
                Vector3 randomVec = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                shootAt += randomVec;

                // 检测射线是否碰撞到对象
                if (Physics.Raycast(rayOrigin, shootAt-rayOrigin, out hit, weaponRange))
                {
                    laserLine.SetPosition(1, hit.point);
                    if (hit.collider.GetComponent<MechMissle>())
                    {
                        hit.collider.GetComponent<MechMissle>().ToBurst();
                    }
                }
                else
                {
                    // 如果未射中任何对象，则将射击轨迹终点设为相机前方的武器射程最大距离处
                    laserLine.SetPosition(1, rayOrigin+Vector3.Normalize(shootAt - rayOrigin)*weaponRange);
                }
            }
        }
        else
        {
            transform.LookAt(transform.position + transform.parent.forward);
        }
    }


    private IEnumerator ShotEffect()
    {
        // 播放音效
        gunAudio.Play();

        // 显示射击轨迹
        laserLine.enabled = true;

        // 等待0.07秒
        yield return shotDuration;

        // 等待结束后隐藏轨迹
        laserLine.enabled = false;
    }

}
