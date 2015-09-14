
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
		EventManager.InitializePickupSystem += InitializeSystem;
	}

	void OnDisable() {
		EventManager.InitializePickupSystem -= InitializeSystem;
	}


	/************************************************** DROP HANDLER **************************************************/

	public SlotScript[,] slotArray;

	void InitializeSystem(SlotScript[,] slots) {
		slotArray = slots;
		StartCoroutine ("CheckForItemsInProximity");
	}

	IEnumerator CheckForItemsInProximity() {
		while (true) {
			Vector3 startPoint = gameObject.transform.position - new Vector3(0, .75f, 0);
			Vector3 endPoint = gameObject.transform.position - new Vector3(0, .75f, 0) + new Vector3(5, 0, 0) * gameObject.GetComponent <PlayerAction> ().GetFacingDirection();

			RaycastHit2D linecastResult = Physics2D.Linecast(startPoint, endPoint, 1 << LayerMask.NameToLayer ("Drops"));
			//Debug.DrawLine(startPoint, endPoint);

			if (linecastResult.collider != null) {
				Debug.Log ("ItemCheck hit collider with name of " + linecastResult.collider.gameObject.name + ".");
				if (linecastResult.collider.gameObject.GetComponent <DroppedItemProperties> () != null)
					PickupItem(linecastResult.collider.gameObject);
			}

			yield return null;
		}
	}

	public void PickupItem(GameObject item) {
		UISlotContentReference pendingObject = new UISlotContentReference (item.GetComponent <DroppedItemProperties> ().localResourceReference, 1);
		SlotScript bestAvailableSlot = AttemptToStackOnBestAvailableSlot (slotArray, pendingObject);

		if (bestAvailableSlot != null) {
			bestAvailableSlot.ModifyCurrentItemStack (1);
			Debug.Log("Assigned " + item.GetComponent <DroppedItemProperties> ().localResourceReference.itemScreenName + " to best available slot.");
			Destroy (item);
		} else {
			Debug.Log("Could not stack item: Attempting null slot");
			bestAvailableSlot = AttemptToPlaceOnBestAvailableNullSlot(slotArray);
			if (bestAvailableSlot != null) {
				bestAvailableSlot.AssignNewItem(pendingObject);
				Destroy (item);
			}
		}
	}

	SlotScript AttemptToStackOnBestAvailableSlot(SlotScript[,] slotScriptArray, UISlotContentReference pendingObjectToCheck) {
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

	SlotScript AttemptToPlaceOnBestAvailableNullSlot(SlotScript[,] slotScriptArray) {
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
