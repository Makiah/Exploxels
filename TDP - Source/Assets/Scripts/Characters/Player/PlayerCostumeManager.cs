
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the initial player setup, such as instantiating the race components on to the player itself, then 
 * updating the item in use by the player.  This script controls item switching, as well as armor, etc.  Added by the Hotbar
 * Manager.  
 * 
 */


using UnityEngine;
using System.Collections;

public class PlayerCostumeManager : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/

	void OnEnable() {
		LevelEventManager.InitializeCostume += InitializePlayerCostume;
	}
	
	void OnDisable() {
		LevelEventManager.InitializeCostume -= InitializePlayerCostume;
	}


	/************************************************ COSTUME MANAGEMENT ************************************************/

	private CharacterBaseActionClass mainPlayerAction;

	private SpriteRenderer head;
	private SpriteRenderer body;
	private SpriteRenderer idleArm;
	private SpriteRenderer holdingArm;
	private SpriteRenderer topLeg;
	private SpriteRenderer bottomLeg;
	//The prefab of the item will be childed to this object.  
	private Transform holdingItem;

	void InitializePlayerCostume(Race race) {
		mainPlayerAction = transform.parent.parent.gameObject.GetComponent <CharacterBaseActionClass> ();
		//Just setting up the basic race costume.  
		body = transform.FindChild("Body").GetComponent <SpriteRenderer> ();
		head = transform.FindChild ("Head").GetComponent <SpriteRenderer> ();
		idleArm = transform.FindChild ("Hands").FindChild ("IdleHand").GetComponent <SpriteRenderer> ();
		holdingArm = transform.FindChild ("Hands").FindChild ("HoldingHand").GetComponent <SpriteRenderer> ();
		topLeg = transform.FindChild ("Legs").FindChild ("Top Leg").GetComponent <SpriteRenderer> ();
		bottomLeg = transform.FindChild ("Legs").FindChild("Bottom Leg").GetComponent <SpriteRenderer> ();
		holdingItem = transform.FindChild("Hands").FindChild("HoldingHand").FindChild ("HoldingItem");

		body.sprite = race.body;
		head.sprite = race.heads[race.headVariationIndex];
		idleArm.sprite = race.arm;
		holdingArm.sprite = race.arm;
		topLeg.sprite = race.legs;
		bottomLeg.sprite = race.legs;
	}

	//Called by HotbarManager when a new hotbar item is selected.
	public void UpdatePlayerItem(GameObject prefabSelectedInHotbar) {

		//Deletes the previous item that had existed before this new item.  
		if (holdingItem.childCount != 0 && holdingItem != null) {
			Destroy (holdingItem.GetChild (0).gameObject);
		}

		//Instantiating the new item (even if the new item is null).  
		if (prefabSelectedInHotbar != null) {
			GameObject createdItem = (GameObject)Instantiate (prefabSelectedInHotbar);
			createdItem.transform.SetParent (holdingItem);
			createdItem.transform.localPosition = Vector2.zero; 
			createdItem.transform.localScale = new Vector3(prefabSelectedInHotbar.transform.localScale.x, prefabSelectedInHotbar.transform.localScale.y, 1);//transform.parent.localScale * createdItem.transform.localScale;
			createdItem.transform.localRotation = transform.parent.localRotation;

			if (prefabSelectedInHotbar.GetComponent <ItemBase> () != null) {
				prefabSelectedInHotbar.GetComponent <ItemBase> ().SetAttachedCharacterInput (mainPlayerAction);
				mainPlayerAction.OnRefreshCurrentWeaponMoves (prefabSelectedInHotbar.GetComponent <ItemBase> ());
			} else {
				mainPlayerAction.OnRefreshCurrentWeaponMoves (null);
			}

		} else {
			mainPlayerAction.OnRefreshCurrentWeaponMoves(null);
		}


	}

}

