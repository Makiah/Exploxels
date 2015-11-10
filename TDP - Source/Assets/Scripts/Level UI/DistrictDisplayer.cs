using UnityEngine;
using System.Collections;

public class DistrictDisplayer : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializePlayer += StartCountdown;
	}

	void OnDisable() {
		LevelEventManager.InitializePlayer -= StartCountdown;
	}

	void StartCountdown() {
		gameObject.SetActive (true);
		StartCoroutine ("DistrictDisplayingManager");
	}

	IEnumerator DistrictDisplayingManager() {
		yield return new WaitForSeconds(5f);
		gameObject.SetActive (false);
	}

}
