
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the hatchet, and defines the possible actions that the hatchet can undertake.  
 * 
 * When a hatchet is chopped, the ChopTreeInFocus() is added to the the ActionsAfterAnimation event in the PlayerAction class.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HatchetScript : ItemBase {

	float treeChoppingBounds = .2f;
	float distToTreeOffset = 1f;

	public override MovementAndMethod[] GetPossibleActionsForItem () {
		MovementAndMethod[] possibleMoves = new MovementAndMethod[1];
		possibleMoves [0] = new MovementAndMethod (MovementAndMethod.PossibleMovements.OverheadSlice, MovementAndMethod.PossibleTriggers.LeftMouseClick, false);
		return possibleMoves;
	}

	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		AttemptToChopATreeAfterCompletedAnimation ();
	}

	void AttemptToChopATreeAfterCompletedAnimation () {
		attachedCharacterInput.GetActualClass().ActionsOnAttack += ChopTreeInFocus;
	}

	void ChopTreeInFocus () {
		Vector3 treeChoppingVectorBound = new Vector3 (treeChoppingBounds, 0, 0);
		Vector3 distToTreeVectorOffset = new Vector3 (distToTreeOffset, 0, 0);
		
		int playerFacingDirection = attachedCharacterInput.GetActualClass().GetFacingDirection ();
		
		Vector3 startRaycastParameter = attachedCharacterInput.GetActualClass().transform.position - treeChoppingVectorBound;
		Vector3 endRaycastParameter = attachedCharacterInput.GetActualClass().transform.position + treeChoppingVectorBound;
		
		Vector3 actualStartRaycastParameter = startRaycastParameter + distToTreeVectorOffset * playerFacingDirection;
		Vector3 actualEndRaycastParameter = endRaycastParameter + distToTreeVectorOffset * playerFacingDirection;
		
		RaycastHit2D linecastResult = Physics2D.Linecast (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer ("Fighting"));

		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.black, 2, false);

		if (linecastResult.collider != null) {
			Debug.Log ("Hatchet hit collider with name of " + linecastResult.collider.gameObject.name + ".");
			if (linecastResult.collider.gameObject.GetComponent <TreeScript> () != null) {
				linecastResult.collider.gameObject.GetComponent <TreeScript> ().TreeChopped();
			}
		} else {
			Debug.Log("Hatchet did not hit a collider.");
		}
	}

}
