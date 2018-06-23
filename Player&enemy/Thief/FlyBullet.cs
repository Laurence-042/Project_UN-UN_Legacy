using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBullet : MonoBehaviour {
    public float speed = 10f;

    public string enemyType = "MechEnemy";
	// Use this for initialization
	void Start () {
        if (!GetComponent<Collider>())
        {
            this.gameObject.AddComponent<Collider>();
            GetComponent<Collider>().isTrigger = true;
        }
        if (!GetComponent < Rigidbody>()) {
            this.gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
}
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (enemyType == "MechEnemy")
            {
                MechHealth enemyHeahth = other.gameObject.GetComponent<MechHealth>();
                enemyHeahth.TakeDamage(enemyHeahth.maxHP, transform.position + transform.forward * 0.3f);
            }
            else
            {
                if(enemyType == "FlightEnemy")
                {
                    NormalArmor enemyHeahth= other.gameObject.GetComponent<NormalArmor>();
                    enemyHeahth.TakeDamage(0.3f*enemyHeahth.fullArmor, transform.position + transform.forward * 0.3f);
                }
            }
        }
        StartCoroutine(DertroyBullet());
    }

    IEnumerator DertroyBullet()
    {
        speed = 0f;
        GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

}
