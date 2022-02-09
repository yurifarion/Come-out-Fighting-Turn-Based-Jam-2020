using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
      public GameObject target;
	  public Vector3 target_Offset;
		 private void Start()
		 {
			 target_Offset = transform.position - target.transform.position;
		 }
		 void Update()
		 {
			 if (target)
			 {
				 transform.position = Vector3.Lerp(transform.position, target.transform.position+target_Offset, 0.1f);
			 }
		 }
}
