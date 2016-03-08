
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
		LevelEventManager.InitializeCostume += InitializeSpriteChildren;
	}
	
	void OnDisable() {
		LevelEventManager.InitializeCostume -= InitializeSpriteChildren;
	}


	/************************************************ COSTUME MANAGEMENT ************************************************/

	//Main player action
	private PlayerAction mainPlayerAction;
	//SpriteRenderer child components
	private SpriteRenderer head, body, idleArm, holdingArm, topLeg, bottomLeg;
	//The prefab of the item will be childed to this object.  
	private Transform holdingItem;

	private InventoryFunctions playerInventory;

	void InitializeSpriteChildren() {
		mainPlayerAction = transform.parent.parent.gameObject.GetComponent <PlayerAction> ();
		playerInventory = CurrentLevelVariableManagement.GetMainInventoryReference ().GetComponent <InventoryFunctions> ();
		//Just setting up the basic race costume.  
		body = transform.FindChild("Body").GetComponent <SpriteRenderer> ();
		head = transform.FindChild ("Head").GetComponent <SpriteRenderer> ();
		idleArm = transform.FindChild ("Hands").FindChild ("IdleHand").GetComponent <SpriteRenderer> ();
		holdingArm = transform.FindChild ("Hands").FindChild ("HoldingHand").GetComponent <SpriteRenderer> ();
		topLeg = transform.FindChild ("Legs").FindChild ("Top Leg").GetComponent <SpriteRenderer> ();
		bottomLeg = transform.FindChild ("Legs").FindChild("Bottom Leg").GetComponent <SpriteRenderer> ();
		holdingItem = holdingArm.transform.FindChild ("HoldingItem");

		Profession currentPlayerProfession = GameData.GetPlayerProfession ();
		UpdatePlayerProfession (currentPlayerProfession);
	}

	//Used when a player profession is changed.  
	public void UpdatePlayerProfession(Profession profession) {
		//Update with common gender sprites
		body.sprite = profession.body;
		idleArm.sprite = profession.arm;
		holdingArm.sprite = profession.arm;
		topLeg.sprite = profession.leg;
		bottomLeg.sprite = profession.leg;

		//Gender check.  
		if (GameData.GetChosenGender() == GameData.Gender.MALE) {
			head.sprite = profession.maleHead;
		} else {
			head.sprite = profession.femaleHead;
		}

		//Add the initial items for the profession to the inventory.  
		for (int i = 0; i < profession.initialObjects.Length; i++) {
			playerInventory.AssignNewItemToBestSlot(profession.initialObjects[i]);
		}
	}

	//Called by HotbarManager when a new hotbar item is selected.
	public void UpdatePlayerItem(GameObject prefabSelectedInHotbar) {

		//Deletes the previous item that had existed before this new item.  
		if (holdingItem.childCount != 0) {
			if (holdingItem.childCount > 1) {
				Debug.Log("There was more than one object being held by the player.");
			}

			for (int i = 0; i < holdingItem.childCount; i++) {
				Destroy (holdingItem.GetChild (i).gameObject);
			}
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

