
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script pretty much just sends a continual linecast in front of the player, and if an object is found and matches the specified
 * criteria, the script will then assign that item to the best available slot.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class DropHandler : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/

	void OnEnable() {
		LevelEventManager.InitializePlayerDropSystem += InitializeSystem;
	}

	void OnDisable() {
		LevelEventManager.InitializePlayerDropSystem -= InitializeSystem;
	}


	/************************************************** DROP HANDLER **************************************************/

	public SlotScript[,] slotArray;
	bool initialized = false;

	void InitializeSystem(SlotScript[,] slots) {
		slotArray = slots;
		initialized = true;
	}
	
	//When an item drop hits the player.  
	void OnTriggerEnter2D(Collider2D externalTrigger) {
		if (externalTrigger.gameObject.GetComponent <DroppedItemProperties> () != null && initialized) 
			PickupItem (externalTrigger.gameObject);
	}

	public void PickupItem(GameObject item) {
		//This does not check the resourcereference property of the attached script as a comparison, only the tag.  Consider changing later.  
		if (!(item.CompareTag ("ExpNodule"))) {

			UISlotContentReference pendingObject = new UISlotContentReference (item.GetComponent <DroppedItemProperties> ().localResourceReference, 1);
			SlotScript bestAvailableSlot = FindBestAvailableSlot (slotArray, pendingObject);

			if (bestAvailableSlot != null) {
				bestAvailableSlot.ModifyCurrentItemStack (1);
				Debug.Log ("Assigned " + item.GetComponent <DroppedItemProperties> ().localResourceReference.itemScreenName + " to best available slot.");
				Destroy (item);
			} else {
				Debug.Log ("Could not stack item: Attempting null slot");
				bestAvailableSlot = FindBestAvailableNullSlot (slotArray);
				if (bestAvailableSlot != null) {
					bestAvailableSlot.AssignNewItem (pendingObject);
					Destroy (item);
				}
			}
		} else {
			transform.parent.gameObject.GetComponent <PlayerHealthPanelManager> ().OnExperienceNodulePickedUp(1);
			Destroy(item);
		}
	}

	//Searches fot the best available slot in the slot array.  
	SlotScript FindBestAvailableSlot(SlotScript[,] slotScriptArray, UISlotContentReference pendingObjectToCheck) {
		if (slotScriptArray != null) {
			for (int y = slotScriptArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotScriptArray.GetLength(1); x++) {
					if (slotArray [y, x].GetCurrentlyAssigned () != null) {
						if (slotArray [y, x].GetCurrentlyAssigned ().uiSlotContent.itemType == pendingObjectToCheck.uiSlotContent.itemType) {
							if (slotArray [y, x].GetCurrentlyAssigned ().uiSlotContent.localGroupID == pendingObjectToCheck.uiSlotContent.localGroupID) {
								return slotArray [y, x];
							}
						}
					}
				}
			}
		} else {
			Debug.Log("Slot array is null");
		}

		return null;
	}

	SlotScript FindBestAvailableNullSlot(SlotScript[,] slotScriptArray) {
		if (slotScriptArray != null) {
			for (int y = slotScriptArray.GetLength(0) - 1; y >= 0; y--) {
				//If no stackable slot is found, choose an empty slot.  
				for (int x = 0; x < slotScriptArray.GetLength(1); x++) {
					if (slotArray [y, x].GetCurrentlyAssigned () == null) {
						return slotArray [y, x];
					}
				}
			}
		} else {
			Debug.Log("Slot array is null");
		}

		return null;
	}

}
