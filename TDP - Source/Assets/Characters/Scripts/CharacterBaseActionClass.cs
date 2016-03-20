
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

	//For child classes that need to stop the coroutine.  
	protected IEnumerator characterPhysicsCoroutine;
	
	protected void SetReferences() {
		//Get required components
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild("Character");
		anim = characterSpriteObject.GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();
		groundChecks = GetAllGroundChecks ();

		maxSpeedInitial = maxSpeed;

		//All child classes should have this method.  
		InitializeCharacter ();

		//This changes based on the override methods.  
		characterPhysicsCoroutine = CheckCharacterPhysics ();
		StartCoroutine (CheckCharacterPhysics());

	}

	//Abstract method.  
	protected abstract void InitializeCharacter();

	//Searches for all ground checks in the child transform.  
	private Transform[] GetAllGroundChecks() {
		//The transform that holds all of the ground check transforms.  
		Transform groundCheckParent = transform.FindChild ("FlippingItem").FindChild ("GroundChecks");

		//Will contain all of the ground checks.  
		List <Transform> groundCheckList = new List<Transform>();

		int i = 1; 
		//Loop through all ground checks until one does not exist.  
		while (groundCheckParent.FindChild("GroundCheck" + i) != null) {
			groundCheckList.Add(groundCheckParent.FindChild("GroundCheck" + i));
			i++;
		}

		return groundCheckList.ToArray ();
	}

	//Used to check grounded state.  
	protected virtual IEnumerator CheckCharacterPhysics() {
		while (true) {
			//Update grounded state.  
			grounded = CheckWhetherGrounded();
			//Check the required conditions to set the animation state as grounded (idle or walking).  
			if (grounded && jumpInEffect != 0) {
				jumpInEffect = 0;
				anim.SetInteger ("JumpInEffect", 0);
			} else if (!grounded && jumpInEffect == 0) {
				jumpInEffect = 1;
				anim.SetInteger ("JumpInEffect", 1);
			}
			
			yield return new WaitForFixedUpdate();
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
		//Jumping parameters
		anim.SetInteger("JumpInEffect", jumpStyle);
		jumpInEffect = jumpStyle;

		//Add forces based on the jump style.  
		switch (jumpStyle) {
		case 0: 
			break;
		case 1: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			break;
		case 2: 
			rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpForce);
			break;
		case 3: 
			rb2d.velocity = new Vector2 (wallJumpForce * -GetFacingDirection (), jumpForce);
			Flip ();
			break;
		case 4: 
			rb2d.velocity = new Vector2 (0, -6);
			break;
		default: 
			Debug.LogError ("Invalid jumpStyle of " + jumpStyle + " input");
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
	public delegate void ActionAnimation ();
	public event ActionAnimation ActionsOnAttack;
	public event ActionAnimation ActionsAfterCompletedAnimation;
	
	public bool CheckCurrentAttackAnimationState() {
		return currentlyInAttackAnimation;
	}

	//Attacking method.  
	public void OnAttack() {
		if (ActionsOnAttack != null) {
			ActionsOnAttack ();
			ActionsOnAttack = null;
		}
	}

	//Only called by costume manager.  
	public void OnAttackAnimationCompleted() {
		//Make sure that the attack animation has already been run (it will exit immediately if the delegate is already null.  
		OnAttack();
		if (ActionsAfterCompletedAnimation != null) {
			ActionsAfterCompletedAnimation ();
			ActionsAfterCompletedAnimation = null;
		}
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
			rb2d.AddForce (Vector2.right * moveForce * GetFacingDirection());
			rb2d.velocity = new Vector2 (Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed), rb2d.velocity.y);
			//Wait for the fixed update (has to be done on all physics-related coroutines).
			yield return new WaitForFixedUpdate();
		}
	}

	protected IEnumerator MaintainAConstantXVelocity(float time, float movingForce, float maximumSpeed) {
		//Controls the time that it runs for.  
		float currentTime = 0;
		while (currentTime <= time) {
			//Increment time.  
			currentTime += Time.deltaTime;
			//Check velocity, and change accordingly.  
			rb2d.AddForce (Vector2.right * movingForce * GetFacingDirection());
			rb2d.velocity = new Vector2 (Mathf.Clamp (rb2d.velocity.x, -maximumSpeed, maximumSpeed), rb2d.velocity.y);
			//Wait for the fixed update (has to be done on all physics-related coroutines).
			yield return new WaitForFixedUpdate();
		}
	}

	//When knockback should be applied to the character.  
	public void ApplyKnockback(Vector2 force) {
		//Maintaining a constant velocity would interfere with this.  
		rb2d.AddForce (force);
	}

	//Combatant stuff.  
	public string GetCombatantID() {
		return characterGUID;
	}

	//Used for stuff like the transform.  
	public CharacterBaseActionClass GetActualClass() {
		return this;
	}

	/***************************************** USED FOR POINTS WHERE THE ORIGINAL BEHAVIOUR WOULD BE OVERRIDDEN BY THE STORY *****************************************/

	//Used to make a character follow a transform (most likely the transform is also in motion).  
	public IEnumerator SetTargetTransform(Transform target, float acceptableDistance, float movingForce, float maximumSpeed) {
		//Continuously, until the player has been found.   (This includes both x and y, so there is no need for a change.  
		while (Vector2.Distance(target.position, transform.position) > acceptableDistance) {
			//Flip to face the target (most important part of the script).  
			FlipToFacePosition (target.position);

			//Start moving toward the target safe zone (we have already flipped to the position
			anim.SetFloat ("Speed", 1);
			//Yield returning a coroutine makes it wait until the coroutine is completed.  
			yield return StartCoroutine (MaintainAConstantXVelocity (1f, movingForce, maximumSpeed));

			//In the event that the x velocity is very small, jump.  
			if (Mathf.Abs (rb2d.velocity.x) < maximumSpeed / 100f && grounded) {
				InitializeJump (1);
				//Wait until we are in the air.  
				//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
				yield return new WaitForSeconds (0.3f);
				//Start moving forward again (mid-air).  
				anim.SetFloat ("Speed", 1);
				yield return StartCoroutine (MaintainAConstantXVelocity (0.3f, movingForce, maximumSpeed));
			}
		}

		anim.SetFloat ("Speed", 0);
	}

	public IEnumerator SetTargetTransform(Transform target, float acceptableDistance) {
		//Continuously, until the player has been found.   (This includes both x and y, so there is no need for a change.  
		while (Vector2.Distance(target.position, transform.position) > acceptableDistance) {
			//Flip to face the target (most important part of the script).  
			FlipToFacePosition (target.position);

			//Start moving toward the target safe zone (we have already flipped to the position
			anim.SetFloat ("Speed", 1);
			//Yield returning a coroutine makes it wait until the coroutine is completed.  
			yield return StartCoroutine (MaintainAConstantXVelocity (.3f, moveForce, maxSpeed));

			//In the event that the x velocity is very small, jump.  
			if (Mathf.Abs (rb2d.velocity.x) < maxSpeed / 100f && grounded) {
				InitializeJump (1);
				//Wait until we are in the air.  
				//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
				yield return new WaitForSeconds (0.3f);
				//Start moving forward again (mid-air).  
				anim.SetFloat ("Speed", 1);
				yield return StartCoroutine (MaintainAConstantXVelocity (0.3f, moveForce, maxSpeed));
			}
		}

		anim.SetFloat ("Speed", 0);
	}

	//Actual coroutine.  Yield return this coroutine in order to get to a position successfully.  Will probably have to be called to get to a certain position.  
	public IEnumerator SetTargetPosition(Vector2 target, float acceptableDistance, float movingForce, float maximumSpeed) {
		//Continuously, until the player has been found.   (This includes both x and y, so there is no need for a change.  
		while (Vector2.Distance(target, transform.position) > acceptableDistance) {
			//Flip to face the target (most important part of the script).  
			FlipToFacePosition (target);

			//Start moving toward the target safe zone (we have already flipped to the position
			anim.SetFloat ("Speed", 1);
			//Yield returning a coroutine makes it wait until the coroutine is completed.  
			yield return StartCoroutine (MaintainAConstantXVelocity (.3f, movingForce, maximumSpeed));

			//In the event that the x velocity is very small, jump.  
			if (Mathf.Abs (rb2d.velocity.x) < maximumSpeed / 100f && grounded) {
				InitializeJump (1);
				//Wait until we are in the air.  
				//At some point, consider calculating the time at which the jump is at it's highest point and then resuming, as opposed to some constant.  
				yield return new WaitForSeconds (0.3f);
				//Start moving forward again (mid-air).  
				anim.SetFloat ("Speed", 1);
				yield return StartCoroutine (MaintainAConstantXVelocity (0.3f, movingForce, maximumSpeed));
			}
		}

		anim.SetFloat ("Speed", 0);
		rb2d.velocity = Vector2.zero;
	}

	void FlipToFacePosition(Vector2 position) {
		if (position.x >= transform.position.x) {
			if (GetFacingDirection() != 1) {
				Flip ();
			}
		} else {
			if (GetFacingDirection() != -1) {
				Flip();
			}
		}
	}

}
