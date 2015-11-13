/*
 * Author: Makiah Bennett
 * Date Created: 13 November 2015
 * 
 * Description: This script controls the main events that occur during the game, such as new level loading, etc.  Similar to GameData.  
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
	
	/***************************** INITIAL SCREEN *****************************/

	//Required references for the initial screen.  
	GameObject gameUI;
	ProfileSwitcher profileSwitcher;

	//Defining initial level elements
	public void DefineInitialLevelElements() {
		DontDestroyOnLoad (this.gameObject);
		gameUI = GameObject.Find ("Game Initialization UI");
		profileSwitcher = gameUI.transform.FindChild ("Profile Switcher").gameObject.GetComponent <ProfileSwitcher> ();
	}
	
	public void OnLevelLoadButtonPress() {
		//Gather data from the initial screen.  
		GetComponent <GameData> ().chosenGender = profileSwitcher.currentGender;
		GetComponent <GameData> ().specifiedPlayerName = gameUI.transform.FindChild ("NameField").GetComponent <InputField> ().text;
		//Load Profession Chooser level
		Application.LoadLevel (3);
	}
	
	/***************************** PROFESSION CHOICE SCREEN *****************************/
	
	ProfessionChoiceManager mainProfessionChoiceManager;
	ProfessionSpeechManager mainProfessionSpeechManager;

	//Defines the elements required to get data.  
	public void InitializeProfessionObjects() {
		mainProfessionChoiceManager = GameObject.Find ("UI").transform.FindChild ("ProfessionChoice").GetComponent <ProfessionChoiceManager> ();
		mainProfessionSpeechManager = GameObject.Find ("UI").transform.FindChild ("Speech Bubble").GetComponent <ProfessionSpeechManager> ();

		if (GetComponent <GameData> ().currentLevel == 0) {
			StoneAgeProfessionChoice ();
		} else if (GetComponent <GameData> ().currentLevel == 1) {
			IronAgeProfessionChoice();
		}
	}

	public void StoneAgeProfessionChoice() {
		mainProfessionSpeechManager.SetSpeechDialogue (new string[] {
			"Welcome, young wanderer, to the land of Exploxels!", 
			"Our world is undergoing rapid change.",
			"You will travel through many different time periods, watching the world progress.", 
			"The person that you will become can change greatly.", 
			"Good luck."
		});
		mainProfessionChoiceManager.CreateProfessionChoice ("Choose your player's profession.", 
		                                                    ResourceDatabase.GetRaceByParameter ("Gatherer"), "Gatherer", 
		                                                    ResourceDatabase.GetRaceByParameter ("Hunter"), "Hunter"
		                                                    );
	}

	public void IronAgeProfessionChoice() {
		mainProfessionSpeechManager.SetSpeechDialogue (new string[] {
			"Nice job dealing with those cavemen!", 
			"I have an important announcement for you.", 
			"While mining deep underground, we have discovered a new metal.", 
			"We call it Bronze."
		});
		mainProfessionChoiceManager.CreateProfessionChoice ("Choose your player's profession.", 
		                                                    ResourceDatabase.GetRaceByParameter ("Gatherer"), "Gatherer", 
		                                                    ResourceDatabase.GetRaceByParameter ("Hunter"), "Hunter"
		                                                    );
	}
	
	//Called by the ProfessionChoiceManager when the profession has been chosen.  
	public void OnProfessionChosen(Profession chosen) {
		GetComponent<GameData> ().chosenProfession = chosen;
		//Load tutorial.  
		Application.LoadLevel (2);
	}
	
	/**************************** TUTORIAL ****************************/
	
	public void OnTutorialComplete() {
		Debug.Log ("Tutorial has been completed!");
		//Load Profession Chooser Level.  
		GetComponent <GameData> ().currentLevel = 1;
		//Load the Profession Manager.
		Application.LoadLevel (3);
	}
}
