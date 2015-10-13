
/*
 * Author: Makiah Bennett
 * Last edited: 13 September 2015
 * 
 * 9/13 - Added a constructor that allows a custom Texture2D as a UI cursor image.  
 * 
 * ResourceReference defines all of the game items.  While only really managed through the UI, ResourceDatabase
 * defines each ResourceReference.  In addition, the class contains the prefab of the item, so that it can be 
 * used in the game itself.  
 * 
 * 
 */


using UnityEngine;	
using System.Collections;

public class ResourceReference {
	
	public ItemType itemType;
	public string itemScreenName;
	public string itemDescription;
	public int localGroupID;
	public Sprite itemIcon;
	public GameObject inGamePrefab;
	public GameObject holdingPrefab;
	
	public enum ItemType {
		GameTool, 
		CraftingMaterial, 
		Ore, 
		Other
	}
	
	public ResourceReference (ItemType ctorItemType, string ctorItemScreenName, string ctorItemDescription, int ctorLocalGroupID, string localPath) {
		itemType = ctorItemType;
		itemScreenName = ctorItemScreenName;
		itemDescription = ctorItemDescription;
		localGroupID = ctorLocalGroupID;
		inGamePrefab = Resources.Load ("Prefabs/" + localPath) as GameObject;
		holdingPrefab = inGamePrefab;
		if (! (ctorItemType == ItemType.Other)) {
			itemIcon = inGamePrefab.GetComponent <SpriteRenderer> ().sprite;
		} else {
			itemIcon = null;
		}
	}

	public ResourceReference (ItemType ctorItemType, string ctorItemScreenName, string ctorItemDescription, int ctorLocalGroupID, string localPath, string customUIPrefabPath) {
		itemType = ctorItemType;
		itemScreenName = ctorItemScreenName;
		itemDescription = ctorItemDescription;
		localGroupID = ctorLocalGroupID;
		inGamePrefab = Resources.Load ("Prefabs/" + localPath) as GameObject;
		holdingPrefab = Resources.Load("Prefabs/" + customUIPrefabPath) as GameObject;
		itemIcon = holdingPrefab.GetComponent <SpriteRenderer> ().sprite;
	}

}

