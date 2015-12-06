
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
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
	
	/********************************************* ZOMBIE AI *********************************************/

	//Only for the custom properties of the zombie.  
	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Zombie");

		ItemBase zombieSword = characterSpriteObject.FindChild ("Hands").FindChild ("HoldingHand").FindChild ("HoldingItem").GetChild(0).GetComponent <ItemBase> ();
		OnRefreshCurrentWeaponMoves (zombieSword);
		zombieSword.SetAttachedCharacterInput (this);
		zombieSword.ChangeLayerToCheckForItemsOn ("Player");

		ignorePlayerMovementThreshold = Random.Range (ignorePlayerMovementThreshold - 1, ignorePlayerMovementThreshold + 2);

		//Refers to the EnemyBaseActionClass SetReferences, not the CharacterBaseActionClass SetReferences.  
		base.SetReferences ();
	}

	protected override void Attack() {
		AttackAction ("Stab");
	}

}
