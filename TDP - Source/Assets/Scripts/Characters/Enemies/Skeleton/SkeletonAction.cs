
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
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

	/********************************************* SKELETON AI *********************************************/

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Skeleton");

		ItemBase skeletonBow = characterSpriteObject.FindChild ("Hands").FindChild ("HoldingHand").FindChild ("HoldingItem").GetChild(0).GetComponent <ItemBase> ();
		OnRefreshCurrentWeaponMoves (skeletonBow);
		skeletonBow.SetAttachedCharacterInput (this);

		ignorePlayerMovementThreshold = Random.Range (ignorePlayerMovementThreshold - 2, ignorePlayerMovementThreshold + 3);

		base.SetReferences ();
	}
	
	protected override void Attack() {
		AttackAction ("ShootBow");
	}

}
