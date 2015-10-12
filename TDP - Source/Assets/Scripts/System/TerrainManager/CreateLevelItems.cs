
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
 * 
 * CreateLevelItems manages the enemies of the game, and instantiates them at pre-set points in each variation.  
 * This is done through the EnemyReferenceClass probability indicators, which are defined in this class.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class CreateLevelItems : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.CreateTerrainItems += CreateTerrainItems;
		LevelEventManager.CreatePlayer += InstantiatePlayer;
	}
	
	void OnDisable() {
		LevelEventManager.CreateTerrainItems -= CreateTerrainItems;
		LevelEventManager.CreatePlayer -= InstantiatePlayer;
	}


	public GameObject playerToInstantiate;
	public Vector3 pointToInstantiatePlayerAt;

	public int probabilityToInstantiateNullElement;
	public EnemyReferenceClass[] initialGameElements;

	void InstantiatePlayer() {
		Instantiate (playerToInstantiate, pointToInstantiatePlayerAt, Quaternion.identity);
		Debug.Log ("Instantiated Player");
	}

	void CreateTerrainItems (TerrainReferenceClass mazeSegments) {

		for (int i = 0; i < mazeSegments.layer1.Length; i++) {

			if (mazeSegments.layer1[i].FindChild("Points") != null) {

				Transform enemyItemsTransform = mazeSegments.layer1[i].FindChild ("Points").FindChild ("EnemyItems");

				if (enemyItemsTransform.childCount != 0) {
						
					Transform[] enemyItemPoints = ScriptingUtilities.ParseChildrenFromTransform (enemyItemsTransform);
						
					foreach (Transform enemyItemPoint in enemyItemPoints) {

						EnemyReferenceClass chosenElement = ChooseElement();

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
	}

	EnemyReferenceClass ChooseElement() {
		if (Random.Range (0, probabilityToInstantiateNullElement) == 0) {
			while (true) {
				EnemyReferenceClass chosenItem = initialGameElements [Random.Range (0, initialGameElements.Length)];
				if (Random.Range (0, chosenItem.probabilityOfInstantiation) == 0) {
					return chosenItem;
				}
			}
		} else {
			return null;
		}
	}

}
