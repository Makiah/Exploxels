
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
		OreScript resultingOreScript = LinecastingUtilities.FindComponentViaLinecast <OreScript> (
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
	}

}
