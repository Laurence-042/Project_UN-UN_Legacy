using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIY_Movement {
	
	public static void SoftLookAt(GameObject self,Vector3 target,float m_MaxRotatSpeed){
		// 目标偏移  
		Vector3 offset = ( target - self.transform.position ).normalized;  

		// 根据当前面向，计算转到面向目标所需要的角度  
		float angle = Vector3.Angle( self.transform.forward, offset );  

		// 计算面向目标大约所需要的时间  
		float needTime = angle / m_MaxRotatSpeed;  

		float v = 1;  // 旋转进度（第三参数）  
		// 如果所需要的时间接近0，则直接转到位，否则重新计算进度。  
		if( needTime > Mathf.Epsilon )  
			v = Time.deltaTime / needTime;  

		// 目标Rotation  
		Quaternion t = Quaternion.LookRotation(( target - self.transform.position), Vector3.up );  

		// 进行旋转  
		self.transform.rotation = Quaternion.Slerp( self.transform.rotation, t, v );  

	}

}
