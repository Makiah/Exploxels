
/*
 * Author: Makiah Bennett
 * Created 13 September 2015
 * Last edited: 13 September 2015
 * 
 * 9/13 - Added EnemyActionBaseClass as base, created new SetReferences, gave sword.  
 * 
 * This script controls the Zombie, by using a coroutine for skeleton movement, triggers, etc.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieAction : EnemyBaseActionClass {

	/******************************************** INITIALIZATION *******************************************/

	protected override void OnEnable() {
		EventManager.InitializeEnemies += SetReferences;
	}

	protected override void OnDisable() {
		EventManager.InitializeEnemies -= SetReferences;
	}
	

	/********************************************* SKELETON AI *********************************************/

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Zombie");
		anim = characterSpriteObject.GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();
		groundCheck = characterSpriteObject.FindChild ("GroundCheck");

		player = GameObject.Find ("ManagementFrameworks").transform.FindChild ("GameVariables").gameObject.GetComponent <VariableManagement> ().GetPlayerReference ().transform;

		ItemBase zombieSword = characterSpriteObject.FindChild ("Hands").FindChild ("HoldingHand").FindChild ("HoldingItem").GetChild(0).GetComponent <ItemBase> ();
		OnRefreshCurrentWeaponMoves (zombieSword);
		zombieSword.SetAttachedCharacterInput (this);
		zombieSword.ChangeLayerToCheckForItemsOn ("Player");

		remainDistanceFromPlayer = Random.Range (remainDistanceFromPlayer - 1, remainDistanceFromPlayer + 2);

		StartCoroutine ("CheckCharacterPhysics");
		StartCoroutine ("BasicEnemyControl");
	}

	protected override void Attack() {
		AttackAction ("Stab");
	}
	

}
