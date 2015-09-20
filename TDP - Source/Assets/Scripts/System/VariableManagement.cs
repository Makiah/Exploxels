using UnityEngine;
using System.Collections;

public class VariableManagement : MonoBehaviour {

	GameObject playerObject;

	void OnEnable() {
		EventManager.CreatePlayerReference += SetPlayerReference;
	}

	void OnDisable() {
		EventManager.CreatePlayerReference -= SetPlayerReference;
	}

	void SetPlayerReference() {
		playerObject = GameObject.Find ("PlayerReferenceObject(Clone)");
		Debug.Log (playerObject.name);
	}

	public GameObject GetPlayerReference() {
		return playerObject;
	}
}
