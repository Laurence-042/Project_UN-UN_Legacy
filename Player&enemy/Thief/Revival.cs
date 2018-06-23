using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revival : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerPre;
    [SerializeField] AudioSource auSource;
    [SerializeField] ParticleSystem revivalEffect;
    public Vector3 revivalPos=Vector3.zero;
	// Use this for initialization
	void Start () {
        playerPre = Resources.Load<GameObject>("Prefabs/Player/Thief");

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            player = Instantiate(playerPre, revivalPos, Quaternion.Euler(0, 0, 0));
            revivalEffect.transform.position = revivalPos;
            revivalEffect.Play();
            auSource.Play();
        }
	}
}
