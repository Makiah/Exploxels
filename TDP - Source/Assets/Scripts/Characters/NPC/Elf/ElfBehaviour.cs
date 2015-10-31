using UnityEngine;
using System.Collections;

public class ElfBehaviour : Artisan {

	protected override void SetReferences() {
		base.SetReferences ();
		string[] dialogue = new string[]{"It is good to meet you!", "My name is Gwendel.", "I will enchant your weapons."};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
		
	}

	public override void NPCActionAfterSpeaking() {

	}

}
