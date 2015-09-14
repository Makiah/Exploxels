
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

	private float enemyWithinAreaBoundsFloat = .8f;
	private float distToEnemyFloatLength = 1.3f;

	public override Dictionary <string, string> GetPossibleActionsForItem () {
		possibleMoves = new Dictionary<string, string> ();
		possibleMoves.Add ("Stab", "MouseButtonDown0");
		possibleMoves.Add ("OverheadSlice", "MouseButtonDown1");
		return possibleMoves;
	}

	public override void InfluenceEnvironment(string actionKey) {
		AttemptToAttackAfterCompletedAnimation ();
	}
	
	public override void OnEnvironmentInfluenced(DropsItems itemInfluenced) {
		
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		attachedCharacterInput.ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		Vector3 enemyWithinAreaVectorBound = new Vector3 (enemyWithinAreaBoundsFloat, 0, 0);
		Vector3 distToEnemyVectorLength = new Vector3 (distToEnemyFloatLength, 0, 0);
		
		int playerFacingDirection = attachedCharacterInput.GetFacingDirection ();
		
		Vector3 startRaycastParameter = attachedCharacterInput.transform.position - enemyWithinAreaVectorBound;
		Vector3 endRaycastParameter = attachedCharacterInput.transform.position + enemyWithinAreaVectorBound;
		
		Vector3 actualStartRaycastParameter = startRaycastParameter + distToEnemyVectorLength * playerFacingDirection;
		Vector3 actualEndRaycastParameter = endRaycastParameter + distToEnemyVectorLength * playerFacingDirection;

		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer (lookForItemsOnLayer));

		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.red, 3f);

		Debug.Log ("Drew line with distance of " + Vector2.Distance (actualStartRaycastParameter, actualEndRaycastParameter));

		if (linecastResult.Length != 0) {
			if (linecastResult [0].collider != null) {
				Debug.Log ("Sword hit collider with name of " + linecastResult [0].collider.gameObject.name + ".");
				if (linecastResult [0].collider.gameObject.GetComponent <SusceptibleToDamage> () != null) {
					linecastResult [0].collider.gameObject.GetComponent <SusceptibleToDamage> ().YouHaveBeenAttacked ();
				} else {
					Debug.Log ("Sword could not attack collider with no SusceptibleToDamage script attached");
				}
			} else {
				Debug.Log ("Sword did not hit a collider.");
			}
		}
		
		attachedCharacterInput.ActionsAfterAnimation -= AttackEnemyInFocus;
	}

}
