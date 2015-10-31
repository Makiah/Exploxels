using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthPanelReference : HealthPanelReference {

	Slider experienceSlider;
	Text playerName;
	Text playerLevel;
	int maxExpValue = 10;
	int currentLevel = 1;

	//The method that initializes the values of the health panel.  
	protected override void InitializeHealthPanelReference() {
		base.InitializeHealthPanelReference ();

		//Additional components of the panel
		playerName = transform.FindChild ("Name").gameObject.GetComponent <Text> ();
		playerLevel = transform.FindChild ("PlayerLevel").gameObject.GetComponent <Text> ();
		//Set panel initial values.  
		playerName.text = GameObject.Find ("UI Data").GetComponent <UIData> ().specifiedPlayerName;
		experienceSlider = transform.FindChild ("Experience Indicator").gameObject.GetComponent <Slider> ();
		experienceSlider.maxValue = maxExpValue;
		experienceSlider.value = 0;
	}

	public string GetPlayerName() {
		return playerName.text;
	}

	//The method that controls the values of the experience bar.  
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
			return currentExp;
		}
	}

}
