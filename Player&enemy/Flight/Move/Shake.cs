using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{ 
	public float shakeTime = 0.2f;//震动时间  
	public float shakeAmount = 0.7f;//振幅  
	public float decreaseFactor = 1.0f;   

	public bool shaking=true;


	bool damage;  
	bool isDead;  
	// Use this for initialization  
	void Start () {  

	}  
 
	// Update is called once per frame  
	void Update () {  
		if (shaking&&shakeTime >0) 
		{  
			transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeAmount;
			shakeTime -= Time.deltaTime * decreaseFactor;  
		}  
		else  
		{  
			shakeTime = 0.2f;  
		}  
	}  
}