
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * LevelLayout instantiates variations based on the size of each variation, and the length of the level.  Each
 * variable is defined in this class, and in the TerrainManager object.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

[System.Serializable]
public class TransferSegments {
	public GameObject startSegment;
	public GameObject[] l1Variations;
	public GameObject[] l2Variations;
	public GameObject[] l3Variations;
}

public class LevelLayout : MonoBehaviour {

	/*************************** INITIALIZATION ***************************/
	void OnEnable() {
		LevelEventManager.InitializeTerrain += InitializeTerrain;
	}
	
	void OnDisable() {
		LevelEventManager.InitializeTerrain -= InitializeTerrain;
	}

	/*************************** SCRIPT ***************************/
	public TransferSegments transferSegments = new TransferSegments();
	public int levelLength;	

	TerrainReferenceClass InitializeTerrain() {

		float currentXPosition = 0;

		TerrainReferenceClass createdMaze = new TerrainReferenceClass(levelLength);
		
		Transform parentMaze = new GameObject ("Maze").transform;

		GameObject instantiatedStartPoint = LayTerrainAsset(transferSegments.startSegment, Vector3.zero, Quaternion.identity, parentMaze);
		currentXPosition += GetSpriteSizeFromGameObject(instantiatedStartPoint).x / 2f;

		//For all levelLength values
		for (int i = 0; i < levelLength; i ++) {
			//Halfwidth and currentX position are used for all variations.  
			GameObject chosenObjectLayer1 = ChooseRandomObjectFromArray(transferSegments.l1Variations);
			float halfWidth = GetSpriteSizeFromGameObject(chosenObjectLayer1).x / 2f;
			currentXPosition += halfWidth;
			//Layer 1
			Vector3 pointToInstantiateLayer1Object = new Vector3(currentXPosition, 0, 0);
			GameObject instantiatedObjectLayer1 = LayTerrainAsset(chosenObjectLayer1, pointToInstantiateLayer1Object, Quaternion.identity, parentMaze);
			if (Random.Range(0, 2) == 1)
				instantiatedObjectLayer1.transform.localScale = new Vector3(-1, 1, 1);
			createdMaze.layer1[i] = instantiatedObjectLayer1.transform;
			//Layer 2
			GameObject chosenObjectLayer2 = ChooseRandomObjectFromArray(transferSegments.l2Variations);
			Vector3 pointToInstantiateLayer2Object = new Vector3(currentXPosition, - (GetSpriteSizeFromGameObject(chosenObjectLayer1).y + GetSpriteSizeFromGameObject(chosenObjectLayer2).y), 0);
			GameObject instantiatedObjectLayer2 = LayTerrainAsset(chosenObjectLayer2, pointToInstantiateLayer2Object, Quaternion.identity, parentMaze);
			if (Random.Range(0, 2) == 1)
				instantiatedObjectLayer2.transform.localScale = new Vector3(-1, 1, 1);
			createdMaze.layer2[i] = instantiatedObjectLayer2.transform;
			//Layer 3
			GameObject chosenObjectLayer3 = ChooseRandomObjectFromArray (transferSegments.l3Variations);
			Vector3 pointToInstantiateLayer3Object = new Vector3(currentXPosition, instantiatedObjectLayer2.transform.position.y - (GetSpriteSizeFromGameObject(chosenObjectLayer2).y + GetSpriteSizeFromGameObject(chosenObjectLayer3).y), 0); 
			GameObject instantiatedObjectLayer3 = LayTerrainAsset(chosenObjectLayer3, pointToInstantiateLayer3Object, Quaternion.identity, parentMaze);
			if (Random.Range(0, 2) == 1) 
				instantiatedObjectLayer3.transform.localScale = new Vector3(-1, 1, 1);
			createdMaze.layer3[i] = instantiatedObjectLayer3.transform;
			//Add current X position
			currentXPosition += halfWidth;
		}

		currentXPosition += GetSpriteSizeFromGameObject (transferSegments.startSegment).x / 2f;
		GameObject instantiatedEndPoint = LayTerrainAsset (transferSegments.startSegment, new Vector3 (currentXPosition, 0, 0), Quaternion.identity, parentMaze);
		instantiatedEndPoint.transform.localScale = new Vector3 (-1, 1, 1);

		float levelLengthX = instantiatedEndPoint.transform.position.x - instantiatedStartPoint.transform.position.x;

		GameObject.Find ("ManagementFrameworks").transform.FindChild ("GameVariables").GetComponent <VariableManagement> ().SetLevelLengthX (levelLengthX);
		return createdMaze;
	}

	GameObject ChooseRandomObjectFromArray (GameObject[] itemArray) {
		if (itemArray.Length != 0) {
			return itemArray [Random.Range (0, itemArray.Length)];
		} else {
			Debug.LogError("No terrain objects were found.  Check the array attached to the LevelLayout script.");
			return null;
		}
	}

	GameObject LayTerrainAsset(GameObject asset, Vector3 position, Quaternion rotation, Transform parentObj) {
		if (asset != null) {
			GameObject createdAsset = (GameObject)(Instantiate (asset, position, rotation));
			createdAsset.transform.SetParent (parentObj);
			return createdAsset;
		} else {
			Debug.LogError("Asset was null in the LevelLayout script");
			return null;
		}

	}

	Vector2 GetSpriteSizeFromGameObject(GameObject currObject) {
		if (currObject != null) {
			if (currObject.GetComponent <SpriteRenderer> () != null) {
				return currObject.GetComponent <SpriteRenderer> ().sprite.bounds.size;
			} else {
				Debug.LogError ("No SpriteRenderer could be found on one of the LevelLayout variations");
				return Vector2.zero;
			}
		} else {
			Debug.LogError("The size of a gameobject can not be determined from an empty object! (LevelLayout error)");
			return Vector2.zero;
		}

	}

}
