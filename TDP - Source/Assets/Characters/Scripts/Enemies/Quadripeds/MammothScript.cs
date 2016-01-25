
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class MammothScript : EnemyBaseActionClass {

	protected override void InitializeEnemy() {}

	protected override void Attack() {
		AttemptToAttackAfterCompletedAnimation ();
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		//Use the RaycastAttackUtilites class.  
		CharacterHealthPanelManager resultingHealthPanelManager = LinecastingUtilities.FindEnemyViaLinecast (
			transform.position, 
			distToEnemyLength, 
			yOffsetToEnemy, 
			enemyWithinAreaBounds, 
			GetFacingDirection(), 
			GetCombatantID()
		);

		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <CharacterBaseActionClass> ().ApplyKnockbackToCharacter (new Vector2 (enemyKnockbackPower.x * GetFacingDirection (), enemyKnockbackPower.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (enemyAttackingPower);
		}

	}
	
}
