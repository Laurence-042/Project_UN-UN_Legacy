using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendFlight : MonoBehaviour
{
    [SerializeField] bool fighter = false;
    [SerializeField] Transform enemy;
    [SerializeField] NewFire[] friendFire;
    [SerializeField] NormalArmor armor;
    [SerializeField] float lastArmor;
    [SerializeField] float rotSen = 60f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float speed = 0f;
    [SerializeField] float period = 10f;
    float timer;
    bool runingAway = false;

    bool flyOver = false;

    Vector3 randomVector = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        GameObject[] temp_enemy = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> temp_enemy_l = new List<GameObject>();
        foreach (GameObject temp in temp_enemy)
        {
            if (temp != null)
            {
                if (temp.GetComponentInChildren<StealEnemy>()==null)
                {
                    temp_enemy_l.Add(temp);
                }
            }
        }
        if (temp_enemy_l.Count > 1)
        {
            enemy = temp_enemy_l[Random.Range(0, temp_enemy_l.Count)].transform;
            while (enemy.GetComponentInChildren<StealEnemy>())
            {
                enemy = temp_enemy_l[Random.Range(0, temp_enemy_l.Count)].transform;
            }
        }
        else
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
            if (enemy.GetComponentInChildren<StealEnemy>())
            {
                enemy = null;
            }
        }

        friendFire = GetComponentsInChildren<NewFire>();

        if (armor == null)
        {
            armor = GetComponent<NormalArmor>();
        }

        lastArmor = armor.fullArmor;

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            GameObject[] temp_enemy = GameObject.FindGameObjectsWithTag("Enemy");
            List<GameObject> temp_enemy_l = new List<GameObject>();
            foreach (GameObject temp in temp_enemy)
            {
                if (temp != null)
                {
                    temp_enemy_l.Add(temp);
                }
            }
            if (temp_enemy_l.Count > 1)
            {
                enemy = temp_enemy_l[Random.Range(0, temp_enemy_l.Count)].transform;
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Enemy"))
                {
                    enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
                }
                else
                {
                    enemy = null;
                    foreach (NewFire fire in friendFire)
                    {
                        fire.firing = false;
                    }
                    return;
                }

            }
        }
        timer += Time.deltaTime;
        if (timer > period)
        {
            timer = 0f;
            FlushState();
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (runingAway)
        {
            RunAway();
        }
        else
        {
            FlyToEnemy();
        }
        Attack();
    }

    void Attack()
    {
        if (!fighter)
        {
            if (Vector3.Angle(transform.forward, enemy.position - transform.position) < 10f && Vector3.Distance(enemy.position, transform.position) < friendFire[0].weaponRange)
            {
                foreach (NewFire fire in friendFire)
                {
                    fire.firing = true;
                }
            }
            else
            {
                foreach (NewFire fire in friendFire)
                {
                    fire.firing = false;
                }
            }
        }
        else
        {
            foreach (NewFire fire in friendFire)
            {
                fire.firing = true;
            }
        }
    }

    void FlushState()
    {
        if (lastArmor - armor.nowArmor > 10f)
        {
            runingAway = true;
        }
        else
        {
            runingAway = false;
            Debug.Log("damage " + (lastArmor - armor.nowArmor));
            Debug.Log("lastArmor " + lastArmor);
            Debug.Log("armor.nowArmor " + armor.nowArmor);
        }
        lastArmor = armor.nowArmor;
        ReRandom();
    }

    void RunAway()
    {
        DIY_Movement.SoftLookAt(this.gameObject, transform.position + (transform.position - enemy.position), 60f);
        speed = maxSpeed;
    }

    void FlyToEnemy()
    {
        if (Vector3.Dot(transform.position - enemy.position, transform.forward) > 0)
        {
            if (Vector3.Distance(transform.position, enemy.position) > 50f)
            {
                flyOver = false;
            }
            else
            {
                flyOver = true;
            }
        }
        if (!flyOver)
        {
            if (!fighter)
            {
                DIY_Movement.SoftLookAt(this.gameObject, enemy.position + randomVector * 3f * enemy.transform.localScale.x, rotSen);
            }
            else
            {
                DIY_Movement.SoftLookAt(this.gameObject, enemy.position, rotSen);

            }
        }
        else
        {
            DIY_Movement.SoftLookAt(this.gameObject, transform.position + (transform.position - enemy.position), rotSen);
        }

        speed = maxSpeed * 0.8f;
    }

    void ReRandom()
    {
        randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

}
