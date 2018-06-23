using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour {
	
	[SerializeField]private UISlider hpbar;
	public float hp=100.0f;
	public float senDamage=6.0f;

	private bool shaking = false;

	private float selfDamage;
	private float rand=0;

	// Use this for initialization
	void Start () {
        if (hpbar==null)
        {
            hpbar = GameObject.FindGameObjectWithTag("HPbar").GetComponent<UISlider>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		//处理血条
		if (hp > 100)
			hp = 100;
		hpbar.value = hp / 100;

	}
	public void Hit(float damage){
		hp -= damage;
		if (hp < 0) {
			hp = 0;
            transform.parent. GetComponent<EjectPlayer>().ActiveEject();
		}

		selfDamage = damage;

		
	}

}
