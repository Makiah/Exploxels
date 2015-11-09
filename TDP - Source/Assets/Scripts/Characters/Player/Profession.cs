
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

	//Define required components for a gender.  
	public class Gender {
		public Sprite head, body, legs, arm;
	}

	//Components that do not depend on gender.
	public Sprite icon;
	public Gender male;
	public Gender female;
	public int currentGender;
	public string name;
	public int professionID;
	public UISlotContentReference[] initialObjects;

	//Profession constructor
	public Profession(string resourcesPath, string ctorName, int ctorProfessionID, UISlotContentReference[] ctorInitialObjects) {

		//Define the male and female genders.  
		male = new Gender ();
		female = new Gender ();

		//Load sprite resources from the Resources folder.  
		icon = Resources.Load <Sprite> (resourcesPath + "Icon");

		//Male sprites
		male.head = Resources.Load <Sprite> (resourcesPath + "Male/" + "Head");
		male.body = Resources.Load <Sprite> (resourcesPath + "Male/" + "Body");
		male.legs = Resources.Load <Sprite> (resourcesPath + "Male/" + "Legs");
		male.arm = Resources.Load <Sprite> (resourcesPath + "Male/" + "Arm");
		//Female sprites
		female.head = Resources.Load <Sprite> (resourcesPath + "Female/" + "Head");
		female.body = Resources.Load <Sprite> (resourcesPath + "Female/" + "Body");
		female.legs = Resources.Load <Sprite> (resourcesPath + "Female/" + "Legs");
		female.arm = Resources.Load <Sprite> (resourcesPath + "Female/" + "Arm");

		//Define initial gender.  
		currentGender = 0;

		//ID requirements
		professionID = ctorProfessionID;
		name = ctorName;

		//Put each initial item in the hotbar.  
		if (ctorInitialObjects != null) {
			initialObjects = new UISlotContentReference[ctorInitialObjects.Length];
			initialObjects = ctorInitialObjects;
		}
	}

	//Define gender.  
	public void SetGender(int ctorGender) {
		currentGender = ctorGender;
	}

}
