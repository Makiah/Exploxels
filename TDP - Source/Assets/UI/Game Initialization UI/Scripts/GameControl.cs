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
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {
	
	/***************************** INITIAL SCREEN *****************************/

	//Required references for the initial screen.  
	GameObject gameUI;
	ProfileSwitcher profileSwitcher;

	//Bertie (the icon that will be used).  
	[SerializeField] private Sprite bertieHead = null;

	//Defining initial level elements
	public void DefineInitialLevelElements() {
		DontDestroyOnLoad (this.gameObject);
		gameUI = GameObject.Find ("Game Initialization UI");
		profileSwitcher = gameUI.transform.FindChild ("Profile Switcher").gameObject.GetComponent <ProfileSwitcher> ();
	}
	
	public void OnLevelLoadButtonPress() {
		//Gather data from the initial screen.  
		GameData.SetChosenGender(profileSwitcher.currentGender == 0 ? GameData.Gender.MALE : GameData.Gender.FEMALE);
		GameData.SetPlayerName(gameUI.transform.FindChild ("NameField").GetComponent <InputField> ().text);
		//Load Profession Chooser level
		SceneManager.LoadScene ("Profession Chooser");
	}
	
	/***************************** PROFESSION CHOICE SCREEN *****************************/
	
	ProfessionChoiceManager mainProfessionChoiceManager;
	ProfessionSpeechManager mainProfessionSpeechManager;

	//Defines the elements required to get data.  
	public void InitializeProfessionObjects() {
		//Set references to existing items.
		mainProfessionChoiceManager = GameObject.Find ("UI").transform.FindChild ("ProfessionChoice").GetComponent <ProfessionChoiceManager> ();
		mainProfessionSpeechManager = GameObject.Find ("UI").transform.FindChild ("Speech Bubble").GetComponent <ProfessionSpeechManager> ();
		StartCoroutine (SetProfessionDialogue ());
	}

	IEnumerator SetProfessionDialogue() {
		switch (GameData.GetLevel()) {
		case 0:
			//Dramatic pause
			yield return new WaitForSeconds (2);
			//Stone Age Speech
			yield return StartCoroutine (mainProfessionSpeechManager.SetSpeechDialogue (
				bertieHead, 
				new string[] {
					"" + GameData.GetPlayerName() + "?", 
					"Are you all right?", 
					"Thank goodness you are still alive!", 
					"But I don't think we're in the twenty-fifth century anymore...", 
					"I am not sure how it happened, but the time machine malfunctioned.", 
					"It shot us to a point in time I am not programmed to understand.", 
					"Along the way, it fell apart after reaching terminal velocity, thankfully keeping us intact.", 
					"Pieces of it are now strewn across many parts of history.",
					"You have to help me fix it.  If you can help me gather the pieces, I will be able to repair it.", 
					"I don't see any other way.", 
					"But I'll have to disguise you as a native of this period in time, so that you fit in.", 
					"I'll let you choose who you become, then give you what you need to become that person.", 
					"As we gather more pieces, I should be able to push us farther into the future.",
					"...", 
					"...I sure hope my battery lasts long enough to get us to a time period with electricity..."
				}
			));

			//For the Stone Age
			yield return StartCoroutine (mainProfessionChoiceManager.CreateProfessionChoice ("Choose your Ice Age Profession.", 
				ResourceDatabase.GetRaceByParameter ("Spear Fighter"), "Spear Fighter", 
				ResourceDatabase.GetRaceByParameter ("Cave Shaman"), "Cave Shaman"
			));

			//Get the profession
			GameData.SetPlayerProfession(mainProfessionChoiceManager.GetChosenProfession ());

			//Give a bit more of an intro
			yield return StartCoroutine (mainProfessionSpeechManager.SetSpeechDialogue (
				bertieHead, 
				new string[] {
					"You'll have to figure out how to use your weapon.",
					"The basic controls are handled by the mouse.", 
					"Good luck!"
				}
			));

			//Load the Ice Age
			SceneManager.LoadScene("Ice Age");

			break;
		case 1: 
			Debug.LogError ("No real profession choice exists for this level!");
			//Just use the default ice age thing.  
			yield return StartCoroutine (mainProfessionSpeechManager.SetSpeechDialogue (
				bertieHead, 
				new string[] {
					"Nice job dealing with those cavemen!", 
					"I have an important announcement for you.", 
					"While mining deep underground, we have discovered a new metal.", 
					"We call it Bronze.", 
					"Use the new tools created by this metal to your advantage."
				}
			));

			//For the Iron Age
			yield return StartCoroutine (mainProfessionChoiceManager.CreateProfessionChoice ("Choose your Iron Age Profession.", 
				ResourceDatabase.GetRaceByParameter ("Spear Fighter"), "Spear Fighter", 
				ResourceDatabase.GetRaceByParameter ("Mace Fighter"), "Mace Fighter"
			));

			//Get the profession
			GameData.SetPlayerProfession(mainProfessionChoiceManager.GetChosenProfession ());

			//Load the Iron Age (when it exists)
			SceneManager.LoadScene("Iron Age");

			break;
		default: 
			Debug.LogError ("No profession choice available for level " + GameData.GetLevel());
			break;
		}
	}
	
	/**************************** TUTORIAL ****************************/
	
	public void OnCurrentLevelCompleted() {
		Debug.Log ("Gathering player data...");

		//Get player money.  
		GameData.SetPlayerMoney(CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerHealthPanelManager> ().GetPlayerMoney());
		Debug.Log ("Set player money to " + GameData.GetPlayerMoney());

		//Get player items.
		GameData.SetPlayerItems(CurrentLevelVariableManagement.GetMainInventoryReference().GetComponent <InventoryFunctions> ().GetAllPlayerItems ());
		Debug.Log ("Player has " + GameData.GetPlayerItems().Length + " items");

		Debug.Log ("Tutorial has been completed!");
		//Increment the current level.  
		GameData.SetLevel(GameData.GetLevel() + 1);
		//Load the Profession Chooser for the next level
		SceneManager.LoadScene ("Profession Chooser");
	}
}
