
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class TigerScript : EnemyBaseActionClass {

	//Initialization stuff.  
	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Tiger");
		base.SetReferences ();
	}
	
	protected override void Attack() {
		AttemptToAttackAfterCompletedAnimation ();
	}

	void AttemptToAttackAfterCompletedAnimation () {
		ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		//Used for finding the health panel manager via linecast.  
		CharacterHealthPanelManager resultingHealthPanelManager = RaycastAttackUtilities.FindComponentViaLinecast <CharacterHealthPanelManager> (
			transform.position, 
			distToEnemyLength, 
			0, 
			enemyWithinAreaBounds, 
			GetFacingDirection (), 
			GetCombatantID()
		);
		
		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <CharacterBaseActionClass> ().ApplyKnockbackToCharacter (new Vector2 (enemyKnockbackPower.x * GetFacingDirection (), enemyKnockbackPower.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (enemyAttackingPower);
		}
		
	}
}
