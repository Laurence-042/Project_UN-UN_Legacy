using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspMissleLauncher : MonoBehaviour {
    [SerializeField] private GameObject misslePre;
    [SerializeField] private int num=1;
    [SerializeField] private Quaternion direction;

    // Use this for initialization
    void Start () {
		
	}
	
	public void Launch(GameObject target)
    {
        Quaternion rot = rot = Quaternion.Euler(0, 0, 0);
        if (num == 1)
        {
            direction = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            direction = Quaternion.Euler(0, 45, 0);
            rot = Quaternion.Euler(0, 0, 360 / num);
        }
       
        for(int i = 0; i < num; i++)
        {
            GameObject missle = Instantiate(misslePre, transform.localPosition, Quaternion.Euler( transform.localRotation.eulerAngles+direction.eulerAngles));
            missle.GetComponent<WaspMissle>().target = target.transform;
            direction = rot * direction;
        }
    }
}
