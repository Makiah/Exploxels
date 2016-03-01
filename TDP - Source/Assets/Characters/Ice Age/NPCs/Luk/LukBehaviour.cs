using UnityEngine;
using System.Collections;

public class LukBehaviour : NPCBaseScript {

	bool gavePlayerInstructions = false;
	bool tookApples = false;
	bool completedStoryRole = false;

	protected override void InitializeNPC() {
		npcName = "Luk";
		string[] dialogue = new string[] {
			"Why hello there, young one.", 
			"I am sorry about the rough treatment of my guards.", 
			"They are merely paranoid of an attack by Thuk, current leader of the clan.", 
			""
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
