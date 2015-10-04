
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
	public float attackPowerStrength;

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

		GameObject playerObject = VariableManagement.GetPlayerReference ();

		float preHeading = attachedCharacterInput.GetFacingDirection () == 1 ? 0 : 180;

		//Apparently there is some issue with the bow's position when attached to the player object, because it is always (0, 0, 0).  This fixes it.  
		GameObject instantiatedArrow = (GameObject)(Instantiate (arrow, attachedCharacterInput.gameObject.transform.position + new Vector3(1.2f, 0, 0) * attachedCharacterInput.GetFacingDirection(), Quaternion.identity));

		ProjectileScript instantiatedArrowScript = instantiatedArrow.GetComponent <ProjectileScript> ();

		Vector3 positionToFireToward;
		float accuracy;

		if (attachedCharacterInput.characterName != "Player") {
			positionToFireToward = playerObject.transform.position;
			accuracy = 30;
		} else {
			Vector3 shootDirection;
			shootDirection = Input.mousePosition;
			shootDirection.z = 0.0f;
			shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
			shootDirection = shootDirection-transform.position;
			positionToFireToward = shootDirection;
			accuracy = 0;
		}

		instantiatedArrowScript.InitializeProjectileWithThresholdAndDeviation (positionToFireToward, 12, preHeading, 30, accuracy, attackPowerStrength);

	}

}
