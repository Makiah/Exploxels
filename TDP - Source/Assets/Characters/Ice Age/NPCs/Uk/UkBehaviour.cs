using UnityEngine;
using System.Collections;

public class UkBehaviour : NPCBaseScript {

	protected override void InitializeNPC() {
		npcName = "Uk";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"Thuk want shinies for collection!", 
			"I collect shinies for Thuk!", 
			"Have you seen brothers Buk and Gluk?"
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
