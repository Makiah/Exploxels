using UnityEngine;
using System.Collections;

public class LimitRotation : MonoBehaviour {

	//These values are set in the Inspector.  They control the max and min values.  
	[SerializeField] private Vector3 minRotation = Vector3.zero;
	[SerializeField] private Vector3 maxRotation = Vector3.zero;

	//Constantly make sure they are within their ranges.  
	void Update() {
		//Clamp values to respective min and max coordinates.  
		transform.eulerAngles = new Vector3 (Mathf.Clamp (transform.eulerAngles.x, minRotation.x, maxRotation.x), 
		                                     Mathf.Clamp (transform.eulerAngles.y, minRotation.y, maxRotation.y), 
		                                     Mathf.Clamp (transform.eulerAngles.z, minRotation.z, maxRotation.z));
		Debug.Log("Clamping to " + new Vector3 (Mathf.Clamp (transform.eulerAngles.x, minRotation.x, maxRotation.x), 
		                                        Mathf.Clamp (transform.eulerAngles.y, minRotation.y, maxRotation.y), 
		                                        Mathf.Clamp (transform.eulerAngles.z, minRotation.z, maxRotation.z)));
	}

}
