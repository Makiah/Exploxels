using UnityEngine;
using System.Collections;

public class SoldierBehaviour : NPCBaseScript {

	protected override void SetReferences() {
		base.SetReferences ();
		npcName = "Soldier";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"We protect great Thuk!"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
	}

	public override void NPCActionAfterSpeaking() {
	}

}
