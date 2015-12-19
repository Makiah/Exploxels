
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
using System;
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
	protected Transform[] groundChecks;
	
	protected Animator anim;
	protected Rigidbody2D rb2d;

	//For combat (has to be here so that weapons can use it)
	private readonly string characterGUID = Guid.NewGuid().ToString();
	
	protected virtual void SetReferences() {
		//Get required components
		Debug.Log(gameObject.name + " is " + GetCombatantID());
		anim = characterSpriteObject.GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();
		groundChecks = GetAllGroundChecks ();

		maxSpeedInitial = maxSpeed;

		//This changes based on the override methods.  
		StartCoroutine (CheckCharacterPhysics());
	}

	private Transform[] GetAllGroundChecks() {
		//The transform that holds all of the ground check transforms.  
		Transform groundCheckParent = transform.FindChild ("FlippingItem").FindChild ("GroundChecks");

		//Will contain all of the ground checks.  
		List <Transform> groundCheckList = new List<Transform>();

		int i = 1; 
		//Loop through all ground checks until one does not exist.  
		while (groundCheckParent.FindChild("GroundCheck" + i)) {
			groundCheckList.Add(groundCheckParent.FindChild("GroundCheck" + i));
			i++;
		}

		return groundCheckList.ToArray ();
	}

	//Used to check grounded state.  
	protected virtual IEnumerator CheckCharacterPhysics() {
		while (true) {
			grounded = CheckWhetherGrounded();
			
			if (grounded) {
				jumpInEffect = 0;
				anim.SetInteger ("JumpInEffect", 0);
			} else {
				anim.SetInteger ("JumpInEffect", 1);
			}
			
			yield return null;
		}
		
	}

	//Use the grounded boolean instead
	protected bool CheckWhetherGrounded() {
		//Loop through the whole array and look for a true.  
		for (int i = 0; i < groundChecks.Length; i++) {
			if (Physics2D.Linecast (groundChecks[i].position, transform.position, 1 << LayerMask.NameToLayer ("Ground")))
				return true;
		}

		return false;
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
	
	//Used to stop the character.  
	protected void Stop() {
		anim.SetFloat("Speed", 0);
		rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0);
	}
	
	
	/************************************************* ATTACKING *********************************************************/
	//Used for both the player and the enemies.  
	protected bool currentlyInAttackAnimation = false;

	//This delegate is called after the animation completes.  
	public delegate void ActionAfterCompletedAnimation ();
	public event ActionAfterCompletedAnimation ActionsAfterAnimation;
	
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
	//This coroutine keeps enemies and NPCs at a constant velocity for a given duration of time (better than infinite mass).  
	protected IEnumerator MaintainAConstantXVelocity(float time) {
		//Controls the time that it runs for.  
		float currentTime = 0;
		while (currentTime <= time) {
			//Increment time.  
			currentTime += Time.deltaTime;
			//Check velocity, and change accordingly.  
			if (Mathf.Abs (rb2d.velocity.x) >= maxSpeed) 
				//By using Mathf.Sign(rb2d.velocity.x) instead of GetFacingDirection(), the character cannot switch directions in midair.  
				rb2d.velocity = new Vector2 (Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
			else
				rb2d.AddForce (Vector2.right * moveForce * GetFacingDirection());

			//Wait for the fixed update (has to be done on all physics-related coroutines).
			yield return new WaitForFixedUpdate();
		}
	}

	//When knockback should be applied to the character.  
	public void ApplyKnockbackToCharacter(Vector2 force) {
		//Maintaining a constant velocity would interfere with this.  
		rb2d.AddForce (force);
		Debug.Log ("Force of " + force + " applied as knockback to " + gameObject.name);
	}

	//Combatant stuff.  
	public string GetCombatantID() {
		return characterGUID;
	}

	//Used for stuff like the transform.  
	public CharacterBaseActionClass GetActualClass() {
		return this;
	}

}
