using UnityEngine;
using System.Collections;

public abstract class NPCBaseScript : CharacterBaseActionClass {

	protected override void OnEnable() {
		LevelEventManager.InitializeNPCs += SetReferences;
	}

	protected override void OnDisable() {
		LevelEventManager.InitializeNPCs -= SetReferences;
	}
	
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

	protected virtual IEnumerator WalkAround() {
		while (true) {
			anim.SetFloat("Speed", 1);
			rb2d.velocity = new Vector2(GetFacingDirection() * moveForce, 0);
			yield return new WaitForSeconds(3f);
			anim.SetFloat("Speed", 0);
			rb2d.velocity = Vector2.zero;
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
