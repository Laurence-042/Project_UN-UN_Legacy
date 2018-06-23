using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempController : MonoBehaviour
{
    public bool reset = false;
    public Vector3 resetPos;
    public Vector3 resetRot;

    private GameObject player;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                ResetPlayer(player, resetPos, resetRot);
                reset = false;
                player = null;
            }

        }
        

    }

    private void ResetPlayer(GameObject player)
    {
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void ResetPlayer(GameObject player, Vector3 pos, Vector3 rot)
    {
        player.transform.position = pos;
        player.transform.rotation = Quaternion.Euler(rot);
    }


}
