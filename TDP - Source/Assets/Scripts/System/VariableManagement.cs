using UnityEngine;
using System.Collections;

public class VariableManagement : MonoBehaviour {

	GameObject playerObject;
	GameObject mainCamera;
	float levelLengthX;

	void OnEnable() {
		EventManager.CreatePlayerReference += SetPlayerReference;
	}

	void OnDisable() {
		EventManager.CreatePlayerReference -= SetPlayerReference;
	}

	void SetPlayerReference() {
		playerObject = GameObject.Find ("PlayerReferenceObject(Clone)");
		mainCamera = playerObject.transform.FindChild ("Main Camera").gameObject;
	}

	public GameObject GetPlayerReference() {
		return playerObject;
	}

	public GameObject GetMainCameraReference() {
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
