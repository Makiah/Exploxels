using UnityEngine;
using System.Collections;

public class ThukBehaviour : NPCBaseScript {

	protected override void InitializeNPC() {
		npcName = "Thuk";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			""
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
	}

	public override void NPCActionAfterSpeaking() {
	}

}
