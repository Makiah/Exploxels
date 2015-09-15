
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the bow, and defines the possible actions that the bow can undertake.  It currently has no real functionality.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BowScript : ItemBase {

	public GameObject arrow;

	public override Dictionary <string, string> GetPossibleActionsForItem () {
		possibleMoves = new Dictionary<string, string> ();
		possibleMoves.Add ("ShootBow", "MouseButtonDown0");
		return possibleMoves;
	}

	public override void InfluenceEnvironment(string actionKey) {
		AttemptToAttackAfterCompletedAnimation ();
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		attachedCharacterInput.ActionsAfterAnimation += ShootArrow;
	}

	void ShootArrow() {

		float preHeading = attachedCharacterInput.GetFacingDirection() == 1 ? 0 : 180;

		GameObject instantiatedArrow = (GameObject) (Instantiate (arrow, transform.position, Quaternion.identity));
		if (attachedCharacterInput.GetFacingDirection () == 1)
			instantiatedArrow.GetComponent <ProjectileScript> ().SetProjectileParametersWithAutomaticThresholdAndDeviation (8, preHeading, 30, 20);
		else 
			instantiatedArrow.GetComponent <ProjectileScript> ().SetProjectileParametersWithAutomaticThresholdAndDeviation (8, preHeading, 30, 20);
	}

}
