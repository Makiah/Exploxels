
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

	//The player transform
	protected Transform player;

	[SerializeField]
	private float distanceUntilHealthBarActive;

	//Look into initializing this once the player comes into activation distance.  
	protected virtual void InitializeHealthBar() {
		player = VariableManagement.GetPlayerReference ().transform;
		currentHealth = lifePoints;
		//Create panel
		uiHealthController = GameObject.Find ("Inventory (UI)").transform.FindChild ("Health Controller").gameObject.GetComponent <UIHealthController> (); 
		//Initialize icon
		characterHeadSprite = transform.GetChild (0).GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
		//Start the coroutine that manages the active state of the health bar item.  
		StartCoroutine ("ControlHealthBarState");
	}

	// This coroutine controls the health bar controller.  
	IEnumerator ControlHealthBarState() {
		while (true) {
			if (Vector3.Distance(transform.position, player.position) <= distanceUntilHealthBarActive && healthPanelReference == null) {
				Debug.Log("Player entered radius");
				OnThisEnemyActivated();
			} else if (Vector3.Distance(transform.position, player.position) > distanceUntilHealthBarActive && healthPanelReference != null) {
				OnThisEnemyDeActivated();
			}

			yield return null;
		}
	}

	// On player/enemy attacked.  
	public void YouHaveBeenAttacked(float lifePointDeduction) {
		Debug.Log ("Attack received " + lifePointDeduction);
		currentHealth -= lifePointDeduction;
		if (healthPanelReference != null) 
			healthPanelReference.Update (currentHealth);
		if (currentHealth <= 0) {
			OnDeath();
		}
	}

	// Called when player enters radius of the character health controller.  
	void OnThisEnemyActivated() {
		healthPanelReference = uiHealthController.GetEnemyHealthPanelReference (this);
		if (healthPanelReference != null)
			healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);
	}

	//Called when the player exits the radius of the character health controller.  
	void OnThisEnemyDeActivated() {
		DisableHealthPanel ();
	}

	//Called by the HealthController when this object has a health panel available to use.  
	public void HealthPanelNewlyAvailable(HealthPanelReference healthPanel) {
		healthPanelReference = healthPanel;
		healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);
	}

	//Called when the object is de-activated, or on death.  
	public void DisableHealthPanel() {
		if (healthPanelReference != null) {
			healthPanelReference.Reset ();
			healthPanelReference = null;
		}
	}

	//Called when some object dies.  (currentHealth < 0)
	protected virtual void OnDeath() {
		StopCoroutine ("ControlHealthBarState");
		DisableHealthPanel();
		Destroy (this.gameObject);
	}


}
