using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMissle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage = 10;
    [SerializeField] float burstTime = 1f;
    [SerializeField] ParticleSystem burstFire;
    private Vector3 direct = Vector3.zero;
    private bool burst = false;
    private bool lostTarget = false;
    public Transform player;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<Collider>() == null)
        {
            Debug.Log("导弹没碰撞体");
        }
        if (burstFire == null)
        {
            burstFire = GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!burst)
        {
            if (!lostTarget)
            {
                direct = player.position - transform.position;
                direct = Vector3.Normalize(direct);
                transform.Translate(direct * speed * Time.deltaTime);
                if (Vector3.Distance(player.position, transform.position) < 4f)
                {
                    lostTarget = true;
                }
            }
            else
            {
                transform.Translate(direct * speed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<ThiefHealth>() != null)
            {
                other.GetComponent<ThiefHealth>().TakeDamage(damage);
            }
            else
            {
                Debug.Log("玩家生命丢失");
            }
        }
        else
        {
            //Debug.Log("未击中玩家 " + other.gameObject.name);
        }
        if (other.tag != "Enemy")
        {
            burst = true;
            StartCoroutine(Burst());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<ThiefHealth>() != null)
            {
                collision.gameObject.GetComponent<ThiefHealth>().TakeDamage(damage);
            }
            else
            {
                Debug.Log("玩家生命丢失");
            }
        }
        else
        {
            Debug.Log("未击中玩家 " + collision.gameObject.name);
        }
        if (collision.gameObject.tag != "Enemy")
        {
            burst = true;
            StartCoroutine(Burst());
        }
    }

    public void ToBurst()
    {
        burst = true;
        StartCoroutine(Burst());
    }

    IEnumerator Burst()
    {
        burstFire.Play();
        yield return new WaitForSeconds(burstTime);
        Destroy(this.gameObject);
    }

}
