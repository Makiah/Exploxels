
/*
 * Author: Makiah Bennett
 * Last edited: 12 September 2015
 * 
 * This script works similarly to an interface, yet has a few properties that make it useful as an abstract class.  This class is used solely as a base 
 * for game tools, such as pickaxes, hatchets, etc.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//As of right now, this class could work just as well as an interface, and probably simplify a few things.  However, if additional functionality is 
//later added, this class makes more sense.  
public abstract class ItemBase : MonoBehaviour {

	//Used for base classes.  
	protected bool heldByPlayer = false;

	//Attack and move works by creating a serializable system for defining MovementAndMethod[].  
	[System.Serializable]
	public class AttackAndMove {
		public MovementAndMethod.PossibleTriggers method;
		public MovementAndMethod.PossibleMovements attack;
		public bool worksMidair;
	}

	//Unity has a weird issue that only certain ways that classes can be put together appears in the Inspector.  
	[SerializeField] protected AttackAndMove[] movementTriggerPair = null;

	//The class methods.  
	protected ICanHoldItems attachedCharacterInput;

	public virtual void SetAttachedCharacterInput(ICanHoldItems ctorCharacterInput) {
		attachedCharacterInput = ctorCharacterInput;
		//Similar to Java's instanceof operator.  
		Debug.Log(attachedCharacterInput.GetActualClass().gameObject.name + " is " + (attachedCharacterInput.GetActualClass() is PlayerAction ? "" : "not") + " the player");
		heldByPlayer = attachedCharacterInput.GetActualClass () is PlayerAction;
	}

	//Called by CharacterBaseActionClass when a new item is being used.  
	public abstract MovementAndMethod[] GetPossibleActionsForItem ();
	public abstract void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey);

	//Just added.  
	protected void ChangeStackOfCurrentHotbarItem(int stackToChangeBy) {
		CurrentLevelVariableManagement.GetLevelUIReference ().transform.FindChild ("Hotbar").GetComponent <HotbarManager> ().ModifyStackOfSelectedItem (1);
	}

}
