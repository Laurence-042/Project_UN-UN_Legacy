using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspMissleButton : MonoBehaviour {
    [SerializeField] private WaspMissleLauncher missleLauncher;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        GameObject target=null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            target = enemies[0];

            float distance = Vector3.Distance(missleLauncher.transform.position, enemies[0].transform.position);
            for (int i = 1; i < enemies.Length; i++)
            {
                if (Vector3.Distance(missleLauncher.transform.position, enemies[i].transform.position) < distance)
                {
                    target = enemies[i];
                }
            }

            missleLauncher.Launch(target);
        }
    }
}
