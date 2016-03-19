
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
		LevelEventManager.InitializeCameraFunctions += EnableCameraFunctions;
	}

	void OnDisable() {
		LevelEventManager.InitializeCameraFunctions -= EnableCameraFunctions;
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

			//The required variables.  
			Vector3 mousePosition, optimalCameraPosition;
			float speed;

			//As long as the camera movement is enabled (could be disabled by pressing 'c'.  
			if (enable) {
				//Get the mouse and calculate the optimal camera position given the current mouse position.  
				mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				optimalCameraPosition = (playerTransform.position + mousePosition) / 2;
				optimalCameraPosition = new Vector3 (optimalCameraPosition.x, optimalCameraPosition.y, -10);

				//The camera should move less quickly the further it gets away from the player.  The camera has a base movement speed of 30.  
				speed = moveSpeed * Time.deltaTime * (30f / (Vector2.Distance (mousePosition, playerTransform.position) + 1));

				//Set the position of the camera based of off the other variables which had already been calculated.  
				transform.position = Vector3.MoveTowards (transform.position, optimalCameraPosition, speed);
			}

			//Wait one frame.  
			yield return null;
		}
	}

}
