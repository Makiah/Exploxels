using UnityEngine;
using System.Collections;

public class IntroductorySoldierBehaviour : NPCBaseScript {

	public static bool broughtPlayerToLuk = false;

	//IDEA: Why doesn't Luk give the player his weapon?  Kind of weird for the player to get a weapon when he spawns anyway.  

	private PlayerAction player;

	[SerializeField] private CharacterBaseActionClass[] peopleToMoveInOrder = null;

	protected override void InitializeNPC() {
		npcName = "Soldier";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"INTRUDER!", 
			"Follow us.  We will take you to leader."
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
		if (broughtPlayerToLuk == false) {
			Debug.Log ("Starting to walk to Thuk.");
			//Disable the Panel Controller (so that the player cannot interrupt the movement of the soldiers).  
			GetComponent <NPCPanelController> ().Disable ();
			StartCoroutine (WalkToThuk ());
		}
	}

	public override void NPCActionOnPlayerWalkAway () {
		NPCActionAfterSpeaking ();
	}

	//The soldiers should not walk around.  
	protected override IEnumerator WalkAround() {
		yield return null;
	}

	//Moves all characters to Luk.  
	IEnumerator WalkToThuk() {
		//Make it so the player cannot make the soldiers speak to him.  
		foreach (CharacterBaseActionClass character in peopleToMoveInOrder) {
			character.gameObject.GetComponent <NPCPanelController> ().Disable ();
		}

		//Place the player in the right spot in the array.  
		Debug.Log("Constructing arrays");
		int playerIndex = -1;
		for (int i = 0; i < peopleToMoveInOrder.Length - 1; i++) {
			if (peopleToMoveInOrder [i].transform.position.x > player.transform.position.x && player.transform.position.x > peopleToMoveInOrder [i + 1].transform.position.x) {
				playerIndex = i + 1;
				break;
			}
		}
		//Make sure that the place to place the player is valid.  
		if (playerIndex == -1) playerIndex = peopleToMoveInOrder.Length;
		//New array.  
		CharacterBaseActionClass[] newPeopleToMoveInOrder = new CharacterBaseActionClass[peopleToMoveInOrder.Length + 1];
		//People to be moved before the player.
		for (int i = 0; i < playerIndex; i++) {
			newPeopleToMoveInOrder[i] = peopleToMoveInOrder [i];
		}
		//Put in the player. 
		newPeopleToMoveInOrder [playerIndex] = player;
		//People to be moved after the player.  
		for (int i = playerIndex; i < peopleToMoveInOrder.Length; i++) {
			newPeopleToMoveInOrder [i + 1] = peopleToMoveInOrder [i];
		}
		//Set the new move people.  
		peopleToMoveInOrder = newPeopleToMoveInOrder;
		Debug.Log ("Length of people = " + peopleToMoveInOrder.Length + ", and player is at " + playerIndex);

		Debug.Log ("Starting movement");
		//Begin to move toward Luk.  
		//What happens now, is all people move at the same time toward a target, and the coroutine waits until the last is complete.  This approach could be a problem.  
		//Two alternatives: 
		//1. A coroutine array filled with all of the coroutines that are occurring.  Only continues when all are complete.  
		//2. Stop all coroutines once the last is complete.  
		for (int i = 0; i < peopleToMoveInOrder.Length; i++) {
			//Move them to the first part of the floor.  
			if (i < peopleToMoveInOrder.Length - 1) {
				StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (383 - i, -17.9f), 2f, 40, 4));
				yield return new WaitForSeconds (.2f);
			} else {
				yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (383 - i, -17.9f), 3.5f, 40, 4));
			}
		}

		for (int i = 0; i < peopleToMoveInOrder.Length; i++) {
			//Move them to the first part of the floor.  
			yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (389, -25), 2f, 40, 4));

			//Adding 2i spaces out the people.  
			yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (380 + i, -25), 5f, 40, 4));
		}
		Debug.Log ("Starting movement 2");
		for (int i = 0; i < peopleToMoveInOrder.Length; i++) {
			//Adding 2i spaces out the people.  
			if (i < peopleToMoveInOrder.Length - 1) {
				StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (329 + i, -25), 2f, 40, 4));
				yield return new WaitForSeconds (.2f);
			} else {
				yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (329 + i, -25), 2f, 40, 4));
			}
		}

		Debug.Log ("Starting movement 3");
		for (int i = 0; i < peopleToMoveInOrder.Length; i++) {
			//Move them to the first part of the floor.  
			Debug.Log("Currently on step 1");
			yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (322.4f, -29.7f), 2f, 20, 3));
			//Move them to the second part of the stairs. 
			Debug.Log("Currently on step 2");
			yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (326.5f, -36.4f), 2f, 20, 3));
			//Move them to the second part of the stairs.  
			Debug.Log("Currently on step 3");
			yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (325.16f, -41.65f), 4f, 20, 3));
			Debug.Log ("Currently on step 4");
			yield return StartCoroutine (peopleToMoveInOrder [i].SetTargetPosition (new Vector2 (343 - i, -48.2f), 5f, 20, 3));
		}

		//Move the player to Luk.  MISSION ACCOMPLISHED. 
		player.ExternalJumpAction(1);
		yield return StartCoroutine (player.SetTargetPosition (new Vector2 (388f, -41.6f), 3f, 20, 2));

		broughtPlayerToLuk = true;

		//Update the dialogue.  
		string[] dialogue = new string[] {
			"We sorry we thought you was a threat."
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);

		//Re-enable the panel controller (was disabled previously).  
		GetComponent <NPCPanelController> ().Enable ();

		Debug.Log ("Intro completed");

		//From here, Luk will allow the player to move.  The player is forced to speak to Luk (he is standing still in front of him)
	}
}
