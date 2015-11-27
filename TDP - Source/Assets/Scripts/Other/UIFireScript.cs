using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIFireScript : ItemBase {

	[SerializeField] private GameObject physicalFireObject = null;

	//Define the moves for the current weapon.  
	public override Dictionary <string, string> GetPossibleActionsForItem () {
		possibleMoves = new Dictionary<string, string> ();
		possibleMoves.Add ("CreatePhysicalItem", "MouseButtonDown0");
		return possibleMoves;
	}

	//Called whenever the specified action (by the dictionary) occurs.  
	public override void InfluenceEnvironment(string actionKey) {
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
