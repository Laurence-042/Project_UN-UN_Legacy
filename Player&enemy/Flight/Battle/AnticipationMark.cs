using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnticipationMark : MonoBehaviour {
	[SerializeField] private Transform player;
	[SerializeField] private Transform enemy;

	private float bulletSpeed;
	private float enemySpeed;
	private float angle;
	private float distance;

	private float delta;

	private float lastTime;
	private Vector3 enemyLastPos;

	private float a;
	private float b;
	private float c;
	private float ans;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").GetComponent<Transform> ();
		enemy = transform.parent.GetComponent<Transform> ();
		bulletSpeed = player.gameObject.GetComponent<Fire> ().fireLineSpeed*Time.deltaTime;
		enemySpeed = 0;

		enemyLastPos = enemy.position;
		lastTime = Time.time;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//下行的enemySpeed应依照浩楠提供的接口替换
		enemySpeed = enemy.gameObject.GetComponent<FlyDirectly> ().speed*Time.deltaTime;

        if (enemySpeed < bulletSpeed && player != null)
        {
            //下次不要忘了角度制转弧度制
            angle = Vector3.Angle(enemy.position - enemyLastPos, player.position - enemy.position) * Mathf.Deg2Rad;
            distance = Vector3.Distance(enemy.position, player.position);
            /*
		a = enemySpeed * enemySpeed - bulletSpeed * bulletSpeed;
		b = -2 * enemySpeed * (float)System.Math.Cos (angle) * distance;
		c = distance * distance;

		delta = (float)System.Math.Sqrt (b * b - 4 * a * c);

		ans = (-b + delta) / (2 * a);
		
		if (ans < 0) {
			ans = (-b - delta) / (2 * a);
		}
		*/

            a = enemySpeed;
            b = bulletSpeed;
            c = (float)System.Math.Cos(angle);
            ans = distance * (a * c - (float)System.Math.Sqrt(a * a * c * c - a * a + b * b)) / (a * a - b * b);

            transform.position = enemy.position + Vector3.Normalize(enemy.position - enemyLastPos) * enemySpeed * ans;
            transform.LookAt(player.position);
        }
        else
        {
            //Debug.Log("子弹速度低于目标速度");
        }



		lastTime = Time.time;
		enemyLastPos = enemy.position;

	}
}
