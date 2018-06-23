using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCollisionReact : MonoBehaviour
{
    public float damagePerSec = 1;
    [SerializeField] NewHp hp;
    [SerializeField] List<GameObject> colliders;
    // Use this for initialization
    void Start()
    {
        if (hp == null)
        {
            hp = GetComponent<NewHp>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (colliders.Count > 0)
        {
            for (int i = colliders.Count - 1; i >= 0; i--)
            {
                if (colliders[i] == null)
                {
                    colliders.Remove(colliders[i]);
                    i--;
                }
            }
            hp.Hit(damagePerSec * Time.deltaTime, transform.position);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Portal")
            colliders.Add(other.gameObject);
        //Debug.Log("trigger");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Portal")
            colliders.Add(collision.gameObject);
        //Debug.Log("collision");
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject object_in in colliders)
        {
            if (object_in.name == other.gameObject.name)
            {
                colliders.Remove(object_in);
                break;
            }
        }

    }


}
