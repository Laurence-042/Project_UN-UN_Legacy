using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerFlight : MonoBehaviour
{

    public Vector3 destination;
    [SerializeField]  float maxSpeed=5f;
    public float speed=0f;

    public float scanRadius = 400f;
    [SerializeField] NewFire mainGun;
    [SerializeField] NewFire[] gunsUp;
    [SerializeField] NewFire[] gunsDown;
    [SerializeField] NewFire[] gunsLeft;
    [SerializeField] NewFire[] gunsRight;

    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject[] temp_enemies;
    [SerializeField] GameObject[] enemyTarget = new GameObject[4];//设为四,分别为上下左右最近的敌人


    // Use this for initialization
    void Start()
    {
        StartCoroutine(ScanEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        FireToEnemy();
        MoveToDestination();
    }

    void MoveToDestination()
    {
        if (Vector3.Distance(transform.position, destination) > 30f)
        {
            DIY_Movement.SoftLookAt(this.gameObject, destination, 5f);
            if (Vector3.Angle(transform.forward, destination - transform.position) > 5f)
            {
                return;
            }
            speed = Mathf.Lerp(speed, maxSpeed, 0.01f);
            transform.position=transform.position+ speed * transform.forward * Time.deltaTime;
        }
        else
        {
            speed= Mathf.Lerp(speed, 0f, 0.01f);
            transform.position = transform.position + speed * transform.forward*Time.deltaTime;
        }
    }

    void FireToEnemy()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
            }
            i--;
        }
        if (enemies.Count > 0)
        {
            foreach (NewFire gun in gunsUp)
            {
                if (enemyTarget[0] != null)
                {
                    gun.firing = true;
                    DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, enemyTarget[0].transform.position, 10f);
                }
                else
                {
                    gun.firing = false;
                }
            }
            foreach (NewFire gun in gunsDown)
            {
                if (enemyTarget[1] != null)
                {
                    gun.firing = true;
                    DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, enemyTarget[1].transform.position, 10f);
                }
                else
                {
                    gun.firing = false;
                }
            }
            foreach (NewFire gun in gunsLeft)
            {
                if (enemyTarget[2] != null)
                {
                    gun.firing = true;
                    DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, enemyTarget[2].transform.position, 10f);
                }
                else
                {
                    gun.firing = false;
                }
            }
            foreach (NewFire gun in gunsRight)
            {
                if (enemyTarget[3] != null)
                {
                    gun.firing = true;
                    DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, enemyTarget[3].transform.position, 10f);
                }
                else
                {
                    gun.firing = false;
                }
            }
        }
        else
        {
            foreach (NewFire gun in gunsUp)
            {
                gun.firing = false;
                DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, gun.gunEnd.position + gun.gunEnd.up * transform.localScale.x, 10f);
            }
            foreach (NewFire gun in gunsDown)
            {
                gun.firing = false;
                DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, gun.gunEnd.position - gun.gunEnd.up * transform.localScale.x, 10f);
            }
            foreach (NewFire gun in gunsLeft)
            {
                gun.firing = false;
                DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, gun.gunEnd.position - gun.gunEnd.right * transform.localScale.x, 10f);
            }
            foreach (NewFire gun in gunsRight)
            {
                gun.firing = false;
                DIY_Movement.SoftLookAt(gun.gunEnd.gameObject, gun.gunEnd.position + gun.gunEnd.right * transform.localScale.x, 10f);
            }
        }
    }

    IEnumerator ScanEnemy()
    {
        while (true)
        {
            enemies.Clear();
            temp_enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in temp_enemies)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= scanRadius)
                {
                    enemies.Add(enemy);
                }
                else
                {
                    //Debug.Log(Vector3.Distance(enemy.transform.position, transform.position));
                }
            }
            if (enemies.Count > 0)
            {
                enemyTarget[0] = enemyTarget[1] = enemyTarget[2] = enemyTarget[3] = enemies[0];
                for (int i = 0; i < enemies.Count; i++)
                {
                    float distance = 0;
                    distance = Vector3.Dot(transform.up, (enemies[i].transform.position - transform.position));
                    if (distance > 0)
                    {
                        if (Vector3.Angle(transform.up, (enemies[i].transform.position - transform.position)) > 60)
                        {
                            if (distance < Vector3.Dot(transform.up, (enemyTarget[0].transform.position - transform.position)))
                            {
                                enemyTarget[0] = enemies[i];
                            }
                        }
                    }
                    else
                    {
                        if (Vector3.Angle(-transform.up, (enemies[i].transform.position - transform.position)) > 60)
                        {
                            if (-distance < Vector3.Dot(-transform.up, (enemyTarget[1].transform.position - transform.position)))
                            {
                                enemyTarget[1] = enemies[i];
                            }
                        }
                    }

                    distance = Vector3.Dot(transform.right, (enemies[i].transform.position - transform.position));
                    if (distance > 0)
                    {
                        if (Vector3.Angle(transform.right, (enemies[i].transform.position - transform.position)) > 60)
                        {
                            if (distance < Vector3.Dot(transform.right, (enemyTarget[3].transform.position - transform.position)))
                            {
                                enemyTarget[3] = enemies[i];
                            }
                        }
                    }
                    else
                    {
                        if (Vector3.Angle(-transform.right, (enemies[i].transform.position - transform.position)) > 60)
                        {
                            if (-distance < Vector3.Dot(-transform.right, (enemyTarget[2].transform.position - transform.position)))
                            {
                                enemyTarget[2] = enemies[i];
                            }
                        }
                    }
                }
            }
            else
            {
                enemyTarget[0] = enemyTarget[1] = enemyTarget[2] = enemyTarget[3] = null;
            }
            yield return new WaitForSeconds(3f);
        }
    }

}
