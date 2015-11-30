using UnityEngine;
using System.Collections;

public class TimeIndicator : MonoBehaviour {
	/*************** INIT ***************/
	void OnEnable() {
		LevelEventManager.InitializeTimeIndicator += InitializeTimeIndicationSystem;
	}

	void OnDisable() {
		LevelEventManager.InitializeTimeIndicator -= InitializeTimeIndicationSystem;
	}

	/*************** SCRIPT ***************/
	[SerializeField] private Sprite sun = null;
	[SerializeField] private Sprite moon = null;
	private Camera mainCamera;

	Transform sunAndMoonTransform;
	SpriteRenderer currentSprite;

	void InitializeTimeIndicationSystem() {
		//Get the camera.  
		mainCamera = transform.parent.GetComponent <Camera> ();
		sunAndMoonTransform = transform.FindChild ("Sun");
		currentSprite = sunAndMoonTransform.GetComponent <SpriteRenderer> ();

		//Move this object to the left side of the camera.  This might not be necessary.  
		transform.position = mainCamera.ScreenToWorldPoint (new Vector3 (0, mainCamera.orthographicSize / 2f, 0));

		if (sun != null && moon != null) {
			StartCoroutine("MoveSunAndMoon");
		} else {
			Debug.LogError("No sun and moon are specified!");
		}
	}

	IEnumerator MoveSunAndMoon() {
		currentSprite.sprite = sun;
		while (true) {
			//Aspect ratio is width over height.  
			float cameraWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;
			float movingRate = .5f;
			float customTime = Mathf.Repeat(Time.time * .01f * movingRate, 1);
			sunAndMoonTransform.localPosition = new Vector3(cameraWidth * customTime, sunAndMoonTransform.localPosition.y, sunAndMoonTransform.localPosition.z);

			yield return null;
		}
	}

}
