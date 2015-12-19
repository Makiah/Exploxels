
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the pickaxe, and defines the possible actions that the pickaxe can undertake.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickaxeScript : ItemBase {

	float orePickaxingBounds = .2f;
	float distToOreBounds = 1f;

	public override MovementAndMethod[] GetPossibleActionsForItem () {
		MovementAndMethod[] possibleMoves = new MovementAndMethod[1];
		possibleMoves [0] = new MovementAndMethod (MovementAndMethod.PossibleMovements.OverheadSlice, MovementAndMethod.PossibleTriggers.LeftMouseClick, false);
		return possibleMoves;
	}
	
	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		AttemptToChopATreeAfterCompletedAnimation ();
	}
	
	void AttemptToChopATreeAfterCompletedAnimation () {
		attachedCharacterInput.GetActualClass().ActionsAfterAnimation += ChopTreeInFocus;
	}
	
	void ChopTreeInFocus () {
		Vector3 treeChoppingVectorBound = new Vector3 (orePickaxingBounds, 0, 0);
		Vector3 distToTreeVectorOffset = new Vector3 (distToOreBounds, 0, 0);
		
		int playerFacingDirection = attachedCharacterInput.GetActualClass().GetFacingDirection ();
		
		Vector3 startRaycastParameter = attachedCharacterInput.GetActualClass().transform.position - treeChoppingVectorBound;
		Vector3 endRaycastParameter = attachedCharacterInput.GetActualClass().transform.position + treeChoppingVectorBound;
		
		Vector3 actualStartRaycastParameter = startRaycastParameter + distToTreeVectorOffset * playerFacingDirection;
		Vector3 actualEndRaycastParameter = endRaycastParameter + distToTreeVectorOffset * playerFacingDirection;
		
		RaycastHit2D linecastResult = Physics2D.Linecast (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer ("Enemies"));
		
		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.black, 2, false);

		OreScript resultingOreScript = LinecastingUtilities.FindEnemyViaLinecast <OreScript> (
			attachedCharacterInput.GetActualClass ().transform.position,
			distToOreBounds, 
			0, 
			orePickaxingBounds, 
			attachedCharacterInput.GetActualClass ().GetFacingDirection ()
		);

		if (resultingOreScript != null) {
			resultingOreScript.OnOreChipped();
		} else {
			Debug.Log("No ore found");
		}
		
		attachedCharacterInput.GetActualClass().ActionsAfterAnimation -= ChopTreeInFocus;
	}

}
