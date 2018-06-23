using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaze : MonoBehaviour {
    [SerializeField] string gazeTag = "Player";
    [SerializeField] Transform gazeTarget;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gazeTarget == null)
        {
            gazeTarget = GameObject.FindGameObjectWithTag(gazeTag).transform;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((gazeTarget.position - transform.position), Vector3.up), 0.04f);
            //DIY_Movement.SoftLookAt(this.gameObject, gazeTarget.position, 40f);
        }
	}
}
