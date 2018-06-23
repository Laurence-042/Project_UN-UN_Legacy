using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] float stretchScale = 10f;
    [SerializeField] bool transferPlayer = false;

    [SerializeField] AudioSource auSource;
    // Use this for initialization
    void Start()
    {
        if (auSource == null)
        {
            auSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " enter portal");
        if (other.gameObject.tag == "Player" && !transferPlayer)
        {
            GetComponent<SwitchLevel>().Switch();
            transferPlayer = true;
        }
        else if (other.gameObject.tag == "Friend")
        {
            StartCoroutine(TransferFriend(other.gameObject));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + "enter portal");
        if (collision.gameObject.tag == "Player" && !transferPlayer)
        {
            GetComponent<SwitchLevel>().Switch();
            transferPlayer = true;
        }
        else if (collision.gameObject.tag == "Friend")
        {
            StartCoroutine(TransferFriend(collision.gameObject));
        }
    }

    IEnumerator TransferFriend(GameObject friend)
    {
        float nowTransferRate = 0;
        Vector3 targertScale = new Vector3(friend.transform.localScale.x, friend.transform.localScale.y, friend.transform.localScale.z * stretchScale);
        Vector3 targetPos = friend.transform.position + transform.forward * (stretchScale - 1) * friend.transform.localScale.z;

        auSource.Play();
        while (nowTransferRate < 0.9f)
        {
            friend.transform.localScale = Vector3.Lerp(friend.transform.localScale, targertScale, 0.05f);
            nowTransferRate = friend.transform.localScale.z / targertScale.z;
            friend.transform.Translate(Vector3.forward * 100f);
            yield return new WaitForEndOfFrame();
        }
        Destroy(friend);
    }

}
