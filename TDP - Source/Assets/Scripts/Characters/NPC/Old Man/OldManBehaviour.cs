﻿using UnityEngine;
using System.Collections;

public class OldManBehaviour : NPCBaseScript {

	bool gavePlayerInstructions = false;
	bool tookApples = false;
	bool completedStoryRole = false;

	protected override void SetReferences() {
		base.SetReferences ();
		string[] dialogue = new string[] {"Oh dear.  Oh dear me...", 
			"Why hello there, young stranger.  I don't suppose you have a moment?", 
			"These trees contain apples, which I need to stock up for winter.", 
			"But I am too old for this sort of work, and can't do it on my own.", 
			"If you help me, I will be sure to repay you by whatever means necessary.", 
			"I need 6 apples in total.", 
			"Here is a hatchet.  I am sure that it will be fairly easy for you."
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
		if (gavePlayerInstructions && tookApples == false) {
			SlotScript slotWithContent = localNPCSlotModifier.CheckWhetherPlayerHasSpecifiedItem(new UISlotContentReference(ResourceDatabase.GetItemByParameter("Apple"), 6));
			if (slotWithContent != null) {
				slotWithContent.ModifyCurrentItemStack(-6);
				Debug.Log("Player had required items");
				string[] newDialogue = new string[]{
					"Thank you, young one!", 
					"I appreciate your help.", 
					"Here is a bit of money as a reward."
				};
				GetComponent <NPCPanelController> ().SetCharacterDialogue(newDialogue);
				tookApples = true;
			}
		}
	}

	public override void NPCActionAfterSpeaking() {
		if (gavePlayerInstructions == false) {
			localNPCSlotModifier.AddNewItemToPlayerInventory (
			new UISlotContentReference (ResourceDatabase.GetItemByParameter ("Wooden Hatchet"), 1)
			);
			gavePlayerInstructions = true;
		}

		if (tookApples && completedStoryRole == false) {
			string[] newDialogue = new string[]{
				"If you walk over the hills, you should see a small town.", 
				"Use some of the money I gave you to buy some new stuff!"
			};
			GetComponent <NPCPanelController> ().SetCharacterDialogue(newDialogue);
			completedStoryRole = true;
		}
	}

}
