
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

	public override Dictionary <string, string> GetPossibleActionsForItem () {
		possibleMoves = new Dictionary<string, string> ();
		possibleMoves.Add ("OverheadSlice", "MouseButtonDown0");
		return possibleMoves;
	}
	
	public override void InfluenceEnvironment(string actionKey) {

	}

	public override void OnEnvironmentInfluenced(DropsItems itemInfluenced) {
		
	}
}
