using UnityEngine;
using System.Collections;

public class VariableManagement : MonoBehaviour {

	static GameObject playerObject;
	static GameObject mainCamera;
	float levelLengthX;

	void OnEnable() {
		LevelEventManager.CreatePlayerReference += SetPlayerReference;
	}

	void OnDisable() {
		LevelEventManager.CreatePlayerReference -= SetPlayerReference;
	}

	void SetPlayerReference() {
		playerObject = GameObject.Find ("PlayerReferenceObject(Clone)");
		mainCamera = playerObject.transform.FindChild ("Main Camera").gameObject;
	}

	public static GameObject GetPlayerReference() {
		return playerObject;
	}

	public static GameObject GetMainCameraReference() {
		return mainCamera;
	}

	public void SetLevelLengthX(float value) {
		levelLengthX = value;
	}

	public float GetLevelLengthX () {
		Debug.Log("Returned " + levelLengthX + " as level length");
		return levelLengthX;
	}
}
