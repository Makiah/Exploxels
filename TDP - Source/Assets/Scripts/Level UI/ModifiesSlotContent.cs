using UnityEngine;
using System.Collections;

public class ModifiesSlotContent : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/
	
	protected virtual void OnEnable() {
		LevelEventManager.InitializeSlotControlSystem += InitializeSystem;
	}
	
	protected virtual void OnDisable() {
		LevelEventManager.InitializeSlotControlSystem -= InitializeSystem;
	}
	
	
	/************************************************** DROP HANDLER **************************************************/
	
	static SlotScript[,] slotArray;
	static bool initialized = false;

	//Used when called from LevelEventManager.  
	void InitializeSystem(SlotScript[,] slots) {
		slotArray = slots;
		initialized = true;
	}

	public static bool IsInitialized() {
		return initialized;
	}

	//Assigns a new item to the best possible slot.  
	public static bool AssignNewItemToBestSlot(UISlotContentReference item) {

		//Has to be here for the return statement
		bool successfullyAssigned = false;

		//Make sure that the prerequisites are met.  
		if (initialized && item != null) {
			SlotScript bestAvailableSlot = FindBestAvailableSlot (item);
		
			if (bestAvailableSlot != null) {
				//Set successfully assigned.  
				successfullyAssigned = true;
				//Add the new stack to the current item stack.  
				bestAvailableSlot.ModifyCurrentItemStack (item.stack);
				Debug.Log ("Assigned " + item.uiSlotContent.itemScreenName + " to slot with items of same type.");
			} else {
				Debug.Log ("Could not stack item: Attempting to add to an empty slot");
				bestAvailableSlot = FindBestAvailableNullSlot ();
				if (bestAvailableSlot != null) {
					successfullyAssigned = true;
					bestAvailableSlot.AssignNewItem (item);
					//Update the hotbar item.
					CurrentLevelVariableManagement.GetLevelUIReference ().transform.FindChild ("Hotbar").GetComponent <HotbarManager> ().UpdateSelectedItem ();
				} else {
					Debug.LogError("No slots are empty!");
				}
			}
		} else {
			Debug.LogError("Could not modify slot content, not initialized or attempted item to assign is null.  ");
		}

		return successfullyAssigned;
	}

	//Searches for the best available slot in the slot array.  (One that already has the specified item)
	public static SlotScript FindBestAvailableSlot(UISlotContentReference pendingObjectToCheck) {
		if (slotArray != null) {
			for (int y = slotArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotArray.GetLength(1); x++) {
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
	public static SlotScript FindBestAvailableNullSlot() {
		if (slotArray != null) {
			for (int y = slotArray.GetLength(0) - 1; y >= 0; y--) {
				//If no stackable slot is found, choose an empty slot.  
				for (int x = 0; x < slotArray.GetLength(1); x++) {
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
	public static SlotScript DetermineWhetherPlayerHasCertainInventoryItem(UISlotContentReference pendingObjectToCheck) {
		if (slotArray != null) {
			for (int y = slotArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotArray.GetLength(1); x++) {
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
