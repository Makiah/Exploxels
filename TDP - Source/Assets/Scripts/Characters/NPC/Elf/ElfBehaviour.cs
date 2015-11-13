using UnityEngine;
using System.Collections;

public class ElfBehaviour : Artisan {

	protected override void SetReferences() {
		base.SetReferences ();
		string[] dialogue = new string[]{"It is good to meet you!", "My name is Gwendel.", "I will enchant any weapons you have."};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	bool readyToLoadNextLevel = false;

	public override void NPCActionBeforeSpeaking() {
		if (ModifiesSlotContent.DetermineWhetherPlayerHasCertainInventoryItem(
			new UISlotContentReference(ResourceDatabase.GetItemByParameter("Wooden Sword"), 1)) != null
			&&
			ModifiesSlotContent.DetermineWhetherPlayerHasCertainInventoryItem(
			new UISlotContentReference(ResourceDatabase.GetItemByParameter("Wooden Hatchet"), 1)) != null
			) {
			string[] newDialogue = new string[] {"Nice job!", "See you in the next level"};
			GetComponent <NPCPanelController> ().SetCharacterDialogue(newDialogue);
			readyToLoadNextLevel = true;
		}
	}

	public override void NPCActionAfterSpeaking() {
		/*if (readyToLoadNextLevel)*/
			CurrentLevelVariableManagement.GetMainGameControl ().OnTutorialComplete ();
	}

}
