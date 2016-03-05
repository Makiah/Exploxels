using UnityEngine;
using System.Collections;

public class MerchantBehaviour : NPCBaseScript {

	/*
	 * This script controls the merchant of the Ice Age (and possibly other ages at some point in the future).  The slots should be what actually adds stuff 
	 */

	MerchantInventoryFunctions merchantInventory;

	//Using two different arrays instead of an array of a class mainly because it would be irritating to change around MerchantSlotScript to work with a whole new class.  
	ResourceReferenceWithStackAndPrice[] merchantItems;
	int[] costOfItems;

	protected override void InitializeNPC() {
		//Find and hide the inventory.  
		merchantInventory = CurrentLevelVariableManagement.GetLevelUIReference ().transform.FindChild ("Merchant Inventory").GetComponent <MerchantInventoryFunctions> ();

		//Define the UISlotContent items that will be added to the inventory.  
		merchantItems = new ResourceReferenceWithStackAndPrice[2];
		merchantItems [0] = new ResourceReferenceWithStackAndPrice (new ResourceReferenceWithStack (ResourceDatabase.GetItemByParameter ("Wooden Hatchet"), 1), 10);
		merchantItems [1] = new ResourceReferenceWithStackAndPrice (new ResourceReferenceWithStack (ResourceDatabase.GetItemByParameter ("Diamond Sword"), 2), 20);

		string[] dialogue = new string[] {
			"Purchase any item you like!", 
			"Everything is cheap at Sluk's Hardware Store!"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
		//Add the items to the inventory.  
		for (int i = 0; i < merchantItems.Length; i++) {
			Debug.Log ("Assigning " + merchantItems [i].mainContentReference.uiSlotContent.itemScreenName);
			if (! (merchantInventory.AssignNewItemToBestSlot (merchantItems [i]))) {
				Debug.LogError ("Could not assign merchant item " + merchantItems [i].mainContentReference.uiSlotContent.itemScreenName + ", either caused by too many items or an extraneous error");
			}
		}
		//Set the current merchant items to nothing (they are all in the inventory now).  
		merchantItems = new ResourceReferenceWithStackAndPrice[0];

		//Show the inventory.  
		ShowInventory();
	}

	public override void NPCActionAfterSpeaking() {
		
	}

	public override void NPCActionOnPlayerWalkAway(){
		//Hide the inventory
		HideInventory();
		//Remove the items from the inventory and add it to the array of merchant items.  
		merchantItems = merchantInventory.GetAllPlayerItems();
		merchantInventory.ClearInventory ();
	}

	//Quick utilities.  Probably not really necessary, but looks simple.  
	void ShowInventory() {
		merchantInventory.transform.FindChild("Slots").gameObject.SetActive (true);
	}

	void HideInventory() {
		merchantInventory.transform.FindChild("Slots").gameObject.SetActive (false);
	}

}
