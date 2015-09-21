
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
	public GameObject[] variations;
}

public class LevelLayout : MonoBehaviour {

	/*************************** INITIALIZATION ***************************/
	void OnEnable() {
		EventManager.InitializeTerrain += InitializeTerrain;
	}
	
	void OnDisable() {
		EventManager.InitializeTerrain -= InitializeTerrain;
	}

	/*************************** SCRIPT ***************************/
	public TransferSegments transferSegments = new TransferSegments();
	public int levelLength;	

	Transform[] InitializeTerrain() {

		float currentXPosition = 0;

		Transform[] createdMaze = new Transform[levelLength];
		
		Transform parentMaze = new GameObject ("Maze").transform;

		GameObject instantiatedStartPoint = LayTerrainAsset(transferSegments.startSegment, Vector3.zero, Quaternion.identity, parentMaze);
		currentXPosition += GetSpriteSizeFromGameObject(instantiatedStartPoint).x / 2f;

		for (int i = 0; i < levelLength; i++) {
			GameObject chosenObject = ChooseRandomObjectFromArray(transferSegments.variations);
			float halfWidth = GetSpriteSizeFromGameObject(chosenObject).x / 2f;
			currentXPosition += halfWidth;

			GameObject instantiatedObject = LayTerrainAsset(chosenObject, new Vector3 (currentXPosition, 0, 0), Quaternion.identity, parentMaze);
			if (Random.Range(0, 2) == 1) {
				instantiatedObject.transform.localScale = new Vector3(-1, 1, 1);
			}

			currentXPosition += halfWidth;
			createdMaze[i] = instantiatedObject.transform;
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
			Debug.LogError("No terrain objects were detected.  Check the array attached to the LevelLayout script.");
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
