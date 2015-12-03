
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

		StartCoroutine ("BasicEnemyControl");
	}

	protected virtual IEnumerator BasicEnemyControl() {
		while (true) {

			if (Vector2.Distance(transform.position, player.transform.position) <= playerViewableThreshold) {

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
