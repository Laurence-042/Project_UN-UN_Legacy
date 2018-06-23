using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : MonoBehaviour {
	[SerializeField] private float damage=1f;
    [SerializeField] private float destroyTime=1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider target){
		//Debug.Log (target.gameObject.name);
		if (target.GetComponent<DestroyableEnemy> ()) {
			target.GetComponent<DestroyableEnemy> ().HP -= damage;
			if (target.GetComponent<DestroyableEnemy> ().HP <= 0) {
				
				target.GetComponent<DestroyableEnemy> ().Destroying ();
			}
            Destroy(this.gameObject);
		}
	}

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

}
