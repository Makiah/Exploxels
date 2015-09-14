
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

	public override Dictionary <string, string> GetPossibleActionsForItem () {
		possibleMoves = new Dictionary<string, string> ();
		possibleMoves.Add ("ShootBow", "MouseButtonDown0");
		return possibleMoves;
	}
	
	public override void InfluenceEnvironment(string actionKey) {

	}

	public override void OnEnvironmentInfluenced(DropsItems itemInfluenced) {
		
	}
}
