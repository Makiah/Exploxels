using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MerchantInventoryFunctions : MonoBehaviour {

	private MerchantSlotScript[,] slotArray = new MerchantSlotScript[0,0];
	private bool initialized = false;

	//Used when called from LevelEventManager.  
	public void AddSlotsToSystem(MerchantSlotScript[,] slots) {

		//Combine the two slot arrays.  
		MerchantSlotScript[,] newSlotArray = new MerchantSlotScript[slotArray.GetLength(0) + slots.GetLength(0), slots.GetLength(1)];

		//Add the old array first.  
		for (int y = 0; y < slotArray.GetLength (0); y++) {
			for (int x = 0; x < slotArray.GetLength (1); x++) {
				newSlotArray [y, x] = slotArray [y, x];
			}
		}
		//Add the new array last.  
		for (int y = 0; y < slots.GetLength (0); y++) {
			for (int x = 0; x < slots.GetLength (1); x++) {
				newSlotArray [y + slotArray.GetLength(0), x] = slots [y, x];
			}
		}


		//Set the old slot array to the new slot array.  
		slotArray = newSlotArray;

		//Make sure that it is initialized.  
		initialized = true;
	}

	//Returns whether the script has been initialized.  
	public bool IsInitialized() {
		return initialized;
	}

	//Assigns a new item to the best possible slot.  
	public bool AssignNewItemToBestSlot(ResourceReferenceWithStackAndPrice item) {

		//Has to be here for the return statement
		bool successfullyAssigned = false;

		//Make sure that the prerequisites are met.  
		if (initialized && item != null) {
			MerchantSlotScript bestAvailableSlot = FindBestAvailableSlot (item);

			if (bestAvailableSlot != null) {
				//Set successfully assigned.  
				successfullyAssigned = true;
				//Add the new stack to the current item stack.  
				bestAvailableSlot.ModifyCurrentItemStack (item.mainContentReference.stack);
				Debug.Log ("Assigned " + item.mainContentReference.uiSlotContent.itemScreenName + " to slot with items of same type.");
			} else {
				Debug.Log ("Could not stack item: Attempting to add to an empty slot");
				bestAvailableSlot = FindBestAvailableNullSlot ();
				if (bestAvailableSlot != null) {
					successfullyAssigned = true;
					bestAvailableSlot.AssignNewItem (item);
				} else {
					Debug.LogError("No slots are empty!");
				}
			}
		} else {
			if (initialized == false && item == null) {
				Debug.LogError("Not initialized and item is null");
			} else if (initialized == false) {
				Debug.LogError("Not initialized");
			} else {
				Debug.LogError("Item is null");
			}
		}

		return successfullyAssigned;
	}

	//Searches for the best available slot in the slot array.  (One that already has the specified item)
	public MerchantSlotScript FindBestAvailableSlot(ResourceReferenceWithStackAndPrice pendingObjectToCheck) {
		if (slotArray != null) {
			for (int y = slotArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotArray.GetLength(1); x++) {
					//Define the object in the slot. 
					ResourceReferenceWithStackAndPrice objectAssigned = slotArray[y, x].GetCurrentlyAssigned();
					//Check to make sure objectAssigned is not null.  
					if (objectAssigned != null)
						//Check to make sure the item is the same.  
					if (objectAssigned.mainContentReference.uiSlotContent.itemType == pendingObjectToCheck.mainContentReference.uiSlotContent.itemType)
					if (objectAssigned.mainContentReference.uiSlotContent.localGroupID == pendingObjectToCheck.mainContentReference.uiSlotContent.localGroupID)
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
	public MerchantSlotScript FindBestAvailableNullSlot() {
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
	public MerchantSlotScript CheckForCertainInventoryItem(ResourceReferenceWithStackAndPrice pendingObjectToCheck) {
		if (slotArray != null) {
			for (int y = slotArray.GetLength(0) - 1; y >= 0; y--) {
				//Check for a stackable slot.  
				for (int x = 0; x < slotArray.GetLength(1); x++) {
					//Define the item that is in the specified slot.  
					ResourceReferenceWithStackAndPrice objectAssigned = slotArray[y, x].GetCurrentlyAssigned();
					//Check whether the assigned object is null.  
					if (objectAssigned != null)
						//Check to make sure the item types are the same.  
					if (objectAssigned.mainContentReference.uiSlotContent.itemType == pendingObjectToCheck.mainContentReference.uiSlotContent.itemType)
						//Check to see that the IDs are the same.  
					if (objectAssigned.mainContentReference.uiSlotContent.localGroupID == pendingObjectToCheck.mainContentReference.uiSlotContent.localGroupID)
						//Check to see that the stacks are greater or equal to one another.  
					if (objectAssigned.mainContentReference.stack >= pendingObjectToCheck.mainContentReference.stack)
						//Since the slot fits all requirements, return the slot.  
						return slotArray [y, x];
				}
			}
		} else {
			Debug.Log("Slot array is null");
		}

		return null;
	}

	//Used for GameData.  
	public ResourceReferenceWithStackAndPrice[] GetAllPlayerItems() {
		//List that will hold all player items.  
		List <ResourceReferenceWithStackAndPrice> playerItems = new List <ResourceReferenceWithStackAndPrice> ();

		for (int y = slotArray.GetLength(0) - 1; y >= 0; y--) {
			for (int x = 0; x < slotArray.GetLength(1); x++) {
				//Check whether the slot has an item assigned.  
				if (slotArray[y, x].GetCurrentlyAssigned() != null) {
					//Add the item to the slot.  
					playerItems.Add(slotArray[y, x].GetCurrentlyAssigned());
				}
			}
		}

		//Return the list as an array.  
		return playerItems.ToArray ();
	}

	public void ClearInventory() {
		for (int i = 0; i < slotArray.GetLength (0); i++) {
			for (int j = 0; j < slotArray.GetLength (1); j++) {
				slotArray [i, j].DeAssignItem ();
			}
		}
	}

}
