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
		public string text = "";
		public bool useImageInsteadOfText = false;
		public Sprite image = null;
	}

	[SerializeField] LevelDisplay[] levels = null;

	void StartCountdown() {
		//Show the district thing.  
		gameObject.SetActive (true);

		//Define the level that will be used locally by checking the current level in the GameData.  
		int levelToUse = GameData.GetLevel();

		if (levelToUse + 1 <= levels.Length) {

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

			StartCoroutine (DistrictDisplayingTimer());
		} else {
			Debug.LogError("No LevelDisplay fit the specified criteria");
		}
	}

	//Time the length the district is shown for.  
	IEnumerator DistrictDisplayingTimer() {
		yield return new WaitForSeconds(5f);
		gameObject.SetActive (false);
	}

}
