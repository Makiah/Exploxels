
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

	protected HumanoidBaseActionClass attachedCharacterInput;

	protected string lookForItemsOnLayer = "Enemies";

	public void SetAttachedCharacterInput(HumanoidBaseActionClass ctorCharacterInput) {
		attachedCharacterInput = ctorCharacterInput;
	}

	protected Dictionary <string, string> possibleMoves;

	public abstract Dictionary <string, string> GetPossibleActionsForItem ();
	public abstract void InfluenceEnvironment(string actionKey);
	//Just as a side note, enemies and environment items (rocks and trees) share the same enemy base class.  
	public abstract void OnEnvironmentInfluenced(DropsItems influencedItem);

	public void ChangeLayerToCheckForItemsOn(string newLayer) {
		lookForItemsOnLayer = newLayer;
	}

}
