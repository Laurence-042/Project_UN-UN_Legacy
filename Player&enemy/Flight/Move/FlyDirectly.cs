using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyDirectly : MonoBehaviour
{
    public float flyTime = 6f;
    public float speed = 6f;
    public Vector3 dir = Vector3.zero;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dir == Vector3.zero)
        {
            if (flyTime > 0)
            {
                transform.Translate(0, 0, speed * Time.deltaTime);
                flyTime -= Time.deltaTime;
            }
            else
            {
                if (flyTime == -1)
                {
                    transform.Translate(0, 0, speed * Time.deltaTime);
                }
                else
                {
                    Destroy(this.gameObject);
                }

            }
        }
        else
        {
            if (flyTime > 0)
            {
                transform.Translate(dir * speed * Time.deltaTime);
                flyTime -= Time.deltaTime;
            }
            else
            {
                if (flyTime == -1)
                {
                    transform.Translate(dir * speed * Time.deltaTime);
                }
                else
                {
                    Destroy(this.gameObject);
                }

            }
        }
    }
}
