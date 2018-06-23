using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlight : MonoBehaviour
{
    public string enemyType;
    [SerializeField] bool fighter = false;
    [SerializeField] Transform player;
    [SerializeField] Center center;
    [SerializeField] NewFire[] enemyFire;
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
        ReRandom();

        if (center == null)
        {
            center = GameObject.FindGameObjectWithTag("KeepPlayer").GetComponent<Center>();
        }

        enemyFire = GetComponentsInChildren<NewFire>();

        if (armor == null)
        {
            armor = GetComponent<NormalArmor>();
        }

        lastArmor = armor.fullArmor;

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Scane();

        }
        if (player)
        {
            if (!player.GetComponent<NewHp>())
            {
                Scane();
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
            FlyToPlayer();
        }

        if (center.isPlane)
        {
            Attack();
        }
        else
        {
            foreach (NewFire fire in enemyFire)
            {
                fire.firing = false;
            }

        }


    }

    void Attack()
    {
        if (!fighter)
        {
            if (Vector3.Angle(transform.forward, player.position - transform.position) < 10f && Vector3.Distance(player.position, transform.position) < enemyFire[0].weaponRange)
            {
                foreach (NewFire fire in enemyFire)
                {
                    fire.firing = true;
                }
            }
            else
            {
                foreach (NewFire fire in enemyFire)
                {
                    fire.firing = false;
                }
            }
        }
        else
        {
            foreach (NewFire fire in enemyFire)
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
        DIY_Movement.SoftLookAt(this.gameObject, transform.position + (transform.position - player.position), 60f);
        speed = maxSpeed;
    }

    void FlyToPlayer()
    {
        if (Vector3.Dot(transform.position - player.position, transform.forward) > 0)
        {
            if (Vector3.Distance(transform.position, player.position) > 50f)
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
                DIY_Movement.SoftLookAt(this.gameObject, player.position + randomVector * 3f * player.transform.localScale.x, rotSen);
            }
            else
            {
                DIY_Movement.SoftLookAt(this.gameObject, player.position, rotSen);
            }

        }
        else
        {
            DIY_Movement.SoftLookAt(this.gameObject, transform.position + (transform.position - player.position), rotSen);
        }

        speed = maxSpeed * 0.8f;
    }

    void ReRandom()
    {
        randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    void Scane()
    {
        if (randomVector.x > 0)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<NewHp>())
                {
                    player = GameObject.FindGameObjectWithTag("Player").transform;
                    return;
                }
            }

        }


        GameObject[] temp_player = GameObject.FindGameObjectsWithTag("Friend");
        List<GameObject> temp_player_l = new List<GameObject>();
        foreach (GameObject temp in temp_player)
        {
            if (temp != null)
            {
                temp_player_l.Add(temp);
            }
        }
        if (temp_player_l.Count > 0)
        {
            player = temp_player_l[Random.Range(0, temp_player_l.Count)].transform;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }



}
