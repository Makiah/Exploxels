
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

public abstract class EnemyBaseActionClass : CharacterBaseActionClass {

	/******************************************** INITIALIZATION *******************************************/
	
	protected override void OnEnable() {
		LevelEventManager.InitializeEnemies += SetReferences;
	}
	
	protected override void OnDisable() {
		LevelEventManager.InitializeEnemies -= SetReferences;
	}

	//The maximum distance that the player can be away from the enemy for it to activate.  
	public float playerViewableThreshold;
	//When at this distance, the enemy will attack.  
	public float playerAttackDistance;
	//If the enemy is within the safe attack distance + the movement threshold, it will remain stationary.  
	public float ignorePlayerMovementThreshold;
	//What is the maximum difference in Y values the enemies must have to attack?
	public float maxYValueSeparation;

	//The player transform
	protected Transform player;

	protected override void SetReferences() {
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;

		base.SetReferences ();

		StartCoroutine ("EnemyControl");
	}

	protected virtual IEnumerator BasicEnemyControl() {
		while (true) {

			if (Vector2.Distance(transform.position, player.transform.position) <= playerViewableThreshold) {

				Debug.Log(gameObject.name + " can see the player.");

				Stop ();

				float distanceFromLeftPointX = Mathf.Abs(transform.position.x - (player.transform.position.x - ignorePlayerMovementThreshold));
				float distanceFromRightPointX = Mathf.Abs(transform.position.x - (player.transform.position.x + ignorePlayerMovementThreshold));

				if (distanceFromLeftPointX <= distanceFromRightPointX) {
					//Flip if some point to the left of the player is further right than the skeleton.  
					if (player.position.x - ignorePlayerMovementThreshold >= transform.position.x && GetFacingDirection() == -1) {
						Flip ();
					} else if (player.position.x - ignorePlayerMovementThreshold >= transform.position.x && GetFacingDirection() == 1) {
						Flip();
					} else {
						if (Mathf.Abs(rb2d.velocity.x) < 0.6f && grounded) {
							Debug.Log("Jumped");
							InitializeJump(1);
							yield return new WaitForSeconds(0.3f);
						}
					}
				} else {
					//You should not jump if you are going to change direction.  
					//If facing right and the player is farther right than the attack distance
					if (player.position.x - transform.position.x >= ignorePlayerMovementThreshold && GetFacingDirection() == 1) {
						Flip ();
					} else if (player.position.x + transform.position.x >= ignorePlayerMovementThreshold && GetFacingDirection() != -1) {
						Flip ();
					} else {
						//Jump if the enemy is not going to change direction.  
						if (Mathf.Abs(rb2d.velocity.x) < 0.3f && grounded) {
							Debug.Log("Jumped");
							InitializeJump(1);
						}
					}
				}
					
				//Check if the current position is within the boundaries of the attack zone, and the y value is acceptable.  
				if ((distanceFromLeftPointX <= playerAttackDistance || distanceFromRightPointX <= playerAttackDistance) && (Mathf.Abs(player.transform.position.y - transform.position.y) <= maxYValueSeparation)) {
					if (distanceFromLeftPointX <= distanceFromRightPointX) {
						Stop ();
						FlipToFacePlayer();
						Attack ();
						yield return new WaitForSeconds(2.5f);
					} else {
						Stop ();
						FlipToFacePlayer();
						Attack ();
						yield return new WaitForSeconds(2.5f);
					}
				} else {
					//Speed management.  
					Debug.Log("Moving");
					anim.SetFloat("Speed", 1);
					rb2d.velocity = new Vector3(moveForce * GetFacingDirection(), rb2d.velocity.y, 0);
					yield return new WaitForSeconds(3f);
				}

			} else {
				//Remain stopped, if not within the viewable radius.  
				Stop();
				yield return new WaitForSeconds(1);
			}
			
		}

	}

	protected virtual IEnumerator EnemyControl() {
		//PSEUDOCODE
		//Check to make sure the enemy can see the player.  
			//If it can, check whether it is in the safe zone.  
				//Flip to face the player and attack.  
				//Wait a couple seconds.  
			//Otherwise, flip toward the direction of the nearest safe zone (On either side of the player)
				//Start moving.  
				//Wait for a second.  
				//If the velocity is still 0, jump.  
					//Wait for part of a second.  
					//Move again.  
					//Wait for part of a second.  
		//Otherwise stop moving.  

		//ACTUAL CODE

		//Continuously
		while (true) {
			//Check to see whether player is within radius. 
			if (Vector2.Distance(transform.position, player.transform.position) <= playerViewableThreshold) {

				//Calculate the distance from each respective safe zone.  
				float distanceFromLeftSafeZone = transform.position.x - (player.transform.position.x - ignorePlayerMovementThreshold);
				float distanceFromRightSafeZone = transform.position.x - (player.transform.position.x + ignorePlayerMovementThreshold);

				//If we are in a safe zone, either the left, right, or neither.  
				if (Mathf.Abs(distanceFromLeftSafeZone) <= ignorePlayerMovementThreshold || Mathf.Abs(distanceFromRightSafeZone) <= ignorePlayerMovementThreshold) {
					//Flip to face the player and attack.  
					Stop();
					FlipToFacePlayer();
					Attack ();
					Debug.Log("Attacking");
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
					rb2d.velocity = new Vector2(GetFacingDirection() * moveForce, rb2d.velocity.y);
					yield return new WaitForSeconds(1f);

					//In the event that the x velocity is very small, jump.  
					if (rb2d.velocity.x < 0.5f) {
						InitializeJump(1);
						//Wait until we are in the air.  
						//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
						yield return new WaitForSeconds(0.3f);
						//Start moving forward again (mid-air).  
						anim.SetFloat("Speed", 1);
						rb2d.velocity = new Vector2(GetFacingDirection() * moveForce, rb2d.velocity.y);
						yield return new WaitForSeconds(1f);
					}

				}
			} else {
				//We are not viewable by the player.  
				Stop ();
				//Wait a couple seconds instead of a frame (processing reasons).  
				yield return new WaitForSeconds(5);
			}

			//Every frame
			yield return null;
		}

	}

	//Used to flip to face the player.  
	void FlipToFacePlayer() {
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

	protected abstract void Attack();

}
