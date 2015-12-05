
/*
 * Author: Makiah Bennett
 * Last edited: 14 September 2015
 * 
 * 9/14 - Added attacking function.  
 * 
 * This script controls the sword, and defines the possible actions that the sword can undertake.  
 * 
 * While long, this script currently has no real function.  
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordScript : ItemBase {

	private float enemyWithinAreaBounds = 1.2f;
	private float distToEnemyLength = 2f;

	public float swordPowerAttack;

	public override Dictionary <string, string> GetPossibleActionsForItem () {
		possibleMoves = new Dictionary<string, string> ();
		possibleMoves.Add ("Stab", "MouseButtonDown0");
		possibleMoves.Add ("OverheadSlice", "MouseButtonDown1");
		return possibleMoves;
	}

	public override void InfluenceEnvironment(string actionKey) {
		AttemptToAttackAfterCompletedAnimation ();
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		attachedCharacterInput.ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		CharacterHealthPanelManager resultingHealthPanelManager = RaycastAttackUtilities.LookForEnemyViaLinecast (attachedCharacterInput.transform.position, 
		                                                                                                          distToEnemyLength, 
		                                                                                                          enemyWithinAreaBounds, 
		                                                                                                          attachedCharacterInput.GetFacingDirection (), 
		                                                                                                          attachedCharacterInput.name == "Player");
		
		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.YouHaveBeenAttacked (swordPowerAttack);
		}

	}

}
