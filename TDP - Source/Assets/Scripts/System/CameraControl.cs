
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

	void OnEnable() {
		EventManager.InitializeCameraFunctions += EnableCameraFunctions;
	}

	void OnDisable() {
		EventManager.InitializeCameraFunctions -= EnableCameraFunctions;
	}

	bool enable = false;
	public Transform playerTransform;
	public float moveSpeed;


	void EnableCameraFunctions() {
		StartCoroutine ("ListenForCameraFunctionEnable");
		StartCoroutine ("MouseControl");
	}

	IEnumerator ListenForCameraFunctionEnable() {
		while (true) {
			if (Input.GetKeyDown (KeyCode.C)) {
				enable = !enable;
			}
			yield return null;
		}
	}

	IEnumerator MouseControl() {
		while (true) {
			//The Screen point is remaining constant, yet the world point is changing as the object moves.  

			if (enable) {

				Vector3 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 optimalCameraPosition = (playerTransform.position + mousePosition) / 2 + new Vector3 (0, 0, -10);

				float speed = moveSpeed * Time.deltaTime * Mathf.Abs (transform.position.magnitude - optimalCameraPosition.magnitude);

				transform.position = Vector3.MoveTowards (transform.position, optimalCameraPosition, speed);

			}

			yield return null;
		}
	}

}
