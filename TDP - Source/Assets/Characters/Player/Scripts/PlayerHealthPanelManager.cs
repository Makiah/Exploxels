
/*
 * Author: Makiah Bennett
 * Created 16 September 2015
 * Last edited: 8 October 2015
 * 
 * 9/18 - Created override to OnDeath, so that app quits on player death.  
 * 
 * 10/8 - Functionality should be added to this script so that it also controls the experience progress indicator on the health panel.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthPanelManager : CharacterHealthPanelManager {

	protected override void OnEnable() {
		LevelEventManager.InitializePlayer += InitializeHealthBar;
	}

	protected override void OnDisable() {
		LevelEventManager.InitializePlayer -= InitializeHealthBar;
	}
	
	/************************* HEALTH MANAGER *************************/

	int currentExp = 0;

	PlayerHealthPanelReference playerHealthPanelReference;

	public override void InitializeHealthBar() {
		if (lifePoints <= 0) {
			Debug.Log ("Player health is " + lifePoints + " which is an invalid value.  Switching to 10.");
			lifePoints = 10;
		}
		currentHealth = lifePoints;
		//Create panel
		uiHealthController = CurrentLevelVariableManagement.GetLevelUIReference().transform.FindChild ("Health Controller").gameObject.GetComponent <UIHealthController> (); 
		playerHealthPanelReference = uiHealthController.GetPlayerHealthPanelReference ();
		//Initialize icon
		characterHeadSprite = transform.FindChild ("FlippingItem").GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
		playerHealthPanelReference.InitializePanel (characterHeadSprite, lifePoints, currentHealth);

		//Give player money obtained previously.  
		GiveMoneyToPlayer (GameData.GetPlayerMoney());
	}

	//Called by PlayerDropHandler.  
	public void OnExperienceNodulePickedUp() {
		currentExp ++;
		//Done in case the experience level is incremented past the maximum.  
		currentExp = playerHealthPanelReference.UpdateExperience (currentExp);
	}

	public void OnCoinPickedUp(int value) {
		playerHealthPanelReference.UpdateCoinValue(value);
	}

	public override void YouHaveBeenAttacked(float lifePointDeduction) {
		currentHealth -= lifePointDeduction;
		if (playerHealthPanelReference != null) 
			playerHealthPanelReference.UpdateHealth (currentHealth);
		if (currentHealth <= 0) {
			OnDeath();
		}
	}

	public string GetPlayerName() {
		return playerHealthPanelReference.GetPlayerName ();
	}

	public bool GiveMoneyToPlayer(int amount) {
		return playerHealthPanelReference.UpdateCoinValue (amount);
	}

	public int GetPlayerMoney() {
		return playerHealthPanelReference.GetCoinAmount ();
	}

	protected override void OnDeath() {
		playerHealthPanelReference.Clear ();
		//Note: Application.Quit() does not work for the Web Player or the Unity Editor.  
		//Application.Quit ();
		//The following does work for the editor.  
		Debug.Log ("Quitting the game");
		ScriptingUtilities.QuitGame ();
		Destroy (this.gameObject);
	}
}
