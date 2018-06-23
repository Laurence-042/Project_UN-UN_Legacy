using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
	[SerializeField]private GameObject self;
	[SerializeField]private GameObject fireLinePre;
    [SerializeField]private GameObject LaserFireLinePre;

    [SerializeField]private float rayLenth = 100f;
	[SerializeField]private float damage=1f;

	[SerializeField]private GameObject camera0;

	public float fireLineSpeed;

	public Vector3 gunPos=Vector3.zero;
	public float reInterval=1f;

    public bool lasterGun = false;
    public float lasterFireLineSpeed;
    public Vector3 lasterGunPos = Vector3.zero;
    public float LaserReInterval = 0.1f;

    private float thisInterval = 0;

    private bool firing = false;
	private bool ready=true;

	private float slowTimer=0;
	// Use this for initialization
	void Start () {
		fireLineSpeed = fireLinePre.GetComponent<FlyDirectly> ().speed;
	}
	
	// Update is called once per frame
	void Update () {
        //装弹
        

        if (!ready)
        {
            if (thisInterval > 0)
            {
                thisInterval -= Time.deltaTime;
            }
            else
            {
                if (lasterGun)
                {
                    thisInterval = LaserReInterval;
                }
                else
                {
                    thisInterval = reInterval;
                }
                ready = true;

            }

        }
        else
        {
            if (firing)
            {
                OpenFire();
                ready = false;
            }
        }

	}

	void FireButtonDown(){//按下绑定按钮
		firing = true;
	}

	void FireButtonUp(){//抬起绑定按钮
		firing = false;
	}

	void OpenFire(){
        //实例化子弹预设
        GameObject fireLine1;
        if (lasterGun)
        {
            fireLine1 = Instantiate(LaserFireLinePre, self.transform.position + self.transform.rotation*lasterGunPos, self.transform.rotation);
        }
        else
        {
            fireLine1 = Instantiate(fireLinePre, self.transform.position + gunPos, self.transform.rotation);
        }
	}


}
