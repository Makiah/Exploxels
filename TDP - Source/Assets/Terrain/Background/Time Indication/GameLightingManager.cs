using UnityEngine;
using System.Collections;

public class GameLightingManager : MonoBehaviour {
	/*************** INIT ***************/
	void OnEnable() {
		LevelEventManager.InitializeTimeIndicator += InitializeTimeIndicationSystem;
	}

	void OnDisable() {
		LevelEventManager.InitializeTimeIndicator -= InitializeTimeIndicationSystem;
	}

	/*************** SCRIPT ***************/
	private Camera mainCamera;
	private Transform player;

	//Main directional light.  
	private Light mainLight;

	//Sun
	private Transform sunTransform;
	private Light sunlight;
	float initialSunlightIntensity = 0;

	private Transform moonTransform;
	private Light moonlight;
	float initialMoonlightIntensity = 0;

	//This is multiplied with the player's y coordinate.  
	float sunBrightnessCoefficient = 1;

	void InitializeTimeIndicationSystem() {
		//Get Player
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		//Get the camera.  
		mainCamera = transform.parent.GetComponent <Camera> ();
		//Get main directional light.  
		mainLight = GetComponent <Light> ();
		//Get sun  
		sunTransform = transform.FindChild ("Sun");
		sunlight = sunTransform.FindChild ("Sunlight").GetComponent <Light> ();
		initialSunlightIntensity = sunlight.intensity;
		//Get moon
		moonTransform = transform.FindChild ("Moon");
		moonlight = moonTransform.FindChild ("Moonlight").GetComponent <Light> ();
		initialMoonlightIntensity = moonlight.intensity;

		StartCoroutine("ControlSunAndMoonLighting");
	}

	//Manages the lighting depending on the sun and moon's position in the sky.  
	IEnumerator ControlSunAndMoonLighting() {
		//Controls the sun's speed.  
		float movingRate = .35f;
		float customTime = 0;

		//Continuously
		while (true) {
			//Aspect ratio is width over height.  
			//This may change at some points (resizing window) so it is calculated again every frame.  
			float cameraHeight = mainCamera.orthographicSize * 2;
			float cameraWidth = cameraHeight * mainCamera.aspect;

			//Custom clock that moves from 0 to 1 given the moving rate.  .01 is just a constant for convenience.  
			customTime += Time.deltaTime * .01f * movingRate;
			//Moves the sun and moon.  
			sunTransform.localPosition = new Vector3(cameraWidth * customTime, sunTransform.localPosition.y, sunTransform.localPosition.z);

			//The sun and moon should provide a brightness of a sine wave (brightest in the afternoon, neutral at dawn and dusk, and black at midnight).
			//This function has a period of 1.  (Period is 2pi/coefficient of x).  
			//This function oscillates the sunBrightnessCoefficient between 0.2 and 1.  
			sunBrightnessCoefficient = 0.7f * Mathf.Sin(2 * Mathf.PI * customTime) + .3f;

			//Parametric equations for an ellipse.
			//Where cameraWidth is the length of half of the ellipse, and customTime * 2pi is the period.  
			//It has to be flipped (goes counterclockwise otherwise).  Multiplied by 1.2 so the sun goes off the screen to begin with.  
			float x = -1 * (0.5f * cameraWidth * 1.2f) * Mathf.Cos (customTime * 2 * Mathf.PI);
			//Where 3 is half of the height of the ellipse.  
			float y = 2 * Mathf.Sin (customTime * 2 * Mathf.PI);

			//Sun movement and brightness
			sunTransform.localPosition = new Vector3 (x, y, sunTransform.transform.localPosition.z);
			//Turn off the brightness if behind clouds.  
			if (sunTransform.localPosition.y >= 0 && sunlight.intensity != initialSunlightIntensity) {
				sunlight.intensity = initialSunlightIntensity;
			} else if (sunTransform.localPosition.y < 0 && sunlight.intensity != 0) {
				sunlight.intensity = 0;
			}

			//Moon movement and brightness
			moonTransform.localPosition = new Vector3 (-x, -y, moonTransform.transform.localPosition.z);
			//Turn off the brightness if behind clouds.  
			if (moonTransform.localPosition.y >= 0 && moonlight.intensity != initialMoonlightIntensity) {
				moonlight.intensity = initialMoonlightIntensity;
			} else if (moonTransform.localPosition.y < 0 && moonlight.intensity != 0) {
				moonlight.intensity = 0;
			}
				
			//Player y position manager.  
			float undergroundIntensity = 1f / (Mathf.Sqrt(Mathf.Abs (Mathf.Clamp(player.position.y, -2500, -1))));

			//Works on the basis of (sunBrightness * (i-1)(undergroundIntensity))/i.  As one moves further underground, the sun oscillation is less noticeable.  
			float actualBrightness = (sunBrightnessCoefficient + (Mathf.Abs (Mathf.Clamp (player.transform.position.y, -2500, -1))) * undergroundIntensity) / 
				(Mathf.Abs (Mathf.Clamp (player.transform.position.y, -2500, -1)) + 1);

			//Debug.Log("Lighting is " + "(" + sunBrightnessCoefficient + " + " + (Mathf.Abs (Mathf.Clamp (player.transform.position.y, -2500, -1))) + "*" + undergroundIntensity + ") / " +
			//	(Mathf.Abs (Mathf.Clamp (player.transform.position.y, -2500, -1)) + 1) + " so actualBrightness is " + actualBrightness);

			//The brightness underground (deep underground) should not be affected by the sun.  
			mainLight.intensity = actualBrightness;

			yield return null;
		}
	}
}
