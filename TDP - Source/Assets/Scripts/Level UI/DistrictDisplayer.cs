using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistrictDisplayer : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializePlayer += StartCountdown;
	}

	void OnDisable() {
		LevelEventManager.InitializePlayer -= StartCountdown;
	}

	[System.Serializable]
	class LevelDisplay {
		public string text;
		public bool useImageInsteadOfText;
		public Sprite image;
	}

	[SerializeField] LevelDisplay[] levels;

	void StartCountdown() {
		gameObject.SetActive (true);
		//Set the text or image.  
		if (levels [0].useImageInsteadOfText) {
			transform.FindChild("Image").gameObject.SetActive(true);
			transform.FindChild ("Image").GetComponent <Image> ().sprite = levels [0].image;
		} else {
			transform.FindChild("Text").gameObject.SetActive(true);
			transform.FindChild ("Text").GetComponent <Text> ().text = levels [0].text;
		}

		StartCoroutine ("DistrictDisplayingManager");
	}

	IEnumerator DistrictDisplayingManager() {
		yield return new WaitForSeconds(5f);
		gameObject.SetActive (false);
	}

}
