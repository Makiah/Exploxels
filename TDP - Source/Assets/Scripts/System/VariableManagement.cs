using UnityEngine;
using System.Collections;

public class VariableManagement : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.CreatePlayerReference += SetPlayerReference;
	}
	
	void OnDisable() {
		LevelEventManager.CreatePlayerReference -= SetPlayerReference;
	}

	static GameObject playerObject;
	static GameObject mainCamera;
	static GameObject levelUI;
	static float levelLengthX;
	
	void SetPlayerReference() {
		playerObject = GameObject.Find ("PlayerReferenceObject(Clone)");
		mainCamera = playerObject.transform.FindChild ("Main Camera").gameObject;
		levelUI = GameObject.Find ("UI");
	}

	public static GameObject GetPlayerReference() {
		return playerObject;
	}

	public static GameObject GetMainCameraReference() {
		return mainCamera;
	}

	public static GameObject GetLevelUIReference() {
		return levelUI;
	}

	public static void SetLevelLengthX(float value) {
		levelLengthX = value;
	}

	public float GetLevelLengthX () {
		if (levelLengthX == 0)
			Debug.LogError ("WARNING: Returning 0 as level length!");
		return levelLengthX;
	}
}
