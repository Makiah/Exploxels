using UnityEngine;
using System.Collections;

public class SetInactiveAfterInitialization : MonoBehaviour {
	void OnEnable() {
		LevelEventManager.SetInactiveObjects += SetState;
	}

	void OnDisable() {
		LevelEventManager.SetInactiveObjects -= SetState;
	}

	void SetState() {
		gameObject.SetActive (false);
	}
}
