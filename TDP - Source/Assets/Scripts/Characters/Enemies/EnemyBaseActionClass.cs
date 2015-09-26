
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

public abstract class EnemyBaseActionClass : HumanoidBaseActionClass {

	//The maximum distance that the player can be away from the enemy for it to activate.  
	public float playerViewableThreshold;
	//The safe distance to stay away from the player.  
	public float remainDistanceFromPlayer;
	//Once in the safe zone, how much closer can the player come before moving again?
	public float ignorePlayerMovementThreshold;
	//What is the maximum difference in Y values the enemies must have to attack?
	public float maxYValueSeparation;

	//The player transform
	protected Transform player;

	protected virtual IEnumerator BasicEnemyControl() {
		while (true) {
			if (Vector3.Distance(transform.position, player.transform.position) <= playerViewableThreshold) {

				//Active character health controller.  
				if (GetComponent <CharacterHealthController> ().GetHealthPanelState() == false) {
					GetComponent <CharacterHealthController> ().OnThisEnemyActivated();
				}
				
				float distanceFromLeftPointX = Mathf.Abs(transform.position.x - (player.transform.position.x - remainDistanceFromPlayer));
				float distanceFromRightPointX = Mathf.Abs(transform.position.x - (player.transform.position.x + remainDistanceFromPlayer));

				if (distanceFromLeftPointX <= distanceFromRightPointX) {
					//Flip if some point to the left of the player is further right than the skeleton.  
					if (player.position.x - remainDistanceFromPlayer >= transform.position.x) {
						if (GetFacingDirection() != 1) {
							Flip ();
						}
					} else {
						if (GetFacingDirection() != -1) {
							Flip();
						}
					}
				} else {
					if (player.position.x + remainDistanceFromPlayer >= transform.position.x ) {
						if (GetFacingDirection() != 1) {
							Flip ();
						}
					} else {
						if (GetFacingDirection() != -1) {
							Flip();
						}
					}
				}
				
				if (Mathf.Abs(rb2d.velocity.x) < 1 && grounded) {
					InitializeJump(1);
				}
				
				anim.SetFloat("Speed", 1);
				yield return new WaitForSeconds(.3f);
				rb2d.velocity = new Vector3(moveForce * GetFacingDirection(), rb2d.velocity.y, 0);
				yield return null;

				if (Mathf.Abs(player.transform.position.y - transform.position.y) <= maxYValueSeparation) {
					//Check if the current position is within the boundaries of the safe zone.  
					if (distanceFromLeftPointX <= distanceFromRightPointX) {
						if (distanceFromLeftPointX <= ignorePlayerMovementThreshold) {
							FlipToFacePlayer();
							rb2d.velocity = new Vector2(0, rb2d.velocity.y);
							anim.SetFloat("Speed", 0);
							Attack ();
							yield return new WaitForSeconds(2.5f);
						}
					} else {
						if (distanceFromRightPointX <= ignorePlayerMovementThreshold) {
							FlipToFacePlayer();
							rb2d.velocity = new Vector2(0, rb2d.velocity.y);
							anim.SetFloat("Speed", 0);
							Attack ();
							yield return new WaitForSeconds(2.5f);
						}
					}
				}

			} else {
				anim.SetFloat("Speed", 0);
				rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0);
				//De-activate health controller.  
				if (GetComponent <CharacterHealthController> ().GetHealthPanelState() == true) {
					GetComponent <CharacterHealthController> ().OnThisEnemyDeActivated();
				}
				yield return null;
			}
			
		}

	}

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
