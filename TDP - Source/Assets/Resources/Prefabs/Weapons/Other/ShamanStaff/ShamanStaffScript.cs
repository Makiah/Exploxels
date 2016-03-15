
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

public class ShamanStaffScript : ItemBase {

	[SerializeField] private GameObject arrow = null;
	[SerializeField] private float attackPowerStrength = 0;

	public override MovementAndMethod[] GetPossibleActionsForItem () {
		MovementAndMethod[] possibleMoves = new MovementAndMethod[1];
		possibleMoves [0] = new MovementAndMethod (MovementAndMethod.PossibleMovements.Stab, MovementAndMethod.PossibleTriggers.LeftMouseClick, false);
		return possibleMoves;
	}

	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		AttemptToAttackAfterCompletedAnimation ();
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		attachedCharacterInput.GetActualClass().ActionsOnAttack += ShootArrow;
	}

	void ShootArrow() {
		//Get the player object.  
		GameObject playerObject = CurrentLevelVariableManagement.GetPlayerReference ();

		float preHeading = attachedCharacterInput.GetActualClass().GetFacingDirection () == 1 ? 0 : 180;

		//Apparently there is some issue with the bow's position when attached to the player object, because it is always (0, 0, 0).  This fixes it.  
		GameObject instantiatedArrow = (GameObject)(Instantiate (arrow, attachedCharacterInput.GetActualClass().gameObject.transform.position + new Vector3(1.2f, 0, 0) * attachedCharacterInput.GetActualClass().GetFacingDirection(), Quaternion.identity));

		ProjectileScript instantiatedArrowScript = instantiatedArrow.GetComponent <ProjectileScript> ();

		Vector3 positionToFireToward;
		float accuracy;

		if (!heldByPlayer) {
			positionToFireToward = playerObject.transform.position;
			accuracy = 0;
		} else {
			Vector3 shootDirection;
			shootDirection = Input.mousePosition;
			shootDirection.z = 0.0f;
			shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
			shootDirection = shootDirection-transform.position;
			positionToFireToward = shootDirection;
			accuracy = 0;
		}

		//Initialize the arrow.  
		instantiatedArrowScript.InitializeProjectileWithThresholdAndDeviation (positionToFireToward, 12, preHeading, 30, accuracy, attackPowerStrength);

	}

}
