
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
		LevelEventManager.InitializeCostume += InitializePlayerProfession;
	}
	
	void OnDisable() {
		LevelEventManager.InitializeCostume -= InitializePlayerProfession;
	}


	/************************************************ COSTUME MANAGEMENT ************************************************/

	private PlayerAction mainPlayerAction;

	private SpriteRenderer head;
	private SpriteRenderer body;
	private SpriteRenderer idleArm;
	private SpriteRenderer holdingArm;
	private SpriteRenderer topLeg;
	private SpriteRenderer bottomLeg;
	//The prefab of the item will be childed to this object.  
	private Transform holdingItem;

	void InitializePlayerProfession() {
		mainPlayerAction = transform.parent.parent.gameObject.GetComponent <PlayerAction> ();
		//Just setting up the basic race costume.  
		body = transform.FindChild("Body").GetComponent <SpriteRenderer> ();
		head = transform.FindChild ("Head").GetComponent <SpriteRenderer> ();
		idleArm = transform.FindChild ("Hands").FindChild ("IdleHand").GetComponent <SpriteRenderer> ();
		holdingArm = transform.FindChild ("Hands").FindChild ("HoldingHand").GetComponent <SpriteRenderer> ();
		topLeg = transform.FindChild ("Legs").FindChild ("Top Leg").GetComponent <SpriteRenderer> ();
		bottomLeg = transform.FindChild ("Legs").FindChild("Bottom Leg").GetComponent <SpriteRenderer> ();
		holdingItem = transform.FindChild("Hands").FindChild("HoldingHand").FindChild ("HoldingItem");

		Profession currentPlayerProfession = CurrentLevelVariableManagement.GetMainGameData().chosenProfession;
		UpdatePlayerProfession (currentPlayerProfession);
	}

	//Used when a player profession is changed.  
	public void UpdatePlayerProfession(Profession profession) {
		//Gender check.  
		if (CurrentLevelVariableManagement.GetMainGameData().chosenGender == 0) {
			body.sprite = profession.male.body;
			head.sprite = profession.male.head;
			idleArm.sprite = profession.male.arm;
			holdingArm.sprite = profession.male.arm;
			topLeg.sprite = profession.male.legs;
			bottomLeg.sprite = profession.male.legs;
		} else {
			body.sprite = profession.female.body;
			head.sprite = profession.female.head;
			idleArm.sprite = profession.female.arm;
			holdingArm.sprite = profession.female.arm;
			topLeg.sprite = profession.female.legs;
			bottomLeg.sprite = profession.female.legs;
		}

		//Add the initial items for the profession to the inventory.  
		for (int i = 0; i < profession.initialObjects.Length; i++) {
			ModifiesSlotContent.AssignNewItemToBestSlot(profession.initialObjects[i]);
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

