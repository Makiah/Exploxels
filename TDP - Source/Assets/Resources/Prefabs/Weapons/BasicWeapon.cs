using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicWeapon : ItemBase {
	//Properties of the weapon.  
	//In case the enemy should just attack immediately.  
	[SerializeField] private bool attackAfterAnimation = true;

	//The properties for the weapon.  
	[SerializeField] private float areaBounds = 0;
	[SerializeField] private float distToEnemyOffset = 0;

	[SerializeField] private float attackPower = 0;
	[SerializeField] private Vector2 knockback = Vector2.zero;

	//Just the default moves for an item, should be changed via a child script if these are not the attacks that you are looking for.  
	public override MovementAndMethod[] GetPossibleActionsForItem () {
		MovementAndMethod[] possibleMoves;
		//Add default moves, if it is null otherwise.  
		if (movementTriggerPair == null || movementTriggerPair.Length == 0) {
			possibleMoves = new MovementAndMethod[2];
			possibleMoves [0] = new MovementAndMethod (MovementAndMethod.PossibleMovements.Stab, MovementAndMethod.PossibleTriggers.LeftMouseClick, false);
			possibleMoves [1] = new MovementAndMethod (MovementAndMethod.PossibleMovements.OverheadSlice, MovementAndMethod.PossibleTriggers.RightMouseClick, false);
		} else {
			//Convert the array of enums MovementAndMethod classes.  The constructor converts everything.  
			possibleMoves = new MovementAndMethod[movementTriggerPair.Length];
			//Define all of the movement and trigger pair values based off of the serializable list.  
			for (int i = 0; i < movementTriggerPair.Length; i++) {
				possibleMoves [i] = new MovementAndMethod (movementTriggerPair [i].attack, movementTriggerPair [i].method, movementTriggerPair [i].worksMidair);
			}
		}

		return possibleMoves;
	}

	//Called by ItemBase.
	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		//Ground Pound.  
		if (actionKey == MovementAndMethod.PossibleMovements.GroundPound) {
			attachedCharacterInput.ExternalJumpAction (4);
			if (attackAfterAnimation)
				attachedCharacterInput.GetActualClass ().ActionsOnAttack += GroundPoundEnemy;
			else
				GroundPoundEnemy ();
		} else if (actionKey == MovementAndMethod.PossibleMovements.AirSlash) {
			if (attackAfterAnimation)
				attachedCharacterInput.GetActualClass ().ActionsOnAttack += AirSlashEnemy;
			else
				AirSlashEnemy ();
		} else {
			if (attackAfterAnimation)
				attachedCharacterInput.GetActualClass ().ActionsOnAttack += AttackEnemyInFocus;
			else
				AttackEnemyInFocus ();
		}
	}

	void GroundPoundEnemy () {
		//Used to look for health panel manager.  ALWAYS REMEMBER TO KEEP THE PARAMETERS IN ORDER.   
		CharacterHealthPanelManager resultingHealthPanelManager = LinecastingUtilities.BasicLinecast (
			attachedCharacterInput.GetActualClass().transform.position, 
			new Vector2 (attachedCharacterInput.GetActualClass().transform.position.x, attachedCharacterInput.GetActualClass().transform.position.y - 3f), 
			attachedCharacterInput.GetCombatantID()
		);

		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <ICombatant> ().GetActualClass().ApplyKnockback (new Vector2 (knockback.x * attachedCharacterInput.GetActualClass().GetFacingDirection (), knockback.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (attackPower);
		}
	}

	void AirSlashEnemy() {
		//Used to look for health panel manager.  ALWAYS REMEMBER TO KEEP THE PARAMETERS IN ORDER.   
		CharacterHealthPanelManager resultingHealthPanelManager = LinecastingUtilities.BasicLinecast (
			attachedCharacterInput.GetActualClass().transform.position, 
			new Vector2 (attachedCharacterInput.GetActualClass().transform.position.x, attachedCharacterInput.GetActualClass().transform.position.y + 3f), 
			attachedCharacterInput.GetCombatantID()
		);

		if (resultingHealthPanelManager != null) {
			//Has to be GetComponent <ICombatant> ().GetActualClass() instead of GetComponent <CharacterBaseActionClass> () because otherwise the soldier gets the NPC script intead. 
			resultingHealthPanelManager.gameObject.GetComponent <ICombatant> ().GetActualClass().ApplyKnockback (new Vector2 (knockback.x * attachedCharacterInput.GetActualClass().GetFacingDirection (), knockback.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (attackPower);
		}
	}

	void AttackEnemyInFocus () {
		//Used to look for health panel manager.  ALWAYS REMEMBER TO KEEP THE PARAMETERS IN ORDER.   
		Debug.Log("Ignoring " + attachedCharacterInput.GetCombatantID());
		CharacterHealthPanelManager resultingHealthPanelManager = LinecastingUtilities.FindEnemyViaLinecast (
			attachedCharacterInput.GetActualClass().transform.position, 
			distToEnemyOffset, 
			0, 
			areaBounds, 
			attachedCharacterInput.GetActualClass().GetFacingDirection(), 
			attachedCharacterInput.GetCombatantID()
		);

		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <ICombatant> ().GetActualClass().ApplyKnockback (new Vector2 (knockback.x * attachedCharacterInput.GetActualClass().GetFacingDirection (), knockback.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (attackPower);
		}
	}

}
