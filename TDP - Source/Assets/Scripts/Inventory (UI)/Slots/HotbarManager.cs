
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
		LevelEventManager.InitializeHotbarItems += AssignInitialHotbarItems;
	}

	void OnDisable() {
		LevelEventManager.InitializeHotbarItems -= AssignInitialHotbarItems;
	}


	/************************************************** HOTBAR MANAGEMENT **************************************************/


	GameObject playerObject;
	PlayerCostumeManager playerCostumeManager;


	HotbarSlotScript[] hotbarSlots; 

	public ResourceReference currentlySelected;

	int currentlyActiveSlot;
	int previouslyActiveSlot;

	void AssignInitialHotbarItems(Race race) {

		playerObject = GameObject.Find ("ManagementFrameworks").transform.FindChild ("GameVariables").gameObject.GetComponent <VariableManagement> ().GetPlayerReference ();
		playerCostumeManager = playerObject.transform.FindChild ("FlippingItem").FindChild ("Player").GetComponent <PlayerCostumeManager>();
		hotbarSlots = new HotbarSlotScript[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			hotbarSlots[i] = transform.GetChild(i).GetComponent <HotbarSlotScript> ();
			hotbarSlots[i].masterHotbarManager = this;
		}
		StartCoroutine ("CheckForActiveItemKey");

		if (race.initialObjects != null) {
			for (int i = 0; i < race.initialObjects.Length; i++) {
				hotbarSlots [i].AssignNewItem (new UISlotContentReference(race.initialObjects [i], 1));
			}
		} else
			Debug.Log ("No initial items were found");

		previouslyActiveSlot = -1;
		currentlyActiveSlot = 0;
		UpdateSelectedItem ();

	}


	IEnumerator CheckForActiveItemKey() {
		while (true) {
			if (playerObject.GetComponent <PlayerAction> ().CheckCurrentAttackAnimationState () != true) {
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					if (previouslyActiveSlot != 0) {
						currentlyActiveSlot = 0;
						UpdateSelectedItem ();
						previouslyActiveSlot = 0;
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
					if (previouslyActiveSlot != 1) {
						currentlyActiveSlot = 1;
						UpdateSelectedItem ();
						previouslyActiveSlot = 1;
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
					if (previouslyActiveSlot != 2) {
						currentlyActiveSlot = 2;
						UpdateSelectedItem ();
						previouslyActiveSlot = 2;
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
					if (previouslyActiveSlot != 3) {
						currentlyActiveSlot = 3;
						UpdateSelectedItem ();
						previouslyActiveSlot = 3;
					}
				} else if (Input.GetKeyDown (KeyCode.Alpha5)) {
					if (previouslyActiveSlot != 4) {
						currentlyActiveSlot = 4;
						UpdateSelectedItem ();
						previouslyActiveSlot = 4;
					}
				}
			}

			yield return null;
		}
	}

	void DisableHotbarSelections() {
		for (int i = 0; i < hotbarSlots.Length; i++) {
			hotbarSlots[i].DisableSelection();
		}
	}

	public void UpdateSelectedItem() {
		DisableHotbarSelections ();
		hotbarSlots [currentlyActiveSlot].SetSelectionActive ();

		if (hotbarSlots [currentlyActiveSlot].GetCurrentlyAssigned() != null) {
			playerCostumeManager.UpdatePlayerItem (hotbarSlots [currentlyActiveSlot].GetCurrentlyAssigned().uiSlotContent.holdingPrefab);
		} else {
			playerCostumeManager.UpdatePlayerItem(null);
		}

	}

}
