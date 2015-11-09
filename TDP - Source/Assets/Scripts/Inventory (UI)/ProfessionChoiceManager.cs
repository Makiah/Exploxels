using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfessionChoiceManager : MonoBehaviour {

	//Initialization
	void OnEnable() {
		LevelEventManager.InitializeProfessionChoiceManager += InitializeProfessionChoiceComponents;
	}

	void OnDisable() {
		LevelEventManager.InitializeProfessionChoiceManager -= InitializeProfessionChoiceComponents;
	}

	//Title
	Text title;
	//Choice 1
	Image choice1;
	Text description1;
	//Choice 2
	Image choice2;
	Text description2;

	//Initialization
	void InitializeProfessionChoiceComponents() {
		title = transform.FindChild ("Title").GetComponent <Text> ();
		choice1 = transform.FindChild ("Choice 1").GetComponent <Image> ();
		description1 = choice1.transform.FindChild ("Description").GetComponent <Text> ();
		choice2 = transform.FindChild ("Choice 2").GetComponent <Image> ();
		description2 = choice2.transform.FindChild ("Description").GetComponent <Text> ();
		gameObject.SetActive (false);
		CreateProfessionChoice ("Profession Choice", ResourceDatabase.GetRaceByParameter("MinecrafterMale"), "Hunter", ResourceDatabase.GetRaceByParameter("MinecrafterFemale"), "Gatherer");
	}

	//Used when a profession choice occurs.  
	public void CreateProfessionChoice(string titleText, Profession profession1, string d1, Profession profession2, string d2) {
		title.text = titleText;
		choice1.sprite = profession1.icon;
		description1.text = d1;
		choice2.sprite = profession2.icon;
		description2.text = d2;
		gameObject.SetActive (true);
		ScriptingUtilities.PauseGame ();
	}

	//Used when a profession has been chosen.  
	public void ResetProfessionChoice(int chosen) {
		choice1.sprite = null;
		description1.text = "";
		choice2.sprite = null;
		description2.text = "";
		gameObject.SetActive (false);
		ScriptingUtilities.ResumeGame ();
	}

	//There is an event trigger component on each object that will call these functions.  
	public void OnChoice1Clicked() {
		Debug.Log ("Got choice 1");
		ResetProfessionChoice (1);
	}

	public void OnChoice2Clicked() {
		Debug.Log ("Got choice 2");
		ResetProfessionChoice (2);
	}

}
