using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMissleLauncher : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float launchRadius = 30f;
    [SerializeField] float timeBetweenMissles = 10f;
    [SerializeField] GameObject misslePref;
    [SerializeField] List<MechMissle> missles;
    [SerializeField] int missleNum = 3;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(MissleLaunch());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MissleLaunch()
    {
        while (true)
        {
            if (player != null)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < launchRadius)
                {
                    Ray ray = new Ray();
                    RaycastHit hit;
                    ray.origin = transform.position;
                    ray.direction = player.transform.position - transform.position;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.tag == "Player" && missles.Count == 0)
                        {
                            for (int i = 0; i < missleNum; i++)
                            {
                                GameObject tempMissle = Instantiate(misslePref, transform.position, Quaternion.Euler(0, 0, 0));
                                tempMissle.GetComponent<MechMissle>().player = player.transform;
                                missles.Add(tempMissle.GetComponent<MechMissle>());
                            }

                            yield return new WaitForSeconds(timeBetweenMissles);
                        }

                    }
                    else
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            for(int i= missles.Count - 1; i >= 0; i--)
            {
                if (missles[i] == null)
                {
                    missles.Remove(missles[i]);
                    i--;
                }
            }
            yield return new WaitForEndOfFrame();

        }

    }
}


