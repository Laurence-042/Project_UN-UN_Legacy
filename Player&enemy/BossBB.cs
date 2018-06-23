using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBB : MonoBehaviour
{
    [SerializeField] List<AudioClip> auClips;
    [SerializeField] AudioSource auSource;
    [SerializeField] float timeBetweenBB;
    // Use this for initialization
    void Start()
    {
        if (auSource == null)
        {
            if (!GetComponent<AudioSource>())
            {
                this.gameObject.AddComponent<AudioSource>();
            }
            auSource = GetComponent<AudioSource>();
        }

        StartCoroutine(BB());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BB()
    {
        while (true)
        {
            float randf = timeBetweenBB * Random.Range(0.7f, 1f);
            //Debug.Log("wait for " + randf);
            yield return new WaitForSeconds(randf);
            int rand = Random.Range(0, auClips.Count);
            auSource.PlayOneShot(auClips[rand]);
        }
    }

}
