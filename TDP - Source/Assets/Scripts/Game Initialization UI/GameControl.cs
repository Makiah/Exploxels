﻿/*
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
		Application.LoadLevel ("Profession Chooser");
	}
	
	/***************************** PROFESSION CHOICE SCREEN *****************************/
	
	ProfessionChoiceManager mainProfessionChoiceManager;
	ProfessionSpeechManager mainProfessionSpeechManager;

	//Defines the elements required to get data.  
	public void InitializeProfessionObjects() {
		//Set references to existing items.
		mainProfessionChoiceManager = GameObject.Find ("UI").transform.FindChild ("ProfessionChoice").GetComponent <ProfessionChoiceManager> ();
		mainProfessionSpeechManager = GameObject.Find ("UI").transform.FindChild ("Speech Bubble").GetComponent <ProfessionSpeechManager> ();

		//Check the current level
		if (GetComponent <GameData> ().currentLevel == 0) {
			//Stone Age Speech
			mainProfessionSpeechManager.SetSpeechDialogue (new string[] {
				"Welcome, young wanderer, to the land of Exploxels!", 
				"Our world is undergoing rapid change.",
				"You will travel through many different time periods, watching the world progress.", 
				"The person that you will become can change greatly.", 
				"You will now enter the beginnings of humanity, the Stone Age.", 
				"Good luck."
			});
		} else if (GetComponent <GameData> ().currentLevel == 1) {
			//Iron Age Speech
			mainProfessionSpeechManager.SetSpeechDialogue (new string[] {
				"Nice job dealing with those cavemen!", 
				"I have an important announcement for you.", 
				"While mining deep underground, we have discovered a new metal.", 
				"We call it Bronze.", 
				"Use the new tools created by this metal to your advantage."
			});
		}
	}

	//When the player has finished speaking.  
	public void OnSpeechHasBeenCompleted() {
		if (GetComponent <GameData> ().currentLevel == 0) {
			//For the Stone Age
			mainProfessionChoiceManager.CreateProfessionChoice ("Choose your Stone Age Profession.", 
			                                                    ResourceDatabase.GetRaceByParameter ("Gatherer"), "Gatherer", 
			                                                    ResourceDatabase.GetRaceByParameter ("Hunter"), "Hunter"
			                                                    );
		} else if (GetComponent <GameData> ().currentLevel == 1) {
			//For the Iron Age
			mainProfessionChoiceManager.CreateProfessionChoice ("Choose your Iron Age Profession.", 
			                                                    ResourceDatabase.GetRaceByParameter ("Gatherer"), "Gatherer", 
			                                                    ResourceDatabase.GetRaceByParameter ("Hunter"), "Hunter"
			                                                    );
		}
	}
	
	//Called by the ProfessionChoiceManager when the profession has been chosen.  
	public void OnProfessionChosen(Profession chosen) {
		GetComponent<GameData> ().chosenProfession = chosen;
		//Load tutorial.  
		Application.LoadLevel ("Stone Age");
	}
	
	/**************************** TUTORIAL ****************************/
	
	public void OnTutorialComplete() {
		Debug.Log ("Gathering player data...");

		//Get player money.  
		GetComponent <GameData> ().currentPlayerMoney = CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerHealthPanelManager> ().GetPlayerMoney();
		Debug.Log ("Set player money to " + GetComponent <GameData> ().currentPlayerMoney);

		//Get player items.
		GetComponent <GameData> ().currentPlayerItems = ModifiesSlotContent.GetAllPlayerItems ();
		Debug.Log ("Player has " + GetComponent <GameData> ().currentPlayerItems.Length + " items");

		Debug.Log ("Tutorial has been completed!");
		//Load Profession Chooser Level.  
		GetComponent <GameData> ().currentLevel = 1;
		//Load the Profession Manager.
		Application.LoadLevel ("Profession Chooser");
	}
}
