using UnityEngine;
using System.Collections;

public class BukBehaviour : NPCBaseScript {

	protected override void SetReferences() {
		base.SetReferences ();
		npcName = "Buk";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"Thuk is good leader!", 
			"He teach us about shinies!", 
			"You see Uk and Gluk?"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
	}

	public override void NPCActionAfterSpeaking() {
	}

}
