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
	public string npcName = "NPC";

	bool walkingAround = true;

	protected Transform playerTransform;
	[SerializeField] protected float minDistanceRequiredForInteraction;

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild ("FlippingItem").FindChild ("Character");
		base.SetReferences ();
		playerTransform = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		StartCoroutine (WalkAround());
	}

	public abstract void NPCActionBeforeSpeaking();
	public abstract void NPCActionAfterSpeaking();

	//Walks around fairly randomly, keeping the player at a constant velocity.  
	protected virtual IEnumerator WalkAround() {
		while (true) {
			//Walk in one direction
			anim.SetFloat("Speed", 1);
			//Yield returning a coroutine makes it wait until the coroutine is completed.  
			yield return StartCoroutine(MaintainAConstantXVelocity(GetFacingDirection() * moveForce, 1.5f));
			
			//In the event that the x velocity is very small, jump.  
			if (rb2d.velocity.x < moveForce / 1000f && grounded) {
				InitializeJump(1);
				//Wait until we are in the air.  
				//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
				yield return new WaitForSeconds(0.3f);
				//Start moving forward again (mid-air).  
				anim.SetFloat("Speed", 1);
				yield return StartCoroutine(MaintainAConstantXVelocity(GetFacingDirection() * moveForce, 1.5f));
			}

			//Stop walking.  
			Stop ();
			yield return new WaitForSeconds(3f);
			//Flip (random movement)
			if (Random.Range(0, 2) == 1)
				Flip ();
			
			yield return null;
		}
	}

	protected bool GiveMoneyToPlayer(int amount) {
		return playerTransform.GetComponent <PlayerHealthPanelManager> ().GiveMoneyToPlayer (amount);
	}
	
	public void StopWalkingAround() {
		if (walkingAround) {
			StopCoroutine (WalkAround());
			StopCoroutine("MaintainAConstantXVelocity");
			Stop ();
			walkingAround = false;
		}
	}
	
	public void ResumeWalkingAround() {
		if (! walkingAround) {
			StartCoroutine(WalkAround());
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
