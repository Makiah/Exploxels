
/*
 * Author: Makiah Bennett
 * Created 13 September 2015
 * Last edited: 13 September 2015
 * 
 * 9/13 - Created: Added variables for player activation position, max y value separation, mindistancefromplayer, etc.  
 * 
 * This script should inherit from the character class, and provide support for being attacked by the player, losing health, and basic enemy functions.  
 * 
 */


using UnityEngine;
using System.Collections;
using System;

public abstract class EnemyBaseActionClass : CharacterBaseActionClass, ICombatant {

	/******************************************** INITIALIZATION *******************************************/
	
	protected override void OnEnable() {
		LevelEventManager.InitializeEnemies += SetReferences;
	}
	
	protected override void OnDisable() {
		LevelEventManager.InitializeEnemies -= SetReferences;
	}

	//The maximum distance that the player can be away from the enemy for it to activate.  
	[SerializeField] protected float playerViewableThreshold;
	//When at this distance, the enemy will attack.  
	[SerializeField] protected float playerAttackDistance;
	//If the enemy is within the safe attack distance + the movement threshold, it will remain stationary.  
	[SerializeField] protected float ignorePlayerMovementThreshold;
	//What is the maximum difference in Y values the enemies must have to attack?
	[SerializeField] protected float maxYValueSeparation;

	//Attacking power
	[SerializeField] protected float enemyAttackingPower;
	[SerializeField] protected Vector2 enemyKnockbackPower;

	//Attacking parameters
	[SerializeField] protected float enemyWithinAreaBounds = 0;
	[SerializeField] protected float yOffsetToEnemy = 0;
	[SerializeField] protected float distToEnemyLength = 0;

	//The player transform
	protected Transform player;

	protected IEnumerator enemyControlCoroutine;

	protected override void InitializeCharacter() {
		//Set required variables for the enemy to function.  
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;

		InitializeEnemy ();

		enemyControlCoroutine = EnemyControl ();
		StartCoroutine (enemyControlCoroutine);
	}

	protected abstract void InitializeEnemy();

	//This should be FixedUpdate.  
	protected virtual IEnumerator EnemyControl() {
		//Continuously
		while (true) {
			//Check to see whether player is within radius. 
			if (Vector2.Distance(transform.position, player.transform.position) <= playerViewableThreshold) {

				//Calculate the distance from each respective safe zone.  
				float distanceFromLeftSafeZone = transform.position.x - (player.transform.position.x - playerAttackDistance);
				float distanceFromRightSafeZone = transform.position.x - (player.transform.position.x + playerAttackDistance);

				//If we are in a safe zone, either the left, right, or neither.  
				if (Mathf.Abs(distanceFromLeftSafeZone) <= ignorePlayerMovementThreshold || Mathf.Abs(distanceFromRightSafeZone) <= ignorePlayerMovementThreshold) {
					//Flip to face the player and attack.  
					Stop();
					FlipToFacePlayer();
					//Attack if the y values between the enemy and the player are close enough.  
					if (Mathf.Abs(player.transform.position.y - transform.position.y) <= maxYValueSeparation) {
						anim.SetTrigger("Attack");
						Attack ();
					}
					yield return new WaitForSeconds(1.5f);

				} else {
					//We are not in either safe zone.  

					//This will hold the eventual value of the target safe zone.  
					float distanceFromTargetSafeZone = 0;

					//Give target safe zone a value.  
					if (Mathf.Abs(distanceFromLeftSafeZone) <= Mathf.Abs(distanceFromRightSafeZone)) {
						distanceFromTargetSafeZone = distanceFromLeftSafeZone;
					} else {
						distanceFromTargetSafeZone = distanceFromRightSafeZone;
					}

					//Calculate how to flip based on the distance from the target safe zone.  
					//If we are to the left of the safe zone.  
					//Example: We are at 7, and the left safe zone at 1.  7 - 1 is positive, and we are to the left of the safe zone.  
					if (distanceFromTargetSafeZone < 0) {
						//If we are facing left
						if (GetFacingDirection() == -1)
							//Flip to face the right side.  
							Flip ();
					} else if (distanceFromTargetSafeZone >= 0) {
						//If we are facing right.  
						if (GetFacingDirection() == 1) 
							//Flip to face the left side.  
							Flip ();
					}

					//Start moving toward the target safe zone (we have already flipped to the position
					anim.SetFloat("Speed", 1);
					//Yield returning a coroutine makes it wait until the coroutine is completed.  
					yield return StartCoroutine(MaintainAConstantXVelocity(0.3f));

					//In the event that the x velocity is very small, jump.  
					if (Mathf.Abs (rb2d.velocity.x) < maxSpeed / 100f && grounded) {
						InitializeJump(1);
						//Wait until we are in the air.  
						//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
						yield return new WaitForSeconds(0.3f);
						//Start moving forward again (mid-air).  
						anim.SetFloat("Speed", 1);
						yield return StartCoroutine(MaintainAConstantXVelocity(0.3f));
					}

				}
			} else {
				//We are not viewable by the player.  
				Stop ();
				//Wait a couple seconds instead of a frame (processing reasons).  
				yield return new WaitForSeconds(3);
			}

			//Every frame
			//yield return new WaitForFixedUpdate();
		}

	}

	//Used to flip to face the player.  
	protected void FlipToFacePlayer() {
		if (player.position.x >= transform.position.x) {
			if (GetFacingDirection() != 1) {
				Flip ();
			}
		} else {
			if (GetFacingDirection() != -1) {
				Flip();
			}
		}
	}

	//The Combatant Interface methods
	protected abstract void Attack();

}
