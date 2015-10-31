using UnityEngine;
using System.Collections;

public class NPCSlotModifier : ModifiesSlotContent {

	public bool AddNewItemToPlayerInventory(UISlotContentReference newItem) {
		if (! AssignNewItemToBestSlot (newItem)) {
			Debug.LogError ("There was an error while attempting to add a new item to the player inventory.");
			return false;
		} 
		return true;
	}

	public SlotScript CheckWhetherPlayerHasSpecifiedItem(UISlotContentReference specifiedItem) {
		return CheckWhetherPlayerHasCertainItem(specifiedItem);
	}

}
