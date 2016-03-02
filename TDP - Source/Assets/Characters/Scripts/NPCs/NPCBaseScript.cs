using UnityEngine;
using System.Collections;

public abstract class NPCBaseScript : CharacterBaseActionClass {

	//Initialization.  
	protected override void OnEnable() {
		LevelEventManager.InitializeNPCs += SetReferences;
	}

	protected override void OnDisable() {
		LevelEventManager.InitializeNPCs -= SetReferences;
	}

	//Required for the NPCPanelController.  
	[HideInInspector] public string npcName = "NPC";

	bool walkingAround = true;

	protected Transform playerTransform;
	protected InventoryFunctions playerInventory;

	//By using an actual IEnumerator object, we can selectively run and manipulate coroutines.  
	protected IEnumerator walkAroundCoroutine;

	//Initializing the NPC
	protected override void InitializeCharacter() {
		//Get required components.  
		playerTransform = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		playerInventory = CurrentLevelVariableManagement.GetMainInventoryReference().GetComponent <InventoryFunctions> ();

		//Initialize the NPC before starting to walk around.  
		InitializeNPC ();
		//Create and start the coroutine.  
		walkAroundCoroutine = WalkAround ();
		StartCoroutine (walkAroundCoroutine);
	}

	protected abstract void InitializeNPC ();

	public abstract void NPCActionBeforeSpeaking();
	public abstract void NPCActionAfterSpeaking();
	public abstract void NPCActionOnPlayerWalkAway ();

	//Walks around fairly randomly, keeping the npc at a constant velocity.  IF YOU DON'T WANT THIS TO DO ANYTHING, THEN OVERRIDE IT WITH AN EMPTY COROUTINE
	protected virtual IEnumerator WalkAround() {
		while (true) {

			//Walk in one direction
			anim.SetFloat("Speed", 1);
			//Yield returning a coroutine makes it wait until the coroutine is completed.  
			yield return StartCoroutine(MaintainAConstantXVelocity(2f));
			
			//In the event that the x velocity is very small, jump.  
			if (Mathf.Abs(rb2d.velocity.x) < moveForce / 1000f && grounded) {
				InitializeJump(1);
				//Wait until we are in the air.  
				//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
				yield return new WaitForSeconds(0.3f);
				//Start moving forward again (mid-air).  
				anim.SetFloat("Speed", 1);
				yield return StartCoroutine(MaintainAConstantXVelocity(2f));
			}

			//Stop walking.  
			Stop ();
			yield return new WaitForSeconds(3f);
			//Flip (random movement)
			if (Random.Range(0, 2) == 1)
				Flip ();
		}
	}

	protected bool GiveMoneyToPlayer(int amount) {
		return playerTransform.GetComponent <PlayerHealthPanelManager> ().GiveMoneyToPlayer (amount);
	}
	
	public void StopWalkingAround() {
		if (walkingAround) {
			StopCoroutine (walkAroundCoroutine);
			StopCoroutine("MaintainAConstantXVelocity");
			Stop ();
			walkingAround = false;
		}
	}
	
	public void ResumeWalkingAround() {
		if (! walkingAround) {
			walkAroundCoroutine = WalkAround ();
			StartCoroutine(walkAroundCoroutine);
			walkingAround = true;
		}
	}

	public void FlipToFacePlayer() {
		if (playerTransform.position.x >= transform.position.x) {
			if (GetFacingDirection() != 1) {
				Flip ();
			}
		} else {
			if (GetFacingDirection() != -1) {
				Flip();
			}
		}
	}

}
