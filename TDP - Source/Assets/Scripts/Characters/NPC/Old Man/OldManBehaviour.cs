﻿using UnityEngine;
using System.Collections;

public class OldManBehaviour : NPCBaseScript {

	protected override void SetReferences() {
		base.SetReferences ();
		string[] dialogue = new string[]{"Hey there!", "My name is Gandalf.", "I will be your guide."};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

}
