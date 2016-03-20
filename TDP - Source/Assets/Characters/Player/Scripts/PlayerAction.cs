
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

	private bool touchingWall = false;
	private bool playerCoroutinesCurrentlyActive = true;
	private Transform wallCheck;

	//Used so when the player is in between two close walls, he/she automatically goes up by just pressing the up arrow.  
	private bool lastJumpWasWallJump = false;

	IEnumerator weaponInputCoroutine, arrowMovementCoroutine;

	protected override void InitializeCharacter() {
		//No other character has a wall check, so only the player uses this property.  
		wallCheck = transform.FindChild("FlippingItem").FindChild ("WallCheck");

		//Start the coroutines required for the player.  
		weaponInputCoroutine = CheckForWeaponInput ();
		arrowMovementCoroutine = ListenForArrowMovement ();
		StartCoroutine (weaponInputCoroutine);
		StartCoroutine (arrowMovementCoroutine);
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
				jumpInEffect = 1;
				anim.SetInteger ("JumpInEffect", 1);
			}

			//When the player wants to jump.  
			if (playerCoroutinesCurrentlyActive && Input.GetButtonDown ("Jump")) {
				//The order of these conditions is important.  
				if (jumpInEffect == 0)
					InitializeJump (1);
				else if (touchingWall && (lastJumpWasWallJump || (Input.GetAxis("Horizontal") != 0 && Mathf.Sign(Input.GetAxis("Horizontal")) == GetFacingDirection())))
					//Make sure that the player is trying to wall jump before actually wall jumping.  
					InitializeJump (3);
				else if (jumpInEffect == 1)
					InitializeJump (2);
			}

			//Every frame.  
			yield return null;
		}

	}

	protected override void InitializeJump(int jumpStyle) {
		//Jumping parameters
		anim.SetInteger("JumpInEffect", jumpStyle);
		jumpInEffect = jumpStyle;

		//Add forces based on the jump style.  
		switch (jumpStyle) {
		case 0: 
			break;
		case 1: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			lastJumpWasWallJump = false;
			break;
		case 2: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			lastJumpWasWallJump = false;
			break;
		case 3: 
			rb2d.velocity = new Vector2 (wallJumpForce * -GetFacingDirection (), jumpForce);
			lastJumpWasWallJump = true;
			Flip ();
			break;
		case 4: 
			rb2d.velocity = new Vector2 (0, -6);
			lastJumpWasWallJump = false;
			break;
		default: 
			Debug.LogError ("Invalid jumpStyle of " + jumpStyle + " input");
			break;
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

	/**************************************** OTHER STUFF ******************************************/
	public void DisablePlayerActions() {
		if (playerCoroutinesCurrentlyActive) {
			//Disable the coroutines.  
			StopCoroutine (arrowMovementCoroutine);
			StopCoroutine (weaponInputCoroutine);
			playerCoroutinesCurrentlyActive = false;
			anim.SetFloat ("Speed", 0);
		} else {
			Debug.Log ("Cannot disable coroutines: Coroutines are already disabled");
		}
	}

	public void EnablePlayerActions() {
		if (playerCoroutinesCurrentlyActive == false) {
			//Enable the coroutines.  
			arrowMovementCoroutine = ListenForArrowMovement ();
			weaponInputCoroutine = CheckForWeaponInput ();
			StartCoroutine (arrowMovementCoroutine);
			StartCoroutine (weaponInputCoroutine);
			playerCoroutinesCurrentlyActive = true;
		} else {
			Debug.Log ("Cannot enable coroutines: Coroutines are already active.");
		}
	}
}
