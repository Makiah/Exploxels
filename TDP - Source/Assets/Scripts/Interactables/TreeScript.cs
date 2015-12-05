
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script handles the tree position and segments.  When the hatchet swings, it cuts down one segment, and updates the tree.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class TreeScript : DropsItems {

	/**************************************** SCRIPT ****************************************/

	[System.Serializable]
	public class SpritePair {
		public Sprite baseTreeSprite, trunkTreeSprite;
		public Sprite[] topTreeSprite;
	}
	
	[SerializeField] private SpritePair[] treeTypes = null;
	[SerializeField] int treeType = 0;

	//Note: Segment 1 is at top, 3 is at bottom.  
	private SpriteRenderer top, segment1, segment2, segment3, segment4, segment5;
	private int currentlyActiveSegments;

	private int spriteToUseForTreeTop;

	protected override void MakeReferences() {
		DropReferenceClass woodDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter ("Wood"), 1, 1, 1);
		DropReferenceClass appleDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter ("Apple"), 1, 2, 2);
		DropReferenceClass sproutDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter ("Sprout"), 1, 1, 4);
		drops = new DropReferenceClass[]{woodDrop, appleDrop, sproutDrop};

		top = transform.FindChild ("Top").GetComponent <SpriteRenderer> ();
		segment1 = transform.FindChild ("Segment 1").GetComponent <SpriteRenderer> ();
		segment2 = transform.FindChild ("Segment 2").GetComponent <SpriteRenderer> ();
		segment3 = transform.FindChild ("Segment 3").GetComponent <SpriteRenderer> ();
		segment4 = transform.FindChild ("Segment 4").GetComponent <SpriteRenderer> ();
		segment5 = transform.FindChild ("Segment 5").GetComponent <SpriteRenderer> ();

		currentlyActiveSegments = Random.Range (1, 6);

		spriteToUseForTreeTop = Random.Range (0, treeTypes [treeType].topTreeSprite.Length);

		UpdateTree ();
	}

	//You could also just change the pivot point of the sprite and no longer have to deal with the top gameobject position.  
	void UpdateTree() {
		switch (currentlyActiveSegments) {
		case 0: 
			top.sprite = null;
			segment1.sprite = null;
			segment2.sprite = null;
			segment3.sprite = null;
			segment4.sprite = null;
			segment5.sprite = null;
			break;
		case 1:
			top.sprite = treeTypes[treeType].topTreeSprite[spriteToUseForTreeTop];
			top.gameObject.transform.localPosition = new Vector3 (0, 0.91f, 0);
			segment1.sprite = null;
			segment2.sprite = null;
			segment3.sprite = null;
			segment4.sprite = null;
			segment5.sprite = treeTypes[treeType].baseTreeSprite;
			break;
		case 2:
			top.sprite = treeTypes[treeType].topTreeSprite[spriteToUseForTreeTop];
			top.gameObject.transform.localPosition = new Vector3 (0, 1.59f, 0);
			segment1.sprite = null;
			segment2.sprite = null;
			segment3.sprite = null;
			segment4.sprite = treeTypes[treeType].trunkTreeSprite;
			segment5.sprite = treeTypes[treeType].baseTreeSprite;
			break;
		case 3: 
			top.sprite = treeTypes[treeType].topTreeSprite[spriteToUseForTreeTop];
			top.gameObject.transform.localPosition = new Vector3 (0, 2.29f, 0);
			segment1.sprite = null;
			segment2.sprite = null;
			segment3.sprite = treeTypes[treeType].trunkTreeSprite;
			segment4.sprite = treeTypes[treeType].trunkTreeSprite;
			segment5.sprite = treeTypes[treeType].baseTreeSprite;
			break;
		case 4: 
			top.sprite = treeTypes[treeType].topTreeSprite[spriteToUseForTreeTop];
			top.gameObject.transform.localPosition = new Vector3 (0, 2.99f, 0);
			segment1.sprite = null;
			segment2.sprite = treeTypes[treeType].trunkTreeSprite;
			segment3.sprite = treeTypes[treeType].trunkTreeSprite;
			segment4.sprite = treeTypes[treeType].trunkTreeSprite;
			segment5.sprite = treeTypes[treeType].baseTreeSprite;
			break;
		case 5: 
			top.sprite = treeTypes[treeType].topTreeSprite[spriteToUseForTreeTop];
			top.gameObject.transform.localPosition = new Vector3 (0, 3.69f, 0);
			segment1.sprite = treeTypes[treeType].trunkTreeSprite;
			segment2.sprite = treeTypes[treeType].trunkTreeSprite;
			segment3.sprite = treeTypes[treeType].trunkTreeSprite;
			segment4.sprite = treeTypes[treeType].trunkTreeSprite;
			segment5.sprite = treeTypes[treeType].baseTreeSprite;
			break;
		default: 
			Debug.LogError("Tree Initialization has not been correctly handled!!  Number was " + currentlyActiveSegments);
			break;
		}
	}

	public void TreeChopped() {
		currentlyActiveSegments--;
		UpdateTree ();
		DropItems ();
		if (currentlyActiveSegments == 0) {
			Destroy (gameObject);
		}

		CurrentLevelVariableManagement.GetMainObjectiveManager ().OnTreeChopped ();
	}

}
