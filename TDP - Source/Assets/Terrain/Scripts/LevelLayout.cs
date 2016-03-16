
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
public class VariationReference {
	public GameObject variationReference;
	public bool canBeFlipped;
}

[System.Serializable]
public class TransferSegments {
	public GameObject startSegment;
	public VariationReference[] introductoryVariations;
	public VariationReference[] l1Variations;
	public VariationReference[] l2Variations;
	public VariationReference[] l3Variations;
	public VariationReference[] endVariations;
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

	//Takes the transfer segments defined earlier and instantiates them based on sprite size.  
	TerrainReferenceClass InitializeTerrain() {
		//This is recorded and changed as more terrain is added.  
		float currentXPosition = 0;
		//This will be returned once filled in.  
		TerrainReferenceClass createdMaze = new TerrainReferenceClass(levelLength + transferSegments.introductoryVariations.Length + transferSegments.endVariations.Length);
		//This holds the main maze part.  
		Transform parentMaze = new GameObject ("Maze").transform;
		parentMaze.localPosition = new Vector3 (0, 0, -8);

		//Instantiate the starting point.  
		GameObject instantiatedStartPoint = LayTerrainAsset(transferSegments.startSegment, Vector3.zero, Quaternion.identity, parentMaze);
		instantiatedStartPoint.transform.localPosition = Vector3.zero;
		currentXPosition += GetSpriteSizeFromGameObject(instantiatedStartPoint).x / 2f;

		//Apparently this can be null (weird error)
		for (int i = 0; i < transferSegments.introductoryVariations.Length; i++) {
			//Instantiate the next introductory variation.   
			float halfWidth = GetSpriteSizeFromGameObject (transferSegments.introductoryVariations [i].variationReference.gameObject).x / 2f;
			currentXPosition += halfWidth;
			GameObject createdAsset = LayTerrainAsset (transferSegments.introductoryVariations [i].variationReference.gameObject, new Vector3 (currentXPosition, 0, 0), Quaternion.identity, parentMaze);
			createdMaze.layer1[i] = createdAsset.transform;
			currentXPosition += halfWidth;
		}

		//For all levelLength values.  Start at the length of introductory variations and move on from there to the level length minus the number of .  
		for (int i = transferSegments.introductoryVariations.Length; i < levelLength - transferSegments.endVariations.Length; i++) {
			//Half-Width and currentX position are used for all variations.  
			VariationReference chosenVariationLayer1 = ScriptingUtilities.GetRandomObjectFromArray(transferSegments.l1Variations);
			GameObject chosenObjectLayer1 = chosenVariationLayer1.variationReference;
			float halfWidth = GetSpriteSizeFromGameObject(chosenObjectLayer1).x / 2f;
			currentXPosition += halfWidth;
			//Layer 1
			Vector3 pointToInstantiateLayer1Object = new Vector3(currentXPosition, 0, 0);
			GameObject instantiatedObjectLayer1 = LayTerrainAsset(chosenObjectLayer1, pointToInstantiateLayer1Object, Quaternion.identity, parentMaze);
			if (Random.Range(0, 2) == 1 && chosenVariationLayer1.canBeFlipped)
				instantiatedObjectLayer1.transform.localScale = new Vector3(-1, 1, 1);
			createdMaze.layer1[i + transferSegments.introductoryVariations.Length] = instantiatedObjectLayer1.transform;
			//Layer 2
			//Make sure that layer 2 objects do exist.  
			if (transferSegments.l2Variations.Length != 0) {
				VariationReference chosenVariationLayer2 = ScriptingUtilities.GetRandomObjectFromArray(transferSegments.l2Variations);
				GameObject chosenObjectLayer2 = chosenVariationLayer2.variationReference;
				Vector3 pointToInstantiateLayer2Object = new Vector3(currentXPosition, - (GetSpriteSizeFromGameObject(chosenObjectLayer1).y / 2f + GetSpriteSizeFromGameObject(chosenObjectLayer2).y / 2f), 0);
				GameObject instantiatedObjectLayer2 = LayTerrainAsset(chosenObjectLayer2, pointToInstantiateLayer2Object, Quaternion.identity, parentMaze);
				if (Random.Range(0, 2) == 1 && chosenVariationLayer2.canBeFlipped)
					instantiatedObjectLayer2.transform.localScale = new Vector3(-1, 1, 1);
				createdMaze.layer2[i] = instantiatedObjectLayer2.transform;
				//Layer 3
				//Make sure that layer 3 objects do exist (dependent on whether layer 2 objects exist as well).  
				if (transferSegments.l3Variations.Length != 0) {
					VariationReference chosenVariationLayer3 = ScriptingUtilities.GetRandomObjectFromArray(transferSegments.l3Variations);
					GameObject chosenObjectLayer3 = chosenVariationLayer3.variationReference;
					Vector3 pointToInstantiateLayer3Object = new Vector3(currentXPosition, instantiatedObjectLayer2.transform.position.y - (GetSpriteSizeFromGameObject(chosenObjectLayer2).y / 2f + GetSpriteSizeFromGameObject(chosenObjectLayer3).y / 2f), 0); 
					GameObject instantiatedObjectLayer3 = LayTerrainAsset(chosenObjectLayer3, pointToInstantiateLayer3Object, Quaternion.identity, parentMaze);
					if (Random.Range(0, 2) == 1 && chosenVariationLayer3.canBeFlipped) 
					instantiatedObjectLayer3.transform.localScale = new Vector3(-1, 1, 1);
					createdMaze.layer3[i] = instantiatedObjectLayer3.transform;
				}
			}
			//Add current X position
			currentXPosition += halfWidth;
		}

		//Apparently this can be null (weird error)
		for (int i = 0; i < transferSegments.endVariations.Length; i++) {
			//Instantiate the next introductory variation.   
			float halfWidth = GetSpriteSizeFromGameObject (transferSegments.endVariations [i].variationReference.gameObject).x / 2f;
			currentXPosition += halfWidth;
			GameObject createdAsset = LayTerrainAsset (transferSegments.endVariations [i].variationReference.gameObject, new Vector3 (currentXPosition, 0, 0), Quaternion.identity, parentMaze);
			createdMaze.layer1[i + levelLength + transferSegments.introductoryVariations.Length] = createdAsset.transform;
			currentXPosition += halfWidth;
		}

		//Instantiate the end segment.  
		currentXPosition += GetSpriteSizeFromGameObject(transferSegments.startSegment).x / 2f;
		GameObject instantiatedEndPoint = LayTerrainAsset (transferSegments.startSegment, new Vector3 (currentXPosition, 0, 0), Quaternion.identity, parentMaze);
		instantiatedEndPoint.transform.localScale = new Vector3 (-1, 1, 1);

		instantiatedEndPoint.transform.localPosition = new Vector3 (instantiatedEndPoint.transform.position.x, instantiatedEndPoint.transform.position.y, 0);

		//Calculate the float level length, then send it over to CurrentLevelVariableManagement.  
		float levelLengthX = instantiatedEndPoint.transform.position.x - instantiatedStartPoint.transform.position.x;

		//Set the level length (will be used for things like the particle effect).  
		CurrentLevelVariableManagement.SetLevelLengthX (levelLengthX);

		return createdMaze;
	}

	//Used as a convenient method of instantiating terrain segments.  
	GameObject LayTerrainAsset(GameObject asset, Vector3 position, Quaternion rotation, Transform parentObj) {
		if (asset != null) {
			GameObject createdAsset = (GameObject)(Instantiate (asset, position, rotation));
			createdAsset.transform.SetParent (parentObj);
			createdAsset.transform.localPosition = new Vector3(createdAsset.transform.position.x, createdAsset.transform.position.y, 0);
			return createdAsset;
		} else {
			Debug.LogError("Asset was null in the LevelLayout script");
			return null;
		}

	}

	//Only works if the Game Object has a SpriteRenderer component.  
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
