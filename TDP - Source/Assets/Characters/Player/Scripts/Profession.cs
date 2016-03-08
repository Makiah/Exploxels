
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script manages the properties of each race that will be used in the game (male character, female character, wizard, etc.  
 * Resources are loaded based on the path that is given for each element, and then placed into the game.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class Profession {
	//Components that do not depend on gender.
	public Sprite icon;
	public readonly Sprite maleHead, femaleHead, body, arm, leg;
	public string name;
	public int professionID;
	public ResourceReferenceWithStack[] initialObjects;

	//Profession constructor
	public Profession(string resourcesPath, string ctorName, int ctorProfessionID, ResourceReferenceWithStack[] ctorInitialObjects) {
		//Load sprite resources from the Resources folder.  
		icon = Resources.Load <Sprite> (resourcesPath + "Icon");

		//Sprites
		maleHead = Resources.Load <Sprite> (resourcesPath + "Male Head");
		femaleHead = Resources.Load <Sprite> (resourcesPath + "Female Head");
		body = Resources.Load <Sprite> (resourcesPath + "Body");
		leg = Resources.Load <Sprite> (resourcesPath + "Leg");
		arm = Resources.Load <Sprite> (resourcesPath + "Arm");

		//ID requirements
		professionID = ctorProfessionID;
		name = ctorName;

		//Put each initial item in the hotbar.  
		if (ctorInitialObjects != null) {
			initialObjects = ctorInitialObjects;
		}
	}

}
