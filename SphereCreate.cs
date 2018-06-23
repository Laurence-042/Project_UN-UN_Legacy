using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreate : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private List<GameObject> prefabs;

    [SerializeField] private Vector3 selfPos;
    [SerializeField] private float creatRadius;
    [SerializeField] private float range;
    [SerializeField] private int num;
    [SerializeField] private float force;
    [SerializeField] private float timer;
    [SerializeField] private List<Vector3> torqueDirections;
    [SerializeField] private List<Vector3> flyDirections;

    [SerializeField] private GameObject parent;

    [SerializeField] private bool randomStartRotation;
    [SerializeField] private Vector3 startRotation;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < num; i++)
        {
            prefabs.Add(Instantiate(prefab));
            //thisObject.transform.position = selfPos + Random.onUnitSphere * radius;
            prefabs[i].transform.position = selfPos + Random.insideUnitSphere * creatRadius;
            if (randomStartRotation)
            {
                prefabs[i].transform.rotation = Random.rotation;
            }
            else
            {
                prefabs[i].transform.rotation = Quaternion.Euler(startRotation);
            }

            prefabs[i].transform.parent = parent.transform;

            float x = Random.Range(0, 10);
            float y = Random.Range(0, 10);
            float z = Random.Range(0, 10);
            torqueDirections.Add(new Vector3(x, y, z) * force / 10);
            x = Random.Range(-10, 10);
            y = Random.Range(-10, 10);
            z = Random.Range(-10, 10);
            flyDirections.Add(new Vector3(x, y, z) * force / 10);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (timer > 0)
        {
            for (int i = 0; i < num; i++)
            {
                while(prefabs[i] == null)
                {
                    prefabs.Remove(prefabs[i]);
                    num--;
                    if (num == 0)
                    {
                        break;
                    }
                }

                if (prefabs[i].GetComponent<Rigidbody>() != null)
                {
                    prefabs[i].GetComponent<Rigidbody>().AddTorque(torqueDirections[i]);
                    prefabs[i].GetComponent<Rigidbody>().AddForce(flyDirections[i]);
                }
            }
            timer -= Time.deltaTime;
        }/*
        else
        {
            Destroy(this.gameObject);
        }*/
        for (int i = 0; i < num; i++)
        {
            while (prefabs[i] == null)
            {
                prefabs.Remove(prefabs[i]);
                num--;
                if (num == 0)
                {
                    break;
                }
            }
            if (prefabs[i].GetComponent<Rigidbody>() != null)
            {
                if (Vector3.Distance(prefabs[i].transform.position, selfPos) > range)
                {
                    prefabs[i].GetComponent<Rigidbody>().AddForce(-flyDirections[i]);
                }
            }
        }
    }
}
