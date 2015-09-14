
/*
 * Author: Makiah Bennett
 * Last edited: 13 September 2015
 * 
 * 9/13 - Added EnemyActionBaseClass as base, set variables.  
 * 
 * This script controls the Skeleton, by using a coroutine for skeleton movement, triggers, etc.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkeletonAction : EnemyBaseActionClass {

	/******************************************** INITIALIZATION *******************************************/

	protected override void OnEnable() {
		EventManager.InitializeEnemies += SetReferences;
	}

	protected override void OnDisable() {
		EventManager.InitializeEnemies -= SetReferences;
	}
	

	/********************************************* SKELETON AI *********************************************/

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Skeleton");
		anim = characterSpriteObject.GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();
		groundCheck = characterSpriteObject.FindChild ("GroundCheck");

		player = GameObject.Find ("PlayerReferenceObject").transform;

		StartCoroutine ("CheckCharacterPhysics");
		StartCoroutine ("BasicEnemyControl");

		ItemBase skeletonBow = characterSpriteObject.FindChild ("Hands").FindChild ("HoldingHand").FindChild ("HoldingItem").GetChild(0).GetComponent <ItemBase> ();
		OnRefreshCurrentWeaponMoves (skeletonBow);
		skeletonBow.SetAttachedCharacterInput (this);
	}
	
	protected override void Attack() {
		AttackAction ("ShootBow");
	}

}
