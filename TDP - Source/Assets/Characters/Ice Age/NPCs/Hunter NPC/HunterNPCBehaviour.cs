using UnityEngine;
using System.Collections;

public class HunterNPCBehaviour : NPCBaseScript {

	//Local booleans
	bool talkedToPlayer = false;
	bool playerHasCreatedFire = false;
	bool roleCompleted;

	protected override void InitializeNPC() {
		npcName = "Miner";
		string[] dialogue = new string[] {
			"Eh?  Why hello there!", 
			"Hey, can ya keep a secret?", 
			"I think that I've discovered somethin' purty cool.", 
			"It's made by combinin' 2 coal with 5 wood.", 
			"I'll let you see out what happens when you craft it.", 
			"Use this pickaxe to mine coal."
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {
		//Check to make sure that the player has not already created fire.  
		if (playerHasCreatedFire == false) {
			if (CurrentLevelVariableManagement.GetMainObjectiveManager().CheckStateOfObjective(5)) { //Check whether the player has created fire;
				playerHasCreatedFire = true;
				string[] dialogue = new string[]{
					"Nice job!", 
					"I wish you luck on your travels."
				};
				GetComponent <NPCPanelController> ().SetCharacterDialogue(dialogue);
			}
		}

		//Check whether the NPC has talked to the player
		if (talkedToPlayer && playerHasCreatedFire == false) {
			string[] dialogue = new string[] {
				"Hello again!  Forgot already?", 
				"While on me many travels, I discovered something that will change our lives forever.", 
				"I call it FIRE.", 
				"Here's how you make it.  Gather some wood.  6 blocks should be enough.", 
				"Combine them to make 3 planks.", 
				"Then burn a bit of wood to make charcoal, and combine that with the planks."
			};
			GetComponent <NPCPanelController>().SetCharacterDialogue (dialogue);
		}
	}

	public override void NPCActionAfterSpeaking() {
		//When the NPC has finished speaking to the player.  
		if (talkedToPlayer == false) {
			talkedToPlayer = true;
			playerInventory.AssignNewItemToBestSlot(new ResourceReferenceWithStack(ResourceDatabase.GetItemByParameter("Wooden Pickaxe"), 1));
		}
	}

	public override void NPCActionOnPlayerWalkAway(){
	}
}
