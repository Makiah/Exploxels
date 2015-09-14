
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the camera.  Right now, it is not entirely effective, but it does work.    
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public bool enable;
	public Transform playerTransform;
	public float moveSpeed;

	void Update() {

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 optimalCameraPosition = (playerTransform.position + mousePosition) / 2 + new Vector3(0, 0, -10);

		//The Screen point is remaining constant, yet the world point is changing as the object moves.  

		if (enable) {

			float speed = moveSpeed * Time.deltaTime * Mathf.Abs(transform.position.magnitude - optimalCameraPosition.magnitude);

			transform.position = Vector3.MoveTowards(transform.position, optimalCameraPosition, speed);

		}

			
	}

}
