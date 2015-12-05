
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 *  
 * 
 * This script controls the 1, 2, 3, 4, and 5 keys, in order to switch items from the hotbar into use.  It does so by using a coroutine that checks
 * whether or not the player is in an animation state, and if not, checking for Alpha1, 2, etc.  This is sent to the PlayerCostumeManager script, 
 * which actually instantiates the item.  
 * 
 * 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/

	void OnEnable() {
		LevelEventManager.InitializeHotbarManager += InitializeHotbarManager;
	}

	void OnDisable() {
		LevelEventManager.InitializeHotbarManager -= InitializeHotbarManager;
	}


	/************************************************** HOTBAR MANAGEMENT **************************************************/

	GameObject playerObject;
	PlayerCostumeManager playerCostumeManager;
	
	HotbarSlotScript[] hotbarSlots; 

	public ResourceReference currentlySelected;

	//When a user enters the same key twice, there is no need for it to update the item again.  
	int currentlyActiveSlot;
	int previouslyActiveSlot;

	//Called by LevelEventManager.  
	void InitializeHotbarManager() {
		//Define player and hotbar slots.  
		playerObject = CurrentLevelVariableManagement.GetPlayerReference ();
		playerCostumeManager = playerObject.transform.FindChild ("FlippingItem").FindChild ("Player").GetComponent <PlayerCostumeManager>();
		hotbarSlots = new HotbarSlotScript[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			hotbarSlots[i] = transform.GetChild(i).GetComponent <HotbarSlotScript> ();
			hotbarSlots[i].masterHotbarManager = this;
		}
		
		previouslyActiveSlot = -1;
		currentlyActiveSlot = 0;

		StartCoroutine (CheckForActiveItemKey());
	}

	//Used for detecting number keys.  
	IEnumerator CheckForActiveItemKey() {
		while (true) {
			if (playerObject.GetComponent <PlayerAction> ().CheckCurrentAttackAnimationState () != true) {
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					if (previouslyActiveSlot != 0) {
						currentlyActiveSlot = 0;
						UpdateSelectedItem ();
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
					if (previouslyActiveSlot != 1) {
						currentlyActiveSlot = 1;
						UpdateSelectedItem ();
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
					if (previouslyActiveSlot != 2) {
						currentlyActiveSlot = 2;
						UpdateSelectedItem ();
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
					if (previouslyActiveSlot != 3) {
						currentlyActiveSlot = 3;
						UpdateSelectedItem ();
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
					if (previouslyActiveSlot != 4) {
						currentlyActiveSlot = 4;
						UpdateSelectedItem ();
					}
				}
			}

			yield return null;
		}
	}

	//Called by ModifiesSlotContent and locally.  
	public void UpdateSelectedItem() {
		// Disable selection on previous item.  
		if (previouslyActiveSlot != -1)
			hotbarSlots [previouslyActiveSlot].DisableSelection ();
		//Set the selection on the currently active slot.  
		hotbarSlots [currentlyActiveSlot].SetSelectionActive ();
		//Set variable stuff
		previouslyActiveSlot = currentlyActiveSlot;

		//Update the player's item
		if (hotbarSlots [currentlyActiveSlot].GetCurrentlyAssigned() != null) {
			playerCostumeManager.UpdatePlayerItem (hotbarSlots [currentlyActiveSlot].GetCurrentlyAssigned().uiSlotContent.holdingPrefab);
		} else {
			playerCostumeManager.UpdatePlayerItem(null);
		}
	}

	//For instance: when an item is used.  
	public void ModifyStackOfSelectedItem(int stackToRemove) {
		//Error checking.  
		if (hotbarSlots [currentlyActiveSlot].GetCurrentlyAssigned () != null) {
			//Remove item
			hotbarSlots [currentlyActiveSlot].ModifyCurrentItemStack (-stackToRemove);
			UpdateSelectedItem();
			
		} else {
			Debug.LogError("Cannot change stack of null!!!");
		}
	}

}
