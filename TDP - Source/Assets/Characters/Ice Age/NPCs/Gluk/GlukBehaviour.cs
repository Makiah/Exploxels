using UnityEngine;
using System.Collections;

public class GlukBehaviour : NPCBaseScript {

	protected override void InitializeNPC() {
		npcName = "Gluk";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"Thuk get angry if we bring no shinies.", 
			"I bring shinies to Thuk!", 
			"Where Uk and Buk go?"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
	}

	public override void NPCActionAfterSpeaking() {
	}
	public override void NPCActionOnPlayerWalkAway(){
	}
}
