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
		//Show the district thing.  
		gameObject.SetActive (true);

		int levelToUse = CurrentLevelVariableManagement.GetMainGameData ().currentLevel;

		if (levelToUse + 1 < levels.Length) {

			//Access game data and determine the correct LevelDisplay.  
			LevelDisplay levelDisplayToUse = levels [levelToUse];
				
			//Set the text or image.  
			if (levelDisplayToUse.useImageInsteadOfText) {
				transform.FindChild ("Image").gameObject.SetActive (true);
				transform.FindChild ("Image").GetComponent <Image> ().sprite = levelDisplayToUse.image;
			} else {
				transform.FindChild ("Text").gameObject.SetActive (true);
				transform.FindChild ("Text").GetComponent <Text> ().text = levelDisplayToUse.text;
			}

			StartCoroutine ("DistrictDisplayingTimer");
		} else {
			Debug.LogError("No LevelDisplay fit the specified criteria");
		}
	}

	IEnumerator DistrictDisplayingTimer() {
		yield return new WaitForSeconds(5f);
		gameObject.SetActive (false);
	}

}
