
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
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
		EventManager.CreateTerrainItems += CreateTerrainItems;
	}
	
	void OnDisable() {
		EventManager.CreateTerrainItems -= CreateTerrainItems;
	}

	public EnemyReferenceClass[] initialGameElements;

	void CreateTerrainItems (Transform[] mazeSegments) {

		for (int i = 0; i < mazeSegments.Length; i++) {

			if (mazeSegments[i].FindChild("Points") != null) {

				Transform enemyItemsTransform = mazeSegments [i].FindChild ("Points").FindChild ("EnemyItems");

				if (enemyItemsTransform.childCount != 0) {
						
					Transform[] enemyItemPoints = ScriptingUtilities.ParseChildrenFromTransform (enemyItemsTransform);
						
					foreach (Transform enemyItemPoint in enemyItemPoints) {
							
						bool instantiationPossible = true;

						EnemyReferenceClass chosenElement = initialGameElements [Random.Range (0, initialGameElements.Length)];

						if (chosenElement.headingNecessary) {
							if (mazeSegments [i].transform.rotation.eulerAngles.z == chosenElement.requiredHeading) {
								instantiationPossible = true;
							} else {
								instantiationPossible = false;
							}
						}

						if (Random.Range (0, chosenElement.probabilityOfInstantiation) == 0 && instantiationPossible) {
							GameObject createdElement = (GameObject)(Instantiate (chosenElement.elementReference, enemyItemPoint.position + chosenElement.elementReference.transform.localPosition, Quaternion.identity));
							createdElement.transform.SetParent (enemyItemPoint);
						}

					}

				}
			}
		}

	}

}
