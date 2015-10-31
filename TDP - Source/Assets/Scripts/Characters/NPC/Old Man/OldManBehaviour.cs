using UnityEngine;
using System.Collections;

public class OldManBehaviour : NPCBaseScript {

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

	public override void NPCActionAfterSpeaking() {
		transform.FindChild ("DropHandler").GetComponent <NPCSlotModifier> ().AddNewItemToPlayerInventory (
			new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wooden Hatchet"), 1)
		);
	}

}
