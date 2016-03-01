using UnityEngine;
using System.Collections;

public class OrangeSoldierBehaviour : SoldierNPCBehaviour {

	public static bool broughtPlayerToLuk = false;

	PlayerAction player;

	protected override void InitializeNPC() {
		npcName = "Soldier";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"INTRUDER!"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
		player = CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerAction> ();
	}

	//NPC Actions
	public override void NPCActionBeforeSpeaking() {
		if (broughtPlayerToLuk == false) {
			player.DisablePlayerActions ();
			Debug.Log ("Disabled Player Actions");
		}
	}

	public override void NPCActionAfterSpeaking() {
		
	}

	public abstract void NPCActionOnPlayerWalkAway () {

	}
}
