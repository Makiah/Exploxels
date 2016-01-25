using UnityEngine;
using System.Collections;

public class LimitSpriteZRotation : MonoBehaviour {

	[SerializeField] private float rotationRange = 0;
	
	void Update() {
		if (rotationRange < 180) {
			//If the angle is between 30 and 180, change to 30.  
			if (transform.eulerAngles.z > rotationRange && transform.eulerAngles.z <= 180) {
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, rotationRange);
			} else if (transform.eulerAngles.z < 360 - rotationRange && transform.eulerAngles.z > 180) {
				//If the angle is between 180 and 330, change to 330.  
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 360 - rotationRange);
			}
		} else {
			Debug.Log(rotationRange + " is not a valid rotation range!  " + rotationRange + " > 180 is " + (rotationRange > 180));
		}
	}

}
