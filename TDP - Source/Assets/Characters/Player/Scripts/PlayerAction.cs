
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls all of the player-based movement that occurs over the course of the game.  This is a subclass of CharacterBaseActionClass, 
 * yet most of the base classes methods are overridden, as well as a few new methods that allow for shooting, slashing, etc.  
 * 
 * 
 */


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerAction : CharacterBaseActionClass, ICanHoldItems {

	private bool touchingWall;
	private Transform wallCheck;

	protected override void InitializeCharacter() {
		wallCheck = transform.FindChild("FlippingItem").FindChild ("WallCheck");

		StartCoroutine (CheckForWeaponInput());
		StartCoroutine (ListenForArrowMovement());
	}

	//Used to check whether or not player is grounded, touching a wall, etc.  Defines movements.  
	protected override IEnumerator CheckCharacterPhysics() {
		while (true) {
			//Update the grounded boolean.  
			grounded = CheckWhetherGrounded();
			//Debug.DrawLine(groundCheck.position + new Vector3(groundedOffset, 0, 0), groundCheck.position - new Vector3(groundedOffset, 0, 0));
			touchingWall = Physics2D.Linecast (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

			//The ground checks should be extremely close to the player, or it appears as grounded on the next frame.  
			if (grounded && jumpInEffect != 0) {
				InitializeJump (0);
			} 

			//In case the player is in the air (not jumping, just falling)
			if (grounded == false && jumpInEffect == 0) {
				//No force should be added, so this is done manually. 
				Debug.Log("Set jumpInEffect to 1");
				jumpInEffect = 1;
				anim.SetInteger ("JumpInEffect", 1);
			}

			//When the player wants to jump.  
			if (Input.GetButtonDown ("Jump")) {
				//The order of these conditions is important.  
				if (jumpInEffect == 0)
					InitializeJump (1);
				else if (touchingWall)
					InitializeJump (3);
				else if (jumpInEffect == 1)
					InitializeJump (2);
			}

			//Every frame.  
			yield return null;
		}

	}

	//Movement based on arrow keys
	IEnumerator ListenForArrowMovement () { 
		//Prevent constant boxing and unboxing.  
		float h = 0;

		//Constantly
		while (true) {
			//This gets the current state of the pressed keys.  
			h = Input.GetAxis ("Horizontal");
			anim.SetFloat ("Speed", Mathf.Abs (h));

			rb2d.AddForce (Vector2.right * moveForce * h * 1 / (2f * jumpInEffect + 1));

			rb2d.velocity = new Vector2 (Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);

			//Control flipping based on the arrow keys.  
			if (h > 0 && !facingRight) 
				Flip ();
			else if (h < 0 && facingRight) 
				Flip ();

			//Tell the camera that the player is moving (should be changed at some point.  
			transform.FindChild("Main Camera").FindChild("Background").GetComponent <BackgroundManager> ().MoveBackground(rb2d.velocity.x / maxSpeed);

			yield return new WaitForFixedUpdate();

		}
	}

	//Used for weapons.  
	public void ExternalJumpAction (int num) {
		InitializeJump (num);
	}

	/************************************************ ITEM STUFF *************************************************************/
	//This dictionary contains the possible weapon moves for the player.  The first entry contains the required action to trigger the action, and the second
	//includes a string of the method.  
	private MovementAndMethod[] possibleWeaponMoves;

	private ItemBase itemInUseByCharacter;

	//This will be called by the item management part of the costume manager script
	public void OnRefreshCurrentWeaponMoves(ItemBase ctorItemInUseByCharacter) {
		itemInUseByCharacter = ctorItemInUseByCharacter;
		if (ctorItemInUseByCharacter != null) {
			possibleWeaponMoves = itemInUseByCharacter.GetPossibleActionsForItem ();
		} else {
			possibleWeaponMoves = null;
		}
	}

	//Has to be public for the interface
	public void AttackAction(MovementAndMethod someAttack) {
		if (!currentlyInAttackAnimation) {
			anim.SetTrigger (someAttack.GetActionKey());
			itemInUseByCharacter.InfluenceEnvironment (someAttack.GetActionEnum());
			if (someAttack.GetActionEnum() != MovementAndMethod.PossibleMovements.CreatePhysicalItem)
				currentlyInAttackAnimation = true;
		} else {
			Debug.Log("Was in attack animation, did not attack");
		}
	}
		
	//The coroutine method that will check whether the dictionary requirements for some attack have been met.  The code that sets the array (above) 
	//is in the costume manager class.  
	IEnumerator CheckForWeaponInput() {
		//Unless the possible attack dictionary is empty,
		while (true) {
			if (itemInUseByCharacter != null) {
				//Works due to short-circuiting.  
				if (possibleWeaponMoves != null && possibleWeaponMoves.Length != 0) {
					if (currentlyInAttackAnimation == false) {
						for (int i = 0; i < possibleWeaponMoves.Length; i++) {
							//If the can be used while midair is false, then it will only work while grounded is true.  Vice versa is also the case.  
							if (possibleWeaponMoves [i].GetCanBeUsedWhileMidair () == ! grounded) {
								if (possibleWeaponMoves [i].GetTriggerHasOccurred ()) {
									AttackAction (possibleWeaponMoves [i]);
								}
							}
						}
					}
				} else {
					Debug.LogError ("Possible weapon moves of " + itemInUseByCharacter.gameObject.name + " is null");
				}
			}

			//For every frame.  
			yield return null;
		}
	}
}
