using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIData : MonoBehaviour {

	void OnEnable() {
		UIEventManager.InitializeUI += InitializeUIElements;
	}

	void OnDisable() {
		UIEventManager.InitializeUI -= InitializeUIElements;
	}

	//This will be the data that the game later receives from the UI.  
	public int chosenRace;

	GameObject gameUI;
	ProfileSwitcher profileSwitcher;

	public void InitializeUIElements() {
		DontDestroyOnLoad (this.gameObject);
		gameUI = GameObject.Find ("Game Initialization UI");
		profileSwitcher = gameUI.transform.FindChild ("Profile Switcher").gameObject.GetComponent <ProfileSwitcher> ();
	}

	public void OnButtonPress() {
		chosenRace = profileSwitcher.currentRace;
		Application.LoadLevel (1);
	}

}
