
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
		EventManager.InitializeEnemies += InitializeHealthBar;
	}

	protected virtual void OnDisable() {
		EventManager.InitializeEnemies -= InitializeHealthBar;
	}

	/************************* HEALTH MANAGER *************************/
	
	public float lifePoints = 10f;
	protected float currentHealth;

	protected UIHealthController uiHealthController;
	protected HealthPanelReference healthPanelReference;
	protected Sprite characterHeadSprite;

	public void YouHaveBeenAttacked(float lifePointDeduction) {
		Debug.Log ("Attack received " + lifePointDeduction);
		currentHealth -= lifePointDeduction;
		if (healthPanelReference != null) 
			healthPanelReference.Update (currentHealth);
		if (lifePoints <= 0) {
			healthPanelReference.Reset();
			Destroy (this.gameObject);
		}
	}

	//Look into initializing this once the player comes into activation distance.  
	protected virtual void InitializeHealthBar() {
		currentHealth = lifePoints;
		//Create panel
		uiHealthController = GameObject.Find ("Inventory (UI)").transform.FindChild ("Health Controller").gameObject.GetComponent <UIHealthController> (); 
		healthPanelReference = uiHealthController.GetEnemyHealthPanelReference (this);
		//Initialize icon
		characterHeadSprite = transform.GetChild (0).GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;

		if (healthPanelReference != null) {
			healthPanelReference.Add(characterHeadSprite, lifePoints, currentHealth);
			if (GetComponent <EnemyBaseActionClass> () != null) {
				GetComponent <EnemyBaseActionClass> ().enemyHealthBarInitialized = true;
			}
		}
	}

	public void HealthBarNowAvailable(HealthPanelReference healthPanel) {
		healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);
		if (GetComponent <EnemyBaseActionClass> () != null) {
			GetComponent <EnemyBaseActionClass> ().enemyHealthBarInitialized = true;
		}
	}
}
