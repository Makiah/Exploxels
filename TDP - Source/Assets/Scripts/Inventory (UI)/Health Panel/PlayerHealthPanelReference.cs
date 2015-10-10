using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthPanelReference : HealthPanelReference {

	Slider experienceSlider;
	Text playerName;
	Text playerLevel;
	int maxExpValue = 10;
	int currentLevel = 1;

	//The inherited base constructor is required, otherwise the constructor does not work.  
	public PlayerHealthPanelReference(Transform objectToReference, UIHealthController ctorMasterController) : base(objectToReference, ctorMasterController) {
		panel = objectToReference.gameObject;
		headIcon = objectToReference.FindChild ("Icon").gameObject.GetComponent <Image> ();
		healthBar = objectToReference.FindChild ("Health Bar").gameObject.GetComponent <Slider> ();
		playerName = objectToReference.FindChild ("Name").gameObject.GetComponent <Text> ();
		playerLevel = objectToReference.FindChild ("PlayerLevel").gameObject.GetComponent <Text> ();
		masterController = ctorMasterController;

		playerName.text = GameObject.Find ("UI Data").GetComponent <UIData> ().specifiedPlayerName;
		experienceSlider = objectToReference.FindChild ("Experience Indicator").gameObject.GetComponent <Slider> ();
		experienceSlider.maxValue = maxExpValue;
		experienceSlider.value = 0;
	}


	public int UpdateExperience (int currentExp) {
		if (currentExp >= experienceSlider.maxValue) {
			int valueForNextSlider = currentExp - (int) experienceSlider.maxValue;
			currentLevel++;
			//Do some level-up thing here
			playerLevel.text = "Player Level: " + currentLevel;
			experienceSlider.value = valueForNextSlider;
			experienceSlider.maxValue = experienceSlider.maxValue += 2;
			Debug.Log ("Incremented experience bar");
			return valueForNextSlider;
		} else {
			experienceSlider.value = currentExp;
			Debug.Log ("Set experience to " + currentExp);
			return currentExp;
		}
	}

}
