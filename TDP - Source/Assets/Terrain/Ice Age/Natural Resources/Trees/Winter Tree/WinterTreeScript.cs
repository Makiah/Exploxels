using UnityEngine;
using System.Collections;

public class WinterTreeScript : MonoBehaviour {

	[System.Serializable]
	private class TreeSprites {
		//Actaual wood sprites.  
		[SerializeField] public Sprite treeBase = null, trunk = null, leftTrunkBreak = null, rightTrunkBreak = null, branch = null, leftBranchEnding = null, rightBranchEnding = null, bareTreeTop = null;
		[SerializeField] public Sprite leftLeafyEnd = null, rightLeafyEnd = null, leafBlock = null, treeTop = null;
	}

	[SerializeField] TreeSprites snowTreeSprites;

	//The tree height (currently at min). 
	int treeHeight = 4;
	//Required sprites to construct the tree.  

	// Use this for initialization
	void Start () {
		//The max height is 8.  
		treeHeight += Random.Range (0, 5);
		UpdateTree ();
	}
	
	// Update is called once per frame
	void UpdateTree () {
		//This will keep track of the height while the tree is built.  
		float currentHeight = .35f;
		//Add the base.  
		InstantiateSpriteAtLocation(snowTreeSprites.treeBase, new Vector2(0, currentHeight));
		currentHeight += 0.7f;

		//Add the actual segments.  Add branches based off of some other constant.   
		for (int i = 0; i < treeHeight - 1; i++) {
			//A 1 in 3 chance of making a branch.  
			if (Random.Range (0, 2) == 0) {
				//Choose the side that the branch faces.  
				if (Random.Range (0, 2) == 0) {
					//Left
					InstantiateSpriteAtLocation (snowTreeSprites.leftTrunkBreak, new Vector2(0, currentHeight));
					//Branches have a max length of treeHeight / 3.  Only go to Random.Range(...) - 1 because the branch ending has to be created.  l
					int branchLength = Random.Range(1, (int) treeHeight / 3);
					for (int j = 1; j <= branchLength - 1; j++) {
						InstantiateSpriteAtLocation (snowTreeSprites.branch, new Vector2 (j * -0.7f, currentHeight));
					}

					//Instantiate the branch ending.  
					InstantiateSpriteAtLocation (snowTreeSprites.leftBranchEnding, new Vector2(-0.7f * branchLength, currentHeight));

				} else {
					//Right
					InstantiateSpriteAtLocation (snowTreeSprites.rightTrunkBreak, new Vector2(0, currentHeight));
					//Branches have a max length of treeHeight / 3.  Only go to Random.Range(...) - 1 because the branch ending has to be created.  l
					int branchLength = Random.Range(1, (int) treeHeight / 3);
					for (int j = 1; j <= branchLength - 1; j++) {
						InstantiateSpriteAtLocation (snowTreeSprites.branch, new Vector2 (j * 0.7f, currentHeight));
					}

					//Instantiate the branch ending.  
					InstantiateSpriteAtLocation (snowTreeSprites.rightBranchEnding, new Vector2(0.7f * branchLength, currentHeight));

				}
			} else {
				InstantiateSpriteAtLocation(snowTreeSprites.trunk, new Vector2(0, currentHeight));
			}

			//Add the height of the new segment to the current height.  
			currentHeight += 0.7f;
		}

		//Add the leafy part of it.  
		int treeLeafHeight = (int) (treeHeight * 2f/3) + 1;
		int treeLeafCurrWidth = 2 * treeLeafHeight + 1;
		for (int i = 0; i < treeLeafHeight; i++) {
			float currentXVal = -treeLeafCurrWidth / 2 * .7f;
			for (int j = 0; j < treeLeafCurrWidth; j++) {
				//Instantiate in the position.  
				Vector2 location = new Vector2 (currentXVal, currentHeight);
				if (j == 0)
					InstantiateSpriteAtLocation (snowTreeSprites.leftLeafyEnd, location);
				else if (j == treeLeafCurrWidth - 1)
					InstantiateSpriteAtLocation (snowTreeSprites.rightLeafyEnd, location);
				else
					InstantiateSpriteAtLocation (snowTreeSprites.leafBlock, location);

				currentXVal += 0.7f;
			}

			treeLeafCurrWidth -= 2;
			currentHeight += 0.7f;
		}

		InstantiateSpriteAtLocation(snowTreeSprites.treeTop, new Vector2(0, currentHeight));
	}

	//Instantiate a Sprite at a location.  
	GameObject InstantiateSpriteAtLocation(Sprite sprite, Vector2 localPosition) {
		//Create the actual object.  
		GameObject createdPortion = new GameObject ("Tree Portion");
		//Add the sprite.  
		createdPortion.AddComponent <SpriteRenderer> ();
		createdPortion.GetComponent <SpriteRenderer> ().sprite = sprite;
		//Add the local position.  
		createdPortion.transform.SetParent (transform);
		createdPortion.transform.localPosition = localPosition;

		//Return the created segment of the tree.  
		return createdPortion;
	}
}
