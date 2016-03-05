using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIFireScript : ItemBase {

	[SerializeField] private GameObject physicalFireObject = null;

	//Define the moves for the current weapon.  
	public override MovementAndMethod[] GetPossibleActionsForItem () {
		MovementAndMethod[] possibleMoves = new MovementAndMethod[1];
		possibleMoves [0] = new MovementAndMethod (MovementAndMethod.PossibleMovements.CreatePhysicalItem, MovementAndMethod.PossibleTriggers.LeftMouseClick, false);
		return possibleMoves;
	}

	//Called whenever the specified action (by the dictionary) occurs.  
	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		if (physicalFireObject != null) {
			//Create the object.  
			Vector3 physicalFireOffset = new Vector3(1, 0, 0) * CurrentLevelVariableManagement.GetPlayerReference().GetComponent <PlayerAction> ().GetFacingDirection();
			GameObject createdFireObject = (GameObject) (Instantiate (physicalFireObject, CurrentLevelVariableManagement.GetPlayerReference().transform.position + physicalFireOffset + physicalFireObject.transform.localPosition, Quaternion.identity));
			createdFireObject.GetComponent <PhysicalFireScript> ().OnFireCreated();
			//Used to remove the current item from the hotbar.  
			ChangeStackOfCurrentHotbarItem(1);
		} else {
			Debug.LogError("Physical Fire Object does not exist!");
		}
	}

}
