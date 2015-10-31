using UnityEngine;
using System.Collections;

public abstract class ModifiesSlotContent : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/
	
	protected virtual void OnEnable() {
		LevelEventManager.InitializePlayerDropSystem += InitializeSystem;
	}
	
	protected virtual void OnDisable() {
		LevelEventManager.InitializePlayerDropSystem -= InitializeSystem;
	}
	
	
	/************************************************** DROP HANDLER **************************************************/
	
	public SlotScript[,] slotArray;
	protected bool initialized = false;
	
	void InitializeSystem(SlotScript[,] slots) {
		slotArray = slots;
		initialized = true;
	}

	protected bool AssignNewItemToBestSlot(UISlotContentReference item) {
		bool successfullyAssigned = false;
		SlotScript bestAvailableSlot = FindBestAvailableSlot (slotArray, item);
		
		if (bestAvailableSlot != null) {
			successfullyAssigned = true;
			bestAvailableSlot.ModifyCurrentItemStack (1);
			Debug.Log ("Assigned " + item.uiSlotContent.itemScreenName + " to slot with items of same type.");
		} else {
			Debug.Log ("Could not stack item: Attempting to add to an empty slot");
			bestAvailableSlot = FindBestAvailableNullSlot (slotArray);
			if (bestAvailableSlot != null) {
				successfullyAssigned = true;
				bestAvailableSlot.AssignNewItem (item);
			}
		}

		return successfullyAssigned;
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
