using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThukBehaviour : NPCBaseScript {

	[SerializeField] private GameObject guard = null;

	protected override void InitializeNPC() {
		npcName = "Thuk";
		//Sets the initial dialogue for the player.  
		GetComponent <NPCPanelController> ().SetCharacterDialogue (new string[] {
			"Hm?  Who be you?", 
			"I don't be recalling seeing your head before...", 
			"Why must you trouble great Thuk, leader of the Ice Clan?", 
			". . .", 
			"You WHAT?", 
			"You can NEVER have my shiny!  It is MINE!", 
			"You may leave with your life if you go now."
		});
	}

	public override void NPCActionBeforeSpeaking() {
		playerTransform.GetComponent <PlayerAction> ().DisablePlayerActions ();
	}

	public override void NPCActionAfterSpeaking() {
		playerTransform.GetComponent <PlayerAction> ().EnablePlayerActions ();
		StartCoroutine (WaitForPlayerToMoveDistanceAway ());
	}

	IEnumerator WaitForPlayerToMoveDistanceAway() {
		while (Mathf.Abs (playerTransform.position.x - transform.position.x) < 5) {
			yield return null;
		}

		//Make player stop.  
		playerTransform.GetComponent <PlayerAction> ().DisablePlayerActions();
		yield return StartCoroutine(GetComponent <NPCPanelController> ().ForceSpeechToPlayer (new string[] {
			"WAIT!"
		}));

		//Disable the player speaking to Thuk as he walks over.  
		GetComponent <NPCPanelController> ().Disable ();

		//Walk to the player.
		yield return StartCoroutine (SetTargetPosition (playerTransform.position, 2f, 40, 4));

		yield return StartCoroutine (GetComponent <NPCPanelController> ().ForceSpeechToPlayer (new string[] {
			"I smell someone that I had known for a long time...", 
			"Something familiar...", 
			". . .", 
			"LUK!", 
			"So you plan to help Luk overthrow me with my shiny!", 
			"NEVER!", 
			"GUARDS!  KILL HIM!"
		}));
			
		yield return StartCoroutine (SetTargetPosition (new Vector2(transform.position.x, transform.position.y) + new Vector2 (-2, 0), 1f, 20, 3));

		//Allow the player to move.  
		playerTransform.GetComponent <PlayerAction> ().EnablePlayerActions ();

		//The idea here is that there will be so many soldiers that Thuk will be overwhelmed.  
		if (guard != null) {
			//Create a list that will hold the soldiers. 
			List <ThukGuardScript> soldiers = new List<ThukGuardScript> ();

			//For all of the preset points where the soldiers should go.  Makes it difficult for the player to escape.  
			Transform pointsParent = transform.parent.FindChild("Points");
			for (int i = 0; i < pointsParent.childCount; i++) {
				GameObject instantiatedSoldier = (GameObject)(Instantiate (guard, pointsParent.GetChild (i).position, Quaternion.identity));
				soldiers.Add (instantiatedSoldier.GetComponent <ThukGuardScript> ());
				soldiers [soldiers.Count - 1].InitializationSequence ();
			}

			//Continually instantiate additional soldiers so the player does not try to run up and kill Thuk.  
			while (Vector2.Distance (playerTransform.position, transform.position) < 100) {
				//Create and initialize the soldier.  
				GameObject createdSoldier = (GameObject)(Instantiate (guard, new Vector2 (transform.position.x, transform.position.y) + new Vector2 (1, 0), Quaternion.identity));
				soldiers.Add (createdSoldier.GetComponent <ThukGuardScript> ());
				soldiers [soldiers.Count - 1].InitializationSequence ();

				yield return new WaitForSeconds (4f);
			}

			for (int i = 0; i < soldiers.Count; i++) {
				Destroy(soldiers [i].gameObject);
			}
		} else {
			Debug.Log ("Thuk has no guards!  Fix in ThukBehaviour in the Unity UI.");
		}
	}

	public override void NPCActionOnPlayerWalkAway(){
	}

	protected override IEnumerator WalkAround() {
		yield return null;
	}

}
