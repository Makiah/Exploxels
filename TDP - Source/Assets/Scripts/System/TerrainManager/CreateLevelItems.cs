
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
 * 
 * CreateLevelItems manages the enemies of the game, and instantiates them at pre-set points in each variation.  
 * This is done through the EnemyReferenceClass probability indicators, which are defined in this class.  
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


	//Variables that will be used later.  
	[SerializeField] private GameObject playerToInstantiate;
	[SerializeField] private Vector3 pointToInstantiatePlayerAt;

	[SerializeField] protected int probabilityToInstantiateNullElement;
	[SerializeField] protected InstantiatableObjectReference[] initialGameElements;

	//Instantiate the player
	void InstantiatePlayer() {
		Instantiate (playerToInstantiate, pointToInstantiatePlayerAt, Quaternion.identity);
		Debug.Log ("Instantiated Player");
	}

	//Parse through all enemy item points.  
	void CreateTerrainItems (TerrainReferenceClass mazeSegments) {
		//This prevents a ton of future errors and if-checking.  
		if (initialGameElements.Length != 0) {

			//Sort the initial things
			SortObjectArray();

			//Instantiate all items
			for (int i = 0; i < mazeSegments.layer1.Length; i++) {
	
				if (mazeSegments.layer1[i].FindChild("Points") != null) {
	
					Transform enemyItemsTransform = mazeSegments.layer1[i].FindChild ("Points").FindChild ("EnemyItems");
	
					if (enemyItemsTransform.childCount != 0) {
							
						Transform[] enemyItemPoints = ScriptingUtilities.ParseChildrenFromTransform (enemyItemsTransform);
							
						foreach (Transform enemyItemPoint in enemyItemPoints) {
	
							InstantiatableObjectReference chosenElement = ChooseElement();
	
							if (chosenElement != null) {
								GameObject createdElement = (GameObject)(Instantiate (chosenElement.elementReference, enemyItemPoint.position + chosenElement.elementReference.transform.localPosition, Quaternion.identity));
								createdElement.transform.SetParent (enemyItemPoint);
							}
						}
					}
				} else {
					Debug.Log("Did not find points upon which to instantiate enemies on increment " + i + " with variation name " + mazeSegments.layer1[i].gameObject.name);
				}
			}

		} else {
			Debug.LogError("No initial game elements are set!");
		}
	}

	//Will be used for ChooseElement()
	InstantiatableObjectReference[] requiredItems;
	InstantiatableObjectReference[] optionalItems;

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

	//Simple solution for the fact that each required item should be instantiated only ONCE.  
	int requiredItemsToInstantiateCount = 0;
	//Choose an element from the required or optional array.  
	InstantiatableObjectReference ChooseElement() {
		//Make sure that the array exists, and also make sure that all must-be instantiated items have not all already been
		if (requiredItems.Length != 0 && requiredItemsToInstantiateCount + 1 <= requiredItems.Length) {
			Debug.Log("Chose required item");
			InstantiatableObjectReference chosenItem = requiredItems [requiredItemsToInstantiateCount];
			Debug.Log("Name is " + chosenItem.elementReference.name);
			requiredItemsToInstantiateCount++;
			return chosenItem;
		} else {
			if (Random.Range (0, probabilityToInstantiateNullElement) == 0) {
				//This will end after the function returns.  
				while (true) {
					InstantiatableObjectReference chosenItem = optionalItems [Random.Range (0, optionalItems.Length)];
					if (Random.Range (0, chosenItem.probabilityOfInstantiation) == 0) {
						return chosenItem;
					}
				}
			} else {
				return null;
			}
		}
	}

}
