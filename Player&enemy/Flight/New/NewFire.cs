using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class NewFire : MonoBehaviour
{
    [SerializeField] int state = 0;//-1:敌人 0:自身 1:友军 
    public bool firing = false;

    // 设置枪击带来的伤害值
    public int gunDamage = 1;
    // 设置两次枪击的间隔时间
    public float gunMissHit = 1f;
    public float fireRate = 0.25f;
    // 设置玩家可以射击的Unity单位
    public float weaponRange = 50f;
    // GunEnd游戏对象
    public Transform gunEnd;

    // 枪击音效
    [SerializeField] AudioSource gunAudio;
    // 射击轨迹
    [SerializeField] LineRenderer laserLine;
    // 瞄准光线
    [SerializeField] LineRenderer aimLine;
    // 射击光焰
    [SerializeField] Light gunLight;

    // 设置射击轨迹显示的时间
    [SerializeField] float shotDuration = 0.15f;
    // 特效时间
    [SerializeField] float effectsDisplayTime = 0.2f;
    // 玩家上次射击后的间隔时间
    [SerializeField] float timeBetweenBullets = 0.1f;

    public bool active = false;
    [SerializeField] bool aimLineEnable = true;
    float timer;
    Ray shootRay = new Ray();
    // Use this for initialization
    void Start()
    {
        if (laserLine == null)
        {
            laserLine = GetComponent<LineRenderer>();
        }
        if (gunAudio == null)
        {
            gunAudio = GetComponent<AudioSource>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (active)
        {
            aimLine.enabled = true && aimLineEnable;
            aimLine.SetPosition(0, gunEnd.position);
            aimLine.SetPosition(1, gunEnd.position + (gunEnd.forward * weaponRange));

            if (state == 0)
            {
                if (CrossPlatformInputManager.GetButton("Fire") && timer > timeBetweenBullets)
                {
                    Shoot();
                }
            }
            else if (firing && timer > timeBetweenBullets)
            {
                OtherShoot();
            }

        }
        else
        {
            aimLine.enabled = false && aimLineEnable;
            transform.LookAt(transform.position + transform.parent.forward);
        }
    }

    public void DisableEffects()
    {
        laserLine.enabled = false && aimLineEnable;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0;
        StartCoroutine(ShotEffect());

        laserLine.SetPosition(0, gunEnd.position);


        shootRay.origin = gunEnd.position;
        shootRay.direction = gunEnd.forward;

        Debug.DrawLine(gunEnd.position, gunEnd.position + (transform.forward * weaponRange));

        // 声明RaycastHit存储射线射中的对象信息
        RaycastHit hit;

        Vector3 shootAt = shootRay.origin + shootRay.direction * weaponRange;
        //一点点偏移
        Vector3 randomVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        shootAt += randomVec;

        // 检测射线是否碰撞到对象
        if (Physics.SphereCast(shootRay, 6f, out hit, weaponRange))//shootRay.origin, shootAt - shootRay.origin, out hit, weaponRange)
        {
            if (hit.collider.GetComponent<NormalArmor>() && hit.collider.tag == "Enemy")
            {
                if (Vector3.Angle(transform.forward, hit.point - gunEnd.position) < 20f)
                {
                    hit.collider.GetComponent<NormalArmor>().TakeDamage(gunDamage, hit.point);
                    laserLine.SetPosition(1, hit.point);
                }
                else
                {
                    laserLine.SetPosition(1, shootAt);
                }
            }
            else
            {
                laserLine.SetPosition(1, shootAt);
            }
        }
        else
        {
            laserLine.SetPosition(1, shootAt);
        }
    }

    void OtherShoot()
    {
        timer = 0;
        StartCoroutine(ShotEffect());

        laserLine.SetPosition(0, gunEnd.position);

        shootRay.origin = gunEnd.position;
        shootRay.direction = gunEnd.forward;

        Debug.DrawLine(gunEnd.position, gunEnd.position + (gunEnd.forward * weaponRange));

        // 声明RaycastHit存储射线射中的对象信息
        RaycastHit hit;

        Vector3 shootAt = shootRay.origin + shootRay.direction * weaponRange;
        Vector3 randomVec = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        // 检测射线是否碰撞到对象
        if (Physics.SphereCast(shootRay, 10f, out hit, weaponRange))//shootRay.origin, shootAt - shootRay.origin, out hit, weaponRange)
        {
            if (state == -1)
            {
                if (hit.collider.GetComponent<NewHp>())
                {
                    float random = Random.Range(-1f, gunMissHit);
                    if (Vector3.Angle(gunEnd.forward, hit.point - gunEnd.position) < 10f && random < 0)
                    {
                        hit.collider.GetComponent<NewHp>().Hit(gunDamage, hit.point);
                        laserLine.SetPosition(1, hit.point);
                    }
                    else
                    {
                        //一点点偏移
                        shootAt = shootRay.origin + Vector3.Normalize(hit.point - gunEnd.position) * weaponRange;
                        shootAt += randomVec;
                        laserLine.SetPosition(1, shootAt); ;
                    }
                }
                else
                {
                    laserLine.SetPosition(1, shootAt);
                    //Debug.Log("shoot at" + shootAt);
                }
            }
            else if (state == 1)
            {
                if (hit.collider.GetComponent<NormalArmor>())
                {
                    float random = Random.Range(-1f, gunMissHit);
                    if (Vector3.Angle(gunEnd.forward, hit.point - gunEnd.position) < 10f && random < 0)
                    {
                        hit.collider.GetComponent<NormalArmor>().TakeDamage(gunDamage, hit.point);
                        laserLine.SetPosition(1, hit.point);
                    }
                    else
                    {
                        //一点点偏移
                        shootAt = shootRay.origin + Vector3.Normalize(hit.point - gunEnd.position) * weaponRange;
                        shootAt += randomVec;
                        laserLine.SetPosition(1, shootAt); ;
                    }
                }
                else
                {
                    laserLine.SetPosition(1, shootAt);
                    //Debug.Log(transform.parent.name + "miss shoot at" + shootAt);
                }

            }
        }
        else
        {
            laserLine.SetPosition(1, shootAt);
            //Debug.Log(transform.parent.name + "lost target shoot at" + shootAt);
        }
    }



    IEnumerator ShotEffect()
    {
        if (state == 0)
        {
            gunAudio.Play();
        }

        laserLine.enabled = true;

        gunLight.enabled = true;
        yield return new WaitForSeconds(effectsDisplayTime * shotDuration);
        gunLight.enabled = false;

        yield return new WaitForSeconds(shotDuration - effectsDisplayTime * shotDuration);
        laserLine.enabled = false;
    }

}
