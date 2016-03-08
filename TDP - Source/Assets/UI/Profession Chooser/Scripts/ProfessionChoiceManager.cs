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

	//The character components.  
	class CharacterSprites {
		//Components and constructor.
		public readonly SpriteRenderer body, head, leg1, leg2, holdingHand, idleHand, holdingItem;
		public CharacterSprites(Transform character) {
			body = character.FindChild("Body").GetComponent <SpriteRenderer> ();
			head = character.FindChild("Head").GetComponent <SpriteRenderer> ();
			leg1 = character.FindChild("Legs").FindChild("Bottom Leg").GetComponent <SpriteRenderer> ();
			leg2 = character.FindChild("Legs").FindChild("Top Leg").GetComponent <SpriteRenderer> ();
			holdingHand = character.FindChild("Hands").FindChild("HoldingHand").GetComponent <SpriteRenderer>();
			idleHand = character.FindChild("Hands").FindChild("IdleHand").GetComponent <SpriteRenderer> ();
			holdingItem = holdingHand.transform.FindChild("HoldingItem").GetComponent <SpriteRenderer> ();
		}

		public void Update(Profession profession) {
			body.sprite = profession.body;
			if (GameData.GetChosenGender () == GameData.Gender.MALE) {
				head.sprite = profession.maleHead;
			} else {
				head.sprite = profession.femaleHead;
			}
			holdingHand.sprite = profession.arm;
			idleHand.sprite = profession.arm;
			leg1.sprite = profession.leg;
			leg2.sprite = profession.leg;
			if (profession.initialObjects != null) 
				if (profession.initialObjects.Length >= 1) 
					holdingItem.sprite = profession.initialObjects [0].uiSlotContent.itemIcon;
		}

		public void Reset() {
			body.sprite = null;
			head.sprite = null;
			leg1.sprite = null;
			leg2.sprite = null;
			holdingHand.sprite = null;
			idleHand.sprite = null;
			holdingItem.sprite = null;
		}
	}

	//Title
	private Text title;
	//Choice 1
	private Text description1;
	private CharacterSprites character1;
	//Choice 2
	private Text description2;
	private CharacterSprites character2;

	//The professions that are defined by the method.  
	private Profession currentProfession1 = null, currentProfession2 = null, chosenProfession = null;
	private PlayerCostumeManager mainPlayerCostumeManager;

	//Initialization
	void InitializeProfessionChoiceComponents() {
		//The UI components.  
		title = transform.FindChild ("Title").GetComponent <Text> ();
		description1 = transform.FindChild("Choice 1").FindChild ("Description").GetComponent <Text> ();
		description2 = transform.FindChild("Choice 2").FindChild ("Description").GetComponent <Text> ();

		//Find and reference all of the character sprites.  
		character1 = new CharacterSprites(transform.FindChild("Choice 1").FindChild("Character"));
		character2 = new CharacterSprites(transform.FindChild("Choice 2").FindChild("Character"));

		//Hide the profession manager (had to be active for initialization).  
		gameObject.SetActive (false);
	}

	//Used when a profession choice occurs.  
	public IEnumerator CreateProfessionChoice(string titleText, Profession profession1, string d1, Profession profession2, string d2) {
		//Create the panel.  
		title.text = titleText;
		currentProfession1 = profession1;
		currentProfession2 = profession2;
		description1.text = d1;
		description2.text = d2;

		//Set all of the profession character sprites and make sure that the animation speed for each is 0.  . 
		character1.Update(profession1);
		character2.Update (profession2);

		//Set the profession choice visible after initialization is complete.  
		gameObject.SetActive (true);

		//Wait until the profession has been chosen.  
		while (chosenProfession == null) {
			yield return null;
		}
	}

	//Used when a profession has been chosen.  
	public void ResetProfessionChoice(int chosen) {
		//Update player costume with new profession.
		Profession chosenProfessionTemp = chosen == 1 ? currentProfession1 : currentProfession2;
		//Reset variables.  
		currentProfession1 = null;
		currentProfession2 = null;
		character1.Reset ();
		description1.text = "";
		character2.Reset ();
		description2.text = "";
		gameObject.SetActive (false);
		//Resume game.
		chosenProfession = chosenProfessionTemp;
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

	public Profession GetChosenProfession() {
		Profession toReturn = chosenProfession;
		//To avoid confusion during the next level.  
		chosenProfession = null;
		return toReturn;
	}

}
