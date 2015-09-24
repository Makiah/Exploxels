
/*
 * Author: Makiah Bennett
 * Created 14 September 2015
 * Last edited: 14 September 2015
 * 
 * 9/14 - Created as a base to all enemies / player that will be affected by swords as well as other items.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterHealthController : MonoBehaviour {

	/************************* INITIALIZATION *************************/

	protected virtual void OnEnable() {
		LevelEventManager.InitializeEnemyHealthControllers += InitializeHealthBar;
	}

	protected virtual void OnDisable() {
		LevelEventManager.InitializeEnemyHealthControllers -= InitializeHealthBar;
	}

	/************************* HEALTH MANAGER *************************/
	
	public float lifePoints;
	protected float currentHealth;

	protected UIHealthController uiHealthController;
	protected HealthPanelReference healthPanelReference;
	protected Sprite characterHeadSprite;

	public void YouHaveBeenAttacked(float lifePointDeduction) {
		Debug.Log ("Attack received " + lifePointDeduction);
		currentHealth -= lifePointDeduction;
		if (healthPanelReference != null) 
			healthPanelReference.Update (currentHealth);
		if (currentHealth <= 0) {
			OnDeath();
		}
	}

	//Look into initializing this once the player comes into activation distance.  
	protected virtual void InitializeHealthBar() {
		lifePoints = 3f;
		currentHealth = lifePoints;
		//Create panel
		uiHealthController = GameObject.Find ("Inventory (UI)").transform.FindChild ("Health Controller").gameObject.GetComponent <UIHealthController> (); 
		//Initialize icon
		characterHeadSprite = transform.GetChild (0).GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
	}

	public void OnThisEnemyActivated() {
		healthPanelReference = uiHealthController.GetEnemyHealthPanelReference (this);
		if (healthPanelReference != null)
			healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);
	}

	public void OnThisEnemyDeActivated() {
		DisableHealthPanel ();
	}

	public void HealthPanelNewlyAvailable(HealthPanelReference healthPanel) {
		healthPanelReference = healthPanel;
		healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);
	}

	public void DisableHealthPanel() {
		if (healthPanelReference != null) {
			healthPanelReference.Reset ();
			healthPanelReference = null;
		}
	}

	protected virtual void OnDeath() {
		DisableHealthPanel();
		Destroy (this.gameObject);
	}

	public bool GetHealthPanelState() {
		return healthPanelReference == null ? false : true;
	}


}
