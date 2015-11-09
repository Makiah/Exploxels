using UnityEngine;
using System.Collections;

public abstract class ModifiesSlotContent : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/
	
	protected virtual void OnEnable() {
		LevelEventManager.InitializeSlotControlSystem += InitializeSystem;
	}
	
	protected virtual void OnDisable() {
		LevelEventManager.InitializeSlotControlSystem -= InitializeSystem;
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

	protected SlotScript CheckWhetherPlayerHasCertainItem(UISlotContentReference certainItem) {
		return DetermineWhetherPlayerHasCertainInventoryItem (slotArray, certainItem);
	}

	//Searches for the best available slot in the slot array.  (One that already has the specified item)
	SlotScript FindBestAvailableSlot(SlotScript[,] slotScriptArray, UISlotContentReference pendingObjectToCheck) {
		if (slotScriptArray != null) {
			for (int y = slotScriptArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotScriptArray.GetLength(1); x++) {
					//Define the object in the slot. 
					UISlotContentReference objectAssigned = slotArray[y, x].GetCurrentlyAssigned();
					//Check to make sure objectAssigned is not null.  
					if (objectAssigned != null)
						//Check to make sure the item is the same.  
						if (objectAssigned.uiSlotContent.itemType == pendingObjectToCheck.uiSlotContent.itemType)
							if (objectAssigned.uiSlotContent.localGroupID == pendingObjectToCheck.uiSlotContent.localGroupID)
								//Since the slot fits all requirements, return the slot.  
								return slotArray [y, x];
				}
			}
		} else {
			Debug.Log("Slot array is null and initialized status is " + initialized);
		}
		
		return null;
	}

	//Find the best available empty slot.  
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

	//Used to determine whether the player has a required item.  
	SlotScript DetermineWhetherPlayerHasCertainInventoryItem(SlotScript[,] slotScriptArray, UISlotContentReference pendingObjectToCheck) {
		if (slotScriptArray != null) {
			for (int y = slotScriptArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotScriptArray.GetLength(1); x++) {
					//Define the item that is in the specified slot.  
					UISlotContentReference objectAssigned = slotArray[y, x].GetCurrentlyAssigned();
					//Check whether the assigned object is null.  
					if (objectAssigned != null)
						//Check to make sure the item types are the same.  
						if (objectAssigned.uiSlotContent.itemType == pendingObjectToCheck.uiSlotContent.itemType)
							//Check to see that the IDs are the same.  
							if (objectAssigned.uiSlotContent.localGroupID == pendingObjectToCheck.uiSlotContent.localGroupID)
								//Check to see that the stacks are greater or equal to one another.  
								if (objectAssigned.stack >= pendingObjectToCheck.stack)
									//Since the slot fits all requirements, return the slot.  
									return slotArray [y, x];
				}
			}
		} else {
			Debug.Log("Slot array is null");
		}
		
		return null;
	}

}
