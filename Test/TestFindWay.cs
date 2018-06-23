using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TestFindWay : MonoBehaviour {

	public NavMeshAgent agent;
	Vector3 point;
	Ray aray;
	RaycastHit ahit;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			aray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(aray,out ahit))
			{
				point = ahit.point;
			}
			agent.SetDestination(point);
		}

	}
}