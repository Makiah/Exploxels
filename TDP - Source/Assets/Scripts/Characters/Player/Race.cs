
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

public class Race {

	public Sprite[] heads;
	public Sprite body;
	public Sprite legs;
	public Sprite arm;
	public string name;
	public int raceID;
	public ResourceReference[] initialObjects;
	public int headVariationIndex;

	public Race(string resourcesPath, string[] ctorHeads, string ctorName, int ctorRaceID, ResourceReference[] ctorInitialObjects) {
		heads = new Sprite[ctorHeads.Length];
		for (int i = 0; i < ctorHeads.Length; i++) {
			heads[i] = Resources.Load <Sprite> (resourcesPath + ctorHeads[i]);
		}

		body = Resources.Load <Sprite> (resourcesPath + ctorName + "Body");
		legs = Resources.Load <Sprite> (resourcesPath + ctorName + "Legs");
		arm = Resources.Load <Sprite> (resourcesPath + ctorName + "Arm");
		name = ctorName;
		raceID = ctorRaceID;

		if (ctorInitialObjects != null) {
			initialObjects = new ResourceReference[ctorInitialObjects.Length];
			initialObjects = ctorInitialObjects;
		}
	}

	public void SetHeadVariation(int ctorHeadVariation) {
		headVariationIndex = ctorHeadVariation;
	}

}
