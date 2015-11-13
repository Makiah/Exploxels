using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfessionChoiceManager : MonoBehaviour {

	//Initialization
	void OnEnable() {
		ProfessionEventManager.InitializeProfessionChoiceManager += InitializeProfessionChoiceComponents;
	}

	void OnDisable() {
		ProfessionEventManager.InitializeProfessionChoiceManager -= InitializeProfessionChoiceComponents;
	}

	//Title
	Text title;
	//Choice 1
	Image choice1;
	Text description1;
	//Choice 2
	Image choice2;
	Text description2;

	//The professions that are defined by the method.  
	Profession currentProfession1;
	Profession currentProfession2;

	PlayerCostumeManager mainPlayerCostumeManager;

	//Initialization
	void InitializeProfessionChoiceComponents() {
		title = transform.FindChild ("Title").GetComponent <Text> ();
		choice1 = transform.FindChild ("Choice 1").GetComponent <Image> ();
		description1 = choice1.transform.FindChild ("Description").GetComponent <Text> ();
		choice2 = transform.FindChild ("Choice 2").GetComponent <Image> ();
		description2 = choice2.transform.FindChild ("Description").GetComponent <Text> ();
		gameObject.SetActive (false);
	}

	//Used when a profession choice occurs.  
	public void CreateProfessionChoice(string titleText, Profession profession1, string d1, Profession profession2, string d2) {
		//Create the panel.  
		title.text = titleText;
		currentProfession1 = profession1;
		currentProfession2 = profession2;
		choice1.sprite = profession1.icon;
		description1.text = d1;
		choice2.sprite = profession2.icon;
		description2.text = d2;
		gameObject.SetActive (true);
	}

	//Used when a profession has been chosen.  
	public void ResetProfessionChoice(int chosen) {
		//Update player costume with new profession.
		Profession chosenProfession = chosen == 1 ? currentProfession1 : currentProfession2;
		//Reset variables.  
		currentProfession1 = null;
		currentProfession2 = null;
		choice1.sprite = null;
		description1.text = "";
		choice2.sprite = null;
		description2.text = "";
		gameObject.SetActive (false);
		//Resume game.
		CurrentLevelVariableManagement.GetMainGameControl().OnProfessionChosen (chosenProfession);
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
