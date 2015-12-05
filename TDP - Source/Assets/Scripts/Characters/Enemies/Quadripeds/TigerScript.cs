
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
		Debug.Log ("Tiger is attacking");
	}

	//Based off of SwordScript
	private float enemyWithinAreaBounds = 1.5f;
	private float distToEnemyLength = 1f;
	
	public float tigerAttackPower;

	void AttemptToAttackAfterCompletedAnimation () {
		ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		CharacterHealthPanelManager resultingHealthPanelManager = RaycastAttackUtilities.LookForEnemyViaLinecast (transform.position, 
		                                                                                                          distToEnemyLength, 
		                                                                                                          enemyWithinAreaBounds, 
		                                                                                                          GetFacingDirection (), 
		                                                                                                          false);
		
		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.YouHaveBeenAttacked (tigerAttackPower);
		}
		
	}
}
