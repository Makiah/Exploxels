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
		StartCoroutine ("WalkAround");
	}

	public abstract void NPCActionBeforeSpeaking();
	public abstract void NPCActionAfterSpeaking();

	//Walks around fairly randomly, keeping the player at a constant velocity.  
	protected virtual IEnumerator WalkAround() {
		while (true) {
			anim.SetFloat("Speed", 1);
			StartCoroutine ("MaintainAConstantXVelocity", GetFacingDirection() * moveForce);
			yield return new WaitForSeconds(3f);
			StopCoroutine("MaintainAConstantXVelocity");
			anim.SetFloat("Speed", 0);
			yield return new WaitForSeconds(3f);
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
			StopCoroutine ("WalkAround");
			StopCoroutine("MaintainAConstantXVelocity");
			rb2d.velocity = Vector2.zero;
			anim.SetFloat("Speed", 0);
			walkingAround = false;
		}
	}
	
	public void ResumeWalkingAround() {
		if (! walkingAround) {
			StartCoroutine("WalkAround");
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
