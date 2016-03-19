using UnityEngine;
using System.Collections;

public class SoldierNPCBehaviour : NPCBaseScript {

	bool npcActionsActive = true;

	//Interacting with player stuff.  
	protected override void InitializeNPC() {
		npcName = "Soldier";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"We protect great Thuk!"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {}

	public override void NPCActionAfterSpeaking() {}

	//A variable has to be used because there is no way to initialize the coroutine to stop.  
	public void EnableNPCActions () {
		if (npcActionsActive == false) {
			Debug.Log ("Enabled NPC Behaviour");
			npcActionsActive = true;
			StartCoroutine (walkAroundCoroutine);
			GetComponent <NPCPanelController> ().Enable ();

			//Set facingRight based on the current facing direction
			facingRight = transform.FindChild("FlippingItem").localScale.x > 0;
		}
	}

	public void DisableNPCActions() {
		if (npcActionsActive == true) {
			Debug.Log ("Disabled NPC Behaviour");
			npcActionsActive = false;
			if (walkAroundCoroutine != null) 
				StopCoroutine (walkAroundCoroutine);
			GetComponent <NPCPanelController> ().Disable ();
		}
	}

	protected override IEnumerator WalkAround() {
		while (true) {
			if (npcActionsActive) {
				//Walk in one direction
				anim.SetFloat ("Speed", 1);
				//Yield returning a coroutine makes it wait until the coroutine is completed.  
				yield return StartCoroutine (MaintainAConstantXVelocity (2f));

				//In the event that the x velocity is very small, jump.  
				if (Mathf.Abs (rb2d.velocity.x) < moveForce / 1000f && grounded) {
					InitializeJump (1);
					//Wait until we are in the air.  
					//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
					yield return new WaitForSeconds (0.3f);
					//Start moving forward again (mid-air).  
					anim.SetFloat ("Speed", 1);
					yield return StartCoroutine (MaintainAConstantXVelocity (2f));
				}

				//Stop walking.  
				Stop ();
				yield return new WaitForSeconds (3f);
				//Flip (random movement)
				if (Random.Range (0, 2) == 1)
					Flip ();
			} else {
				yield return null;
			}
		}
	}


	public override void NPCActionOnPlayerWalkAway(){
	}

}
