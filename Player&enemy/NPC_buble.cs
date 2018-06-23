using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_buble : MonoBehaviour {
    [SerializeField] GameObject buble;
    [SerializeField] Transform player;
    [SerializeField] float reactDistance = 3f;
    [SerializeField] string sayText="hello, world";
	// Use this for initialization
	void Start () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        buble.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            if (Vector3.Distance(player.position, transform.position) < reactDistance)
            {
                buble.SetActive(true);
                buble.GetComponent<TextMesh>().text = sayText.Replace("\\n", "\n");
            }
            else
            {
                buble.SetActive(false);
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}
}
