
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script defines the properties that each enemy must have in order to be instantiated.  Note: This class is only used by the CreateLevelItems
 * script, it does not define the enemy globally.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

[System.Serializable]
public class InstantiatableObjectReference {
	public GameObject elementReference;
	public int probabilityOfInstantiation;
	public bool mustBeInstantiated;

}
