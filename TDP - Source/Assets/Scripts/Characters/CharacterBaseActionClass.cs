
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
 * 
 * 9/14 - Renamed to HumanoidBaseActionClass.  
 * 
 * 9/27 - Maybe this could be a class for all characters (including quadripedal characters), seeing as how there is nothing specifically humanoid.  
 * 
 * This script is the base class for all characters in the game (such as the skeleton, the player, zombies, etc.).  It controls movement, maxSpeed, 
 * moveForce, wall jumping, etc.  Almost all of the methods are overridable, and accessible by child classes.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CharacterBaseActionClass : MonoBehaviour {

	/********************************************** INITIALIZATION ******************************************************/
	
	protected virtual void OnEnable() {
		LevelEventManager.InitializePlayer += SetReferences;
	}
	
	protected virtual void OnDisable() {
		LevelEventManager.InitializePlayer -= SetReferences;
	}
	
	
	/************************************************* MOVEMENT *********************************************************/
	
	//Key: 0 = None in effect, 1 = Initial in effect, 2 = Double in effect, 3 = Wall in effect
	protected int jumpInEffect = 0;
	
	public float maxSpeed;
	protected float maxSpeedInitial;
	public float moveForce;
	public float jumpForce;
	public float wallJumpForce;
	
	protected bool facingRight = true;
	protected bool grounded;
	public float groundedOffset;
	
	protected Transform characterSpriteObject;
	protected Transform groundCheck;
	
	protected Animator anim;
	protected Rigidbody2D rb2d;

	//Only meant for bow script, a more elegant solution could be used later.  
	[HideInInspector]
	public string characterName;
	
	protected virtual void SetReferences() {
		anim = characterSpriteObject.GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();
		groundCheck = transform.FindChild ("GroundCheck");

		maxSpeedInitial = maxSpeed;

		//This changes based on the override methods.  
		StartCoroutine ("CheckCharacterPhysics");
	}
	
	protected virtual IEnumerator CheckCharacterPhysics() {
		while (true) {
			grounded = Physics2D.Linecast (groundCheck.position, transform.position, 1 << LayerMask.NameToLayer ("Ground"));
			
			if (grounded) {
				jumpInEffect = 0;
				anim.SetInteger ("JumpInEffect", 0);
			} else {
				anim.SetInteger ("JumpInEffect", 1);
			}
			
			yield return null;
		}
		
	}
	
	protected virtual void InitializeJump(int jumpStyle) {
		
		anim.SetInteger("JumpInEffect", jumpStyle);
		
		switch (jumpStyle) {
		case 0: 
			break;
		case 1: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			jumpInEffect = 1;
			break;
		case 2: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			jumpInEffect = 2;
			break;
		case 3: 
			rb2d.velocity = new Vector2(0, rb2d.velocity.y);
			rb2d.velocity = new Vector2(wallJumpForce *-GetFacingDirection(), jumpForce);
			Flip ();
			jumpInEffect = 3;
			break;
		default: 
			break;
		}
		
	}

	//Changes the scale of FlippingItem
	protected void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.FindChild("FlippingItem").localScale;
		theScale.x *= -1;
		transform.FindChild ("FlippingItem").localScale = theScale;
	}
	
	//Called by HatchetScript for reversing the linecast.  
	public int GetFacingDirection() {
		return facingRight ? 1 : -1;
	}
	
	
	/************************************************* ATTACKING *********************************************************/

	//This dictionary contains the possible weapon moves for the player.  The first entry contains the required action to trigger the action, and the second
	//includes a string of the method.  
	protected Dictionary <string, string> possibleWeaponMoves;
	
	protected ItemBase itemInUseByCharacter;
	
	protected bool currentlyInAttackAnimation = false;
	
	public delegate void ActionAfterCompletedAnimation ();
	public event ActionAfterCompletedAnimation ActionsAfterAnimation;
	
	//This will be called by the item management part of the costume manager script
	public void OnRefreshCurrentWeaponMoves(ItemBase ctorItemInUseByCharacter) {
		itemInUseByCharacter = ctorItemInUseByCharacter;
		if (ctorItemInUseByCharacter != null) {
			possibleWeaponMoves = itemInUseByCharacter.GetPossibleActionsForItem ();
		} else {
			possibleWeaponMoves = null;
		}
	}
	
	protected void AttackAction(string someAttackKey) {
		if (!currentlyInAttackAnimation) {
			anim.SetTrigger (someAttackKey);
			itemInUseByCharacter.InfluenceEnvironment (someAttackKey);
			currentlyInAttackAnimation = true;
		} else {
			Debug.Log("Was in attack animation, did not attack");
		}
	}
	
	public bool CheckCurrentAttackAnimationState() {
		return currentlyInAttackAnimation;
	}
	
	//Only called by costume manager.  
	public void ResetCurrentAttackAnimationState() {
		if (ActionsAfterAnimation != null) {
			ActionsAfterAnimation ();
		}
		ActionsAfterAnimation = null;
		currentlyInAttackAnimation = false;
	}

	/**************** CHARACTER UTILITIES ***********************/

	//Used for NPCs.  
	protected IEnumerator MaintainAConstantXVelocity(int velocity) {
		while (true) {
			if (rb2d.velocity.x != velocity)
				rb2d.velocity = new Vector2(velocity, rb2d.velocity.y);

			yield return null;
		}
	}


}
