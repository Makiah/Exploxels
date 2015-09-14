
/*
 * Author: Makiah Bennett
 * Last edited: 14 September 2015
 * 
 * 9/14 - Properties assigned pre-game.  
 * 
 * This class is only used when an object is instantiated by chopping a tree, mining a rock, etc.  The script looks for this component, and uses the
 * item to create a new UIResourceReference.   
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class DroppedItemProperties : MonoBehaviour {

	public ResourceReference localResourceReference;

}
