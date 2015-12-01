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

	private Light mainLight;
	private Light sunlight;

	private Transform sunAndMoonTransform;
	private SpriteRenderer currentSprite;

	private Transform player;

	float sunBrightness = 1;

	float initialSunlightIntensity = 0;

	void InitializeTimeIndicationSystem() {
		//Get the camera.  
		mainCamera = transform.parent.GetComponent <Camera> ();
		sunAndMoonTransform = transform.FindChild ("Sun");
		currentSprite = sunAndMoonTransform.GetComponent <SpriteRenderer> ();
		mainLight = GetComponent <Light> ();
		sunlight = sunAndMoonTransform.FindChild ("Sunlight").GetComponent <Light> ();
		initialSunlightIntensity = sunlight.intensity;

		//Move this object to the left side of the camera.  This might not be necessary.  
		transform.position = mainCamera.ScreenToWorldPoint (new Vector3 (0, mainCamera.orthographicSize / 2f, 0));

		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		StartCoroutine ("ManageInGameLighting");

		if (sun != null && moon != null) {
			StartCoroutine("ControlSunAndMoonLighting");
		} else {
			Debug.LogError("No sun and moon are specified!");
		}
	}

	//Manages the lighting depending on the sun and moon's position in the sky.  
	IEnumerator ControlSunAndMoonLighting() {
		bool usingSun = true;
		currentSprite.sprite = sun;
		float movingRate = .4f;
		float customTime = 0;
		while (true) {

			//Aspect ratio is width over height.  
			//This may change at some points (resizing window) so it is calculated again every frame.  
			float cameraWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;

			//Custom clock that moves from 0 to 1 given the moving rate.  .01 is just a constant for convenience.  
			customTime += Time.deltaTime * .01f * movingRate;
			//Moves the sun and moon.  
			sunAndMoonTransform.localPosition = new Vector3(cameraWidth * customTime, sunAndMoonTransform.localPosition.y, sunAndMoonTransform.localPosition.z);

			//The sun and moon should provide a brightness of a sine wave (brightest in the afternoon, neutral at dawn and dusk, and black at midnight).
			//This function has a period of 1.  (Period is 2pi/coefficient of x).  
			//This function oscillates the sunPositionCoefficient between 0.2 and 1.  
			sunBrightness = 0.4f * Mathf.Sin(2 * Mathf.PI * customTime) + .6f;

			//Move to the other side if the sun/moon has reached the opposite end.  
			if (customTime >= 0.5f && usingSun) {
				usingSun = false;
				//Ternary operator
				currentSprite.sprite = moon;
				sunlight.intensity = initialSunlightIntensity / 3f;
			}

			//Switch back to the sun when the moon has passed beyond the camera.  
			if (customTime >= 1 && usingSun == false) {
				usingSun = true;
				currentSprite.sprite = sun;
				sunlight.intensity = initialSunlightIntensity;
				customTime = 0;
			}

			yield return null;
		}
	}
	
	//This co-routine manages the ambient light present in the scene, thus making exploration deep underground more realistic.  However, it is 
	//VERY processor-intensive, and I am concerned that an area light may have to be employed instead.  
	//Note: Color declarations must be normalized (all parameters between 0 and 1).  
	IEnumerator ManageInGameLighting() {
		while (true) {
			//Lighting calculations.  
			float desiredIntensity = 1f / (Mathf.Sqrt(Mathf.Abs (Mathf.Clamp(player.position.y / 8f, -2500, -1))));
			mainLight.intensity = desiredIntensity * sunBrightness;
			yield return null;
		}
	}

}
