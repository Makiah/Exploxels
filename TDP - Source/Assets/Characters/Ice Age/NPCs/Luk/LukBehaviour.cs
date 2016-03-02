using UnityEngine;
using System.Collections;

public class LukBehaviour : NPCBaseScript {

	bool allowedThePlayerToMoveAgain = false;

	protected override void InitializeNPC() {
		npcName = "Luk";
		string[] dialogue = new string[] {
			"Why 'lo there, young'un.", 
			"I be sorry for rough treatment of these persons.", 
			"My name be Luk.",
			"They be merely paranoid of an attack by Thuk.", 
			"Who Thuk?", 
			"I was leader of the ice clan for 8 seasons.",
			"Was good leader, all thought good of me.",
			"About month ago, Thuk challenge myself to test of courage.", 
			"I old, but I fight him anyway and lose.", 
			"I no want to die, so run away real fast.", 
			"I was good leader, so soldiers follow me into exile.", 
			"Waiting until the clan rallies behind me to return to battle."
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);

	}

	public override void NPCActionBeforeSpeaking() {
	}

	public override void NPCActionAfterSpeaking() {
		if (allowedThePlayerToMoveAgain == false) {
			playerTransform.GetComponent <PlayerAction> ().EnablePlayerActions ();
			allowedThePlayerToMoveAgain = true;
		}
		string[] dialogue = new string[] {
			"Help?  You want to help?", 
			"Thank you, young'un, but I refuse.", 
			"I cannot bring you into this conflict.", 
			"It is not meant to be."
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionOnPlayerWalkAway(){
	}

	//The soldiers should not walk around.  
	protected override IEnumerator WalkAround() {
		yield return null;
	}

}
