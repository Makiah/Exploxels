
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
		AttackEnemyInFocus ();
		//AttemptToAttackAfterCompletedAnimation ();
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		attachedCharacterInput.ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		Vector3 enemyWithinAreaVectorBound = new Vector3 (enemyWithinAreaBounds, 0, 0);
		Vector3 distToEnemyVectorLength = new Vector3 (distToEnemyLength, 0, 0);
		
		int playerFacingDirection = attachedCharacterInput.GetFacingDirection ();
		
		Vector3 startRaycastParameter = attachedCharacterInput.transform.position - enemyWithinAreaVectorBound;
		Vector3 endRaycastParameter = attachedCharacterInput.transform.position + enemyWithinAreaVectorBound;
		
		Vector3 actualStartRaycastParameter = startRaycastParameter + distToEnemyVectorLength * playerFacingDirection;
		Vector3 actualEndRaycastParameter = endRaycastParameter + distToEnemyVectorLength * playerFacingDirection;

		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer (lookForItemsOnLayer));

		if (lookForItemsOnLayer == "Player") {
			Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.red, 3f);
		} else {
			Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.green, 3f);
		}

		if (linecastResult.Length != 0) {
			if (linecastResult [0].collider != null) {
				if (linecastResult [0].collider.gameObject.GetComponent <CharacterHealthController> () != null) {
					linecastResult [0].collider.gameObject.GetComponent <CharacterHealthController> ().YouHaveBeenAttacked (swordPowerAttack);
				}
			}
		}
	}

}
