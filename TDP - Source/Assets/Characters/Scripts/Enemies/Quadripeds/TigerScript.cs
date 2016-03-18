
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
	protected override void InitializeEnemy() {}
	
	protected override void Attack() {
		AttemptToAttackAfterCompletedAnimation ();
	}

	void AttemptToAttackAfterCompletedAnimation () {
		ActionsOnAttack += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		//Used for finding the health panel manager via linecast.  
		CharacterHealthPanelManager resultingHealthPanelManager = LinecastingUtilities.FindEnemyViaLinecast (
			transform.position, 
			distToEnemyLength, 
			0, 
			enemyWithinAreaBounds, 
			GetFacingDirection (), 
			GetCombatantID()
		);
		
		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <CharacterBaseActionClass> ().ApplyKnockback (new Vector2 (enemyKnockbackPower.x * GetFacingDirection (), enemyKnockbackPower.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (enemyAttackingPower);
		}
		
	}
}
