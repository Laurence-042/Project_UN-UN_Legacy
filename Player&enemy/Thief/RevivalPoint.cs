using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalPoint : MonoBehaviour {
    [SerializeField] ParticleSystem activeEffect;
    [SerializeField] Revival revival;
	// Use this for initialization
	void Start () {
        if (revival == null)
        {
            revival = GameObject.Find("Revival").GetComponent<Revival>();
        }
        if (activeEffect == null)
        {
            activeEffect = GetComponentInChildren<ParticleSystem>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            revival.revivalPos = new Vector3(0, transform.position.y, transform.position.z);
            activeEffect.Play();
        }
    }

}
