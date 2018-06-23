using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspMissle : MonoBehaviour {
    public Transform target;
    [SerializeField] private float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
            DIY_Movement.SoftLookAt(this.gameObject, target.position, 50);
        }
	}


}
