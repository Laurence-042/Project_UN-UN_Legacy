using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private string text;//跟随的物体名称,target==null时起效
    [SerializeField] private Transform target;//跟随的物体
    [SerializeField] bool lookAt = false;//是否盯着
    [SerializeField] bool instanceFollow = true;
    [SerializeField] bool useTag = true;

    public float followDelay = -1f;//

    public Vector3 m_offset = Vector3.zero;
    public Vector3 offset = Vector3.zero;
    public float acceptableAngle = 0.5f;
    public float lerp_t = 0.1f;

    private float timer = 0;

    // Use this for initialization
    void Start()
    {
        if (!target)
        {
            target = GameObject.Find(text).transform;
        }
        m_offset = offset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            if (useTag)
            {
                if (GameObject.FindGameObjectWithTag(text))
                {
                    target = GameObject.FindGameObjectWithTag(text).transform;
                }
            }
            else
            {
                target = GameObject.Find(text).transform;
            }
        }

        if (followDelay > 0)
        {
            if (timer < followDelay)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                transform.position = target.position;
                if (lookAt)
                {
                    transform.LookAt(target.transform);
                }
            }
        }
        else if (instanceFollow)
        {
            transform.position = target.position + m_offset;
            transform.LookAt(target.transform);
        }
        else
        {
            if (target != null)
                if (Vector3.Angle(Vector3.left, target.transform.position - transform.position) > acceptableAngle)
                {
                    transform.position = Vector3.Lerp(transform.position, target.position + m_offset, lerp_t);
                    if (lookAt)
                    {
                        transform.LookAt(target.transform);
                    }
                }
        }




    }



}
