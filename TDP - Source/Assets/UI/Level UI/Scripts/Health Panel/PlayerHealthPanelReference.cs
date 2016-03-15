using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthPanelReference : HealthPanelReference {

	//Experience components
	private Text currentPlayerProfession;
	private Slider experienceSlider;
	private Text playerLevel;
	private int maxExpValue = 10;
	private int currentLevel = 1;
	//Other components
	private Text playerName;
	//Coin stuff
	private Text coinValue;
	//Mana
	private Slider manaSlider;

	//The method that initializes the values of the health panel.  
	protected override void InitializeHealthPanelReference() {
		base.InitializeHealthPanelReference ();

		//Name of player
		playerName = transform.FindChild ("Name").gameObject.GetComponent <Text> (); 
		playerName.text = GameData.GetPlayerName();
		//Experience components.  
		currentPlayerProfession = transform.FindChild ("Experience").FindChild ("ProfessionName").gameObject.GetComponent <Text> ();
		currentPlayerProfession.text = GameData.GetPlayerProfession().name;
		experienceSlider = transform.FindChild ("Experience").FindChild ("Experience Indicator").gameObject.GetComponent <Slider> ();
		playerLevel = transform.FindChild ("Experience").FindChild ("PlayerLevel").gameObject.GetComponent <Text> ();
		experienceSlider.maxValue = maxExpValue;
		experienceSlider.value = 0;
		//Coin Values
		coinValue = transform.FindChild("Cash").FindChild("Value").GetComponent <Text> ();
		coinValue.text = "0";
		//Mana values
	}

	public string GetPlayerName() {
		return playerName.text;
	}

	//The method that controls the values of the experience bar.  
	public int UpdateExperience (int currentExp) {
		Debug.Log ("Experience updated");
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

	//Add/subtract coins.  
	public bool UpdateCoinValue(int valueToAdd) {
		//In case an objective depends on this.  
		CurrentLevelVariableManagement.GetMainObjectiveManager ().OnMoneyModified (valueToAdd);
		//Determine the new coin value.  
		int newCoinValue = int.Parse (coinValue.text) + valueToAdd;
		if (newCoinValue >= 0) {
			coinValue.text = newCoinValue.ToString ();
			return true;
		} else
			return false;
	}

	//Get the total amount of coins the player has.  
	public int GetCoinAmount() {
		return int.Parse(coinValue.text);
	}

}
