
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class ThukGuardScript : EnemyBaseActionClass, ICanHoldItems {

	//Thuk's guards are not initialized by the LevelEventManager, so this works instead.  
	public void InitializationSequence() {
		SetReferences();
		GetComponent <CharacterHealthPanelManager> ().InitializeHealthBar ();
		GetComponent <EnemyExpDropper> ().InitializeExpDropper ();
	}

	//Initialization stuff.  
	protected override void InitializeEnemy() {
		//Get the spear for the soldier.  
		ItemBase spear = characterSpriteObject.FindChild("Hands").FindChild("HoldingHand").FindChild("HoldingItem").GetChild(0).GetComponent <ItemBase> ();
		spear.SetAttachedCharacterInput (this);
		OnRefreshCurrentWeaponMoves(spear);

		StartCoroutine (EnemyControl());
	}

	//This should be FixedUpdate.  
	protected override IEnumerator EnemyControl() {
		//Continuously
		while (true) {
			//Check to see whether player is within radius. 
			if (Vector2.Distance (transform.position, player.transform.position) <= playerViewableThreshold) {

				//Calculate the distance from each respective safe zone.  
				float distanceFromLeftSafeZone = transform.position.x - (player.transform.position.x - playerAttackDistance);
				float distanceFromRightSafeZone = transform.position.x - (player.transform.position.x + playerAttackDistance);
			
				//If we are in a safe zone, either the left, right, or neither.  
				if (Mathf.Abs (distanceFromLeftSafeZone) <= ignorePlayerMovementThreshold || Mathf.Abs (distanceFromRightSafeZone) <= ignorePlayerMovementThreshold) {
					//Flip to face the player and attack.  
					Stop ();
					FlipToFacePlayer ();
					//Attack if the y values between the enemy and the player are close enough.  
					if (Mathf.Abs (player.transform.position.y - transform.position.y) <= maxYValueSeparation) {
						Attack ();
					}
					yield return new WaitForSeconds (1.5f);

				} else {
					//We are not in either safe zone.  
				
					//This will hold the eventual value of the target safe zone.  
					float distanceFromTargetSafeZone = 0;

					//Give target safe zone a value.  
					if (Mathf.Abs (distanceFromLeftSafeZone) <= Mathf.Abs (distanceFromRightSafeZone)) {
						distanceFromTargetSafeZone = distanceFromLeftSafeZone;
					} else {
						distanceFromTargetSafeZone = distanceFromRightSafeZone;
					}

					//Calculate how to flip based on the distance from the target safe zone.  
					//If we are to the left of the safe zone.  
					//Example: We are at 7, and the left safe zone at 1.  7 - 1 is positive, and we are to the left of the safe zone.  
					if (distanceFromTargetSafeZone < 0) {
						//If we are facing left
						if (GetFacingDirection () == -1)
							//Flip to face the right side.  
							Flip ();
					} else if (distanceFromTargetSafeZone >= 0) {
						//If we are facing right.  
						if (GetFacingDirection () == 1) 
							//Flip to face the left side.  
							Flip ();
					}
				
					//Start moving toward the target safe zone (we have already flipped to the position
					anim.SetFloat ("Speed", 1);
					//Yield returning a coroutine makes it wait until the coroutine is completed.  
					yield return StartCoroutine (MaintainAConstantXVelocity (0.3f));

					//In the event that the x velocity is very small, jump.  
					if (Mathf.Abs (rb2d.velocity.x) < maxSpeed / 100f && grounded) {
						InitializeJump (1);
						//Wait until we are in the air.  
						//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
						yield return new WaitForSeconds (0.3f);
						//Start moving forward again (mid-air).  
						anim.SetFloat ("Speed", 1);
						yield return StartCoroutine (MaintainAConstantXVelocity (0.3f));
					}

				}
			} else {
				//We are not viewable by the player.  
				Stop ();
				//Wait a couple seconds instead of a frame (processing reasons).  
				yield return new WaitForSeconds (3);
			}
		}
	}

	/************************************************ ITEM STUFF *************************************************************/
	//This dictionary contains the possible weapon moves for the player.  The first entry contains the required action to trigger the action, and the second
	//includes a string of the method.  
	private MovementAndMethod[] possibleWeaponMoves = null;

	private ItemBase itemInUseByCharacter = null;

	//This will be called by the item management part of the costume manager script
	public void OnRefreshCurrentWeaponMoves(ItemBase ctorItemInUseByCharacter) {
		itemInUseByCharacter = ctorItemInUseByCharacter;
		if (ctorItemInUseByCharacter != null) {
			possibleWeaponMoves = itemInUseByCharacter.GetPossibleActionsForItem ();
		} else {
			possibleWeaponMoves = null;
		}
	}
		
	//Used to stab with the spear (the input is not necessary, but can be left for now).  
	protected override void Attack() {
		//AttackAction(new MovementAndMethod(MovementAndMethod.PossibleMovements.Stab, MovementAndMethod.PossibleTriggers.LeftMouseClick, false));
		//Make sure that the spear has the accurate index for stab.  Makes it kind of hard to use.  
		AttackAction(possibleWeaponMoves[5]);
	}

	//Has to be public for the interface
	public void AttackAction(MovementAndMethod someAttack) {
		if (!currentlyInAttackAnimation) {
			anim.SetTrigger (someAttack.GetActionKey());
			itemInUseByCharacter.InfluenceEnvironment (someAttack.GetActionEnum());
			if (someAttack.GetActionEnum() != MovementAndMethod.PossibleMovements.CreatePhysicalItem)
				currentlyInAttackAnimation = true;
		} else {
			Debug.Log(gameObject.name + " was in attack animation, did not attack");
		}
	}

	//Used for weapons.  
	public void ExternalJumpAction (int num) {
		InitializeJump (num);
	}

}
