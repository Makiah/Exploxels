﻿using UnityEngine;
using System.Collections;

public class BlacksmithBehaviour : Artisan {

	protected override void SetReferences() {
		base.SetReferences ();
		string[] dialogue = new string[]{"Why, hello!", "My name is Durog.", "I will smelt your metals."};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
//		if (GiveMoneyToPlayer (-25)) {
//			localNPCSlotModifier.AddNewItemToPlayerInventory (
//				new UISlotContentReference (ResourceDatabase.GetItemByParameter ("Wooden Sword"), 1)
//			);
//		}
	}

	public override void NPCActionAfterSpeaking() {

	}

}
