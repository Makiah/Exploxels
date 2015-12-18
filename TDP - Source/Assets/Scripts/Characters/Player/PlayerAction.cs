
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

public class PlayerAction : CharacterBaseActionClass {

	private bool touchingWall;
	private Transform wallCheck;

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Player");
		wallCheck = transform.FindChild("FlippingItem").FindChild ("WallCheck");

		base.SetReferences ();

		StartCoroutine (CheckForWeaponInput());
		StartCoroutine (ListenForArrowMovement());
	}

	//Used to check whether or not player is grounded, touching a wall, etc.  Defines movements.  
	protected override IEnumerator CheckCharacterPhysics() {
		while (true) {
			grounded = CheckWhetherGrounded();
			//Debug.DrawLine(groundCheck.position + new Vector3(groundedOffset, 0, 0), groundCheck.position - new Vector3(groundedOffset, 0, 0));
			touchingWall = Physics2D.Linecast (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

			if (grounded) {
				jumpInEffect = 0;
				anim.SetInteger ("JumpInEffect", 0);
			} else {
				anim.SetInteger ("JumpInEffect", 1);
			}

			if (Input.GetButtonDown ("Jump")) {
				if (grounded) {
					InitializeJump (1);
				} else if (touchingWall) {
					InitializeJump (3);
				} else if (jumpInEffect == 1 && !touchingWall) {
					InitializeJump (2);
				} else if (jumpInEffect == 0 && !grounded) {
					InitializeJump (2);
				}
			}

			yield return null;
		}

	}

	//Movement based on arrow keys
	IEnumerator ListenForArrowMovement () { 
		while (true) {
			//This gets the current state of the pressed keys.  
			float h = Input.GetAxis ("Horizontal");
			anim.SetFloat ("Speed", Mathf.Abs (h));

			//Check velocity, and change accordingly.  
			if (Mathf.Abs(rb2d.velocity.x) > maxSpeed) 
				//Changes the velocity of the player to just the max speed if it is going too fast.  
				//By using Mathf.Sign(rb2d.velocity.x) instead of GetFacingDirection(), the character cannot switch directions in midair.  
				rb2d.velocity = new Vector2 (Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
			else
				//The additional variable makes it so the player moves slower when in a jump.  
				rb2d.AddForce (Vector2.right * moveForce * h * 1 / (0.01f * jumpInEffect + 1));

			//Control flipping based on the arrow keys.  
			if (h > 0 && !facingRight) 
				Flip ();
			else if (h < 0 && facingRight) 
				Flip ();

			transform.FindChild("Main Camera").FindChild("Background").FindChild("Background Tiles").GetComponent <BackgroundScroller> ().Movement(rb2d.velocity.x / maxSpeed);

			yield return new WaitForFixedUpdate();

		}
	}
	

	/************************************************* ATTACKING *********************************************************/

	string[] knownAttackKeys = {"Stab", "OverheadSlice", "ShootBow", "CreatePhysicalItem"};
	
	//The Update() method that will check whether the dictionary requirements for some attack have been met.  The code that sets the dictionary 
	//is in the costume manager class.  
	IEnumerator CheckForWeaponInput() {
		//Unless the possible attack dictionary is empty,
		while (true) {
			if (jumpInEffect == 0 && Input.GetAxisRaw("Horizontal") == 0) {
				if (possibleWeaponMoves != null) {
					foreach (string attackKey in knownAttackKeys) {
						if (possibleWeaponMoves.ContainsKey (attackKey)) {
							string outValue;
							possibleWeaponMoves.TryGetValue (attackKey, out outValue);
							if (GetActionBooleanFromString (outValue)) {
								AttackAction (attackKey);
							}
						}
					}
				}
			}
			//For every frame.  
			yield return null;
		}
	}

	bool GetActionBooleanFromString(string actionKey) {
		if (grounded) {
			switch (actionKey) {
			case "MouseButtonDown0":
				return Input.GetMouseButtonDown (0);
			case "MouseButtonDown1":
				return Input.GetMouseButtonDown (1);
			default:
				return false;
			}
		} else {
			return false;
		}
	}

}
