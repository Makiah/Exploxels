
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

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Mammoth");
		base.SetReferences ();
	}

	protected override void Attack() {
		AttemptToAttackAfterCompletedAnimation ();
		Debug.Log ("Mammoth is attacking");
	}
	
	//Based off of SwordScript
	private float enemyWithinAreaBounds = 1.6f;
	private float yOffsetToEnemy = -1f;
	private float distToEnemyLength = 3.2f;
	
	public float mammothAttackPower;
	
	void AttemptToAttackAfterCompletedAnimation () {
		ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		//Use the RaycastAttackUtilites class.  
		CharacterHealthPanelManager resultingHealthPanelManager = RaycastAttackUtilities.LookForEnemyViaLinecast (transform.position, 
		                                                                                                          distToEnemyLength, 
		                                                                                                          yOffsetToEnemy, 
		                                                                                                          enemyWithinAreaBounds, 
																												  GetFacingDirection());
		
		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.YouHaveBeenAttacked (mammothAttackPower);
		}
		
	}
	
}
