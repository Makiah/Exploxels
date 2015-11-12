using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	//Initialization
	void OnEnable() {
		UIEventManager.InitializeUI += DefineInitialLevelElements;
	}

	void OnDisable() {
		UIEventManager.InitializeUI -= DefineInitialLevelElements;
	}


	/***************************** INITIAL SCREEN *****************************/

	//This will be the data that the game later receives from the UI.  
	[HideInInspector] public int chosenGender;
	[HideInInspector] public string specifiedPlayerName;
	GameObject gameUI;
	ProfileSwitcher profileSwitcher;
	
	void DefineInitialLevelElements() {
		DontDestroyOnLoad (this.gameObject);
		gameUI = GameObject.Find ("Game Initialization UI");
		profileSwitcher = gameUI.transform.FindChild ("Profile Switcher").gameObject.GetComponent <ProfileSwitcher> ();
		CurrentLevelVariableManagement.SetGameUIReferences ();
	}

	public void OnLevelLoadButtonPress() {
		//Gather data from the initial screen.  
		chosenGender = profileSwitcher.currentGender;
		specifiedPlayerName = gameUI.transform.FindChild ("NameField").GetComponent <InputField> ().text;
		//Load Profession Chooser Level
		Application.LoadLevel (3);

	}

	/***************************** PROFESSION CHOICE SCREEN *****************************/

	ProfessionChoiceManager mainProfessionChoiceManager;
	[HideInInspector] public Profession chosenProfession;

	//Defines the elements required to get data.  
	public void DefineProfessionChooserElements() {
		mainProfessionChoiceManager = GameObject.Find ("UI").transform.FindChild ("ProfessionChoice").GetComponent <ProfessionChoiceManager> ();
		mainProfessionChoiceManager.CreateProfessionChoice ("Choose your player's profession.", 
		                                                   ResourceDatabase.GetRaceByParameter ("Gatherer"), "Gatherer", 
		                                                   ResourceDatabase.GetRaceByParameter ("Hunter"), "Hunter"
		                                                    );
	}

	//Called by the ProfessionChoiceManager when the profession has been chosen.  
	public void OnProfessionChosen(Profession chosen) {
		chosenProfession = chosen;
		Debug.Log ("Chosen profession is " + chosenProfession.name);
		Application.LoadLevel (2);
	}

}
