using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour {
    public float set_PosX;
    public float set_PosY;
    public float set_PosZ;

    [SerializeField] private Transform player;

    public void Save()
    {
        set_PosX = player.position.x;
        set_PosY = player.position.y;
        set_PosZ = player.position.z;
        PlayerPrefs.SetFloat("PosX", set_PosX);
        PlayerPrefs.SetFloat("PosY", set_PosY);
        PlayerPrefs.SetFloat("PosZ", set_PosZ);
        Debug.Log("Save Successfully");
    }
    public void Load()
    {
        Vector3 pos = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        player.position = pos;
        Debug.Log("Load Successfully");
    }

}
