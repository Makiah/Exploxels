
/*
 * Author: Makiah Bennett
 * Last edited: 23 November 2015
 * 
 * CreateLevelItems manages the enemies of the game, and instantiates them at pre-set points in each variation.  
 * This is done through the EnemyReferenceClass probability indicators, which are defined in this class.  
 * 
 * Completely reworked the whole way the script works.  You can now set items to "must be instantiated" and they will be instantiated at one random point in the level.  
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateLevelItems : MonoBehaviour {

	//Initialization
	void OnEnable() {
		LevelEventManager.CreateTerrainItems += CreateTerrainItems;
		LevelEventManager.CreatePlayer += InstantiatePlayer;
	}
	
	void OnDisable() {
		LevelEventManager.CreateTerrainItems -= CreateTerrainItems;
		LevelEventManager.CreatePlayer -= InstantiatePlayer;
	}

	/****************** PLAYER ********************/

	//Variables that will be used later.  
	[SerializeField] private GameObject playerToInstantiate = null;
	[SerializeField] private Vector3 pointToInstantiatePlayerAt = Vector3.zero;

	//Instantiate the player
	void InstantiatePlayer() {
		Instantiate (playerToInstantiate, pointToInstantiatePlayerAt, Quaternion.identity);
		Debug.Log ("Instantiated Player");
	}

	/****************** TERRAIN ********************/
	
	[SerializeField] protected int probabilityToInstantiateNullElement;
	[SerializeField] protected InstantiatableObjectReference[] initialGameElements;

	//Will be used for ChooseElement()
	InstantiatableObjectReference[] requiredItems;
	InstantiatableObjectReference[] optionalItems;

	//Parse through all enemy item points.  
	void CreateTerrainItems (TerrainReferenceClass mazeSegments) {

		//This list will hold the positions of all of the enemy item positions.  
		List <Transform> allEnemyItemPoints = new List<Transform> ();

		//This prevents a ton of future errors and if-checking.  
		if (initialGameElements.Length != 0) {
			//Instantiate all items
			for (int i = 0; i < mazeSegments.layer1.Length; i++) {
				//Preventing future errors.  
				if (mazeSegments.layer1[i] != null) {
					if (mazeSegments.layer1[i].FindChild("Points") != null) {
						Transform enemyItemsTransform = mazeSegments.layer1[i].FindChild ("Points").FindChild ("EnemyItems");
						//Check to make sure enemy item points exist.  
						if (enemyItemsTransform != null) {
							//Find the points
							Transform[] enemyItemPoints = ScriptingUtilities.ParseChildrenFromTransform (enemyItemsTransform);
							for (int j = 0; j < enemyItemPoints.Length; j++) {
								//Add all transforms to the master list.  
								allEnemyItemPoints.Add(enemyItemPoints[j]);
							}
						}
					} else {
						Debug.Log("Did not find points upon which to instantiate enemies on increment " + i + " with variation name " + mazeSegments.layer1[i].gameObject.name);
					}
				} else {
					Debug.LogError("MazeSegments.layer1[" + i + "] is null!!!!");
				}
			}

		} else {
			Debug.LogError("No initial game elements are set!");
		}

		//Make sure that the previous step added transforms to the list.  
		if (allEnemyItemPoints.Count > 0) {
			//Sort the object references
			SortObjectArray ();
			//Make sure that the variations have the capacity to fill all required items.  
			if (requiredItems.Length <= allEnemyItemPoints.Count) {
				//List of empty points	
				List <int> emptyPoints = new List <int> ();
				//Populate the list of points.  
				for (int j = 0; j < allEnemyItemPoints.Count; j++) {
					emptyPoints.Add (j);
				}
			
				//All required items.  
				for (int j = 0; j < requiredItems.Length; j++) {
				
					int chosenLoc = Random.Range (0, emptyPoints.Count);
					//Choose a point out of the existing points.  
					int pointToInstantiateRequiredItem = emptyPoints [chosenLoc];
					//Instantiate the object
					GameObject createdElement = InstantiateElementOnPoint(requiredItems [j].elementReference, 
					                                                      allEnemyItemPoints [pointToInstantiateRequiredItem].position
					                                                      );
					createdElement.transform.SetParent (allEnemyItemPoints [pointToInstantiateRequiredItem]);
				
					//Remove the point that was just used from the list.  
					emptyPoints.RemoveAt (chosenLoc);
				}
			
				//For the rest of the points.  
				while (emptyPoints.Count > 0) {
					int chosenLoc = Random.Range (0, emptyPoints.Count);
					int pointToInstantiateRequiredItem = emptyPoints [chosenLoc];
					//Instantiate the object
					InstantiatableObjectReference chosenElement = ChooseElement ();
					//Check to make sure that it is not supposed to instantiate anything.  
					if (chosenElement != null) {
						//Instantiate element
						GameObject createdElement = InstantiateElementOnPoint (chosenElement.elementReference, 
					                                                           allEnemyItemPoints [pointToInstantiateRequiredItem].position
						                                                       );
						createdElement.transform.SetParent (allEnemyItemPoints [pointToInstantiateRequiredItem]);
					}
					//Remove the point that was just used from the list.  
					emptyPoints.RemoveAt (chosenLoc);
				}
			}
		}

	}

	//Sort the InstantiatableObjectReference array into required and optional instantiations.  
	void SortObjectArray() {
		List <InstantiatableObjectReference> requiredItemsList = new List<InstantiatableObjectReference>();
		List <InstantiatableObjectReference> optionalItemsList = new List<InstantiatableObjectReference>();

		//Loop through and populate the lists
		for (int i = 0; i < initialGameElements.Length; i++) {
			if (initialGameElements[i].mustBeInstantiated) {
				requiredItemsList.Add(initialGameElements[i]);
			} else {
				optionalItemsList.Add(initialGameElements[i]);
			}
		}

		//Add the lists to the arrays.  
		requiredItems = requiredItemsList.ToArray ();
		optionalItems = optionalItemsList.ToArray ();

	}

	//Choose an element from the required or optional array.  
	InstantiatableObjectReference ChooseElement() {
		//Make sure that the array exists, and also make sure that all must-be instantiated items have not all already been
		if (Random.Range (0, probabilityToInstantiateNullElement) == 0) {
			//This will end after the function returns.  
			while (true) {
				//Choose an item
				InstantiatableObjectReference chosenItem = optionalItems [Random.Range (0, optionalItems.Length)];
				if (Random.Range (0, chosenItem.probabilityOfInstantiation) == 0) {
					return chosenItem;
				}
			}
		} else {
			return null;
		}
	}

	//Used for instantiating elements with a z position of 0.  
	GameObject InstantiateElementOnPoint(GameObject element, Vector3 position) {
		GameObject createdElement = (GameObject)(Instantiate (element, position, Quaternion.identity));
		createdElement.transform.position = new Vector3 (element.transform.localPosition.x + position.x, element.transform.localPosition.y + position.y, element.transform.localPosition.z);
		return createdElement;
	}

}
