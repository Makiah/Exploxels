using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicWeapon : ItemBase {
	//Properties of the weapon.  
	//In case the enemy should just attack immediately.  
	[SerializeField] private bool attackAfterAnimation = true;

	//The properties for the weapon.  
	[SerializeField] private float areaBounds;
	[SerializeField] private float distToEnemyOffset;

	[SerializeField] private float attackPower;
	[SerializeField] private Vector2 knockback;

	//Attacking method classes and enumerations.  
	[SerializeField]
	public enum PossibleAttackMethods
	{
		LeftMouseClick, 
		RightMouseClick
	}
	[SerializeField]
	public enum PossibleAttacks 
	{
		OverheadSlice, 
		Stab
	}

	[System.Serializable]
	public class AttackAndMove {
		public PossibleAttackMethods method;
		public PossibleAttacks attack;
	}

	//Unity has a weird thing that only certain ways that classes can be put together appears in the Inspector.  
	[SerializeField] private AttackAndMove[] attacks = null;

	//Just the default moves for an item, should be changed via a child script if these are not the attacks that you are looking for.  
	public override Dictionary <string, string> GetPossibleActionsForItem () {
		//Define the dictionary
		possibleMoves = new Dictionary<string, string> ();

		//Add default moves.  
		if (attacks.Length == 0) {
			possibleMoves.Add ("Stab", "MouseButtonDown0");
			possibleMoves.Add ("OverheadSlice", "MouseButtonDown1");
		} else {
			//Convert the array of enums into strings.  
			for (int i = 0; i < attacks.Length; i++) {
				possibleMoves.Add (ConvertAttackToString (attacks [i].attack), ConvertMethodToString (attacks [i].method));
				Debug.Log ("Added " + ConvertAttackToString (attacks [i].attack) + ", " + ConvertMethodToString (attacks [i].method));
			}
		}

		return possibleMoves;
	}

	string ConvertMethodToString (PossibleAttackMethods method) {
		switch (method) {
		case PossibleAttackMethods.LeftMouseClick: 
			return "MouseButtonDown0";
		case PossibleAttackMethods.RightMouseClick:
			return "MouseButtonDown1";
		default: 
			Debug.LogError("Invalid option specified");
			return "";
		}
	}

	string ConvertAttackToString (PossibleAttacks method) {
		switch (method) {
		case PossibleAttacks.OverheadSlice: 
			return "OverheadSlice";
		case PossibleAttacks.Stab:
			return "Stab";
		default: 
			Debug.LogError("Invalid option specified");
			return "";
		}
	}

	//Called by ItemBase.
	public override void InfluenceEnvironment(string actionKey) {
		if (attackAfterAnimation)
			AttemptToAttackAfterCompletedAnimation ();
		else
			AttackEnemyInFocus ();
	}

	void AttemptToAttackAfterCompletedAnimation () {
		attachedCharacterInput.ActionsAfterAnimation += AttackEnemyInFocus;
	}

	void AttackEnemyInFocus () {
		//Used to look for health panel manager.  ALWAYS REMEMBER TO KEEP THE PARAMETERS IN ORDER.   
		CharacterHealthPanelManager resultingHealthPanelManager = RaycastAttackUtilities.LookForEnemyViaLinecast (
			attachedCharacterInput.transform.position, 
			distToEnemyOffset, 
			0, 
			areaBounds, 
			attachedCharacterInput.GetFacingDirection(), 
			attachedCharacterInput.GetCombatantID()
		);

		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <CharacterBaseActionClass> ().ApplyKnockbackToCharacter (new Vector2 (knockback.x * attachedCharacterInput.GetFacingDirection (), knockback.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (attackPower);
		}
	}

}
