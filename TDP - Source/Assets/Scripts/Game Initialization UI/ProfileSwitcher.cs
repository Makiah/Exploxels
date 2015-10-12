using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfileSwitcher : MonoBehaviour {

	void OnEnable() {
		UIEventManager.InitializeProfileSwitcher += InitializeProfileSwitcher;
	}

	void OnDisable() {
		UIEventManager.InitializeProfileSwitcher -= InitializeProfileSwitcher;
	}

	[HideInInspector]
	public int currentRace = 0;
	int inGameRaces;

	Image currentProfile;

	void InitializeProfileSwitcher() {
		inGameRaces = ResourceDatabase.GetNumberOfRacesInGame ();
		currentProfile = transform.FindChild ("Current Profile").GetComponent <Image> ();
		UpdateProfileSprite();
	}

	public void OnLeftButtonClick() {
		currentRace--;
		if (currentRace < 0) 
			currentRace = inGameRaces - 1;
		UpdateProfileSprite ();
	}

	public void OnRightButtonClick() {
		currentRace++;
		if (currentRace > inGameRaces - 1) 
			currentRace = 0;
		UpdateProfileSprite ();
	}

	void UpdateProfileSprite() {
		currentProfile.sprite = ResourceDatabase.GetRaceByParameter (currentRace).heads[0];
	}

}
	