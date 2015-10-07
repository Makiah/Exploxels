using UnityEngine;
using System.Collections;

public class LightingManager : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeLightingSystem += InitializeLightingSystem;
	}

	void OnDisable() {
		LevelEventManager.InitializeLightingSystem -= InitializeLightingSystem;
	}

	private GameObject player;

	void InitializeLightingSystem() {
		player = VariableManagement.GetPlayerReference ();
		StartCoroutine ("ManageInGameLighting");
	}

	//This co-routine manages the ambient light present in the scene, thus making exploration deep underground more realistic.  However, it is 
	//VERY processor-intensive, and I am concerned that an area light may have to be employed instead.  
	//Note: Color declarations must be normalized (all parameters between 0 and 1).  
	IEnumerator ManageInGameLighting() {
		while (true) {
			Color desiredColor = new Color(1f + Mathf.Clamp(player.transform.position.y / 100f, -1f, 0), 1f + Mathf.Clamp(player.transform.position.y / 100f, -1f, 0), 1f + Mathf.Clamp(player.transform.position.y / 100f, -1f, 0));
			RenderSettings.ambientSkyColor = desiredColor;
			yield return null;
		}
	}

}
