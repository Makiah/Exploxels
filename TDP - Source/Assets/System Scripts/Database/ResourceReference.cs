
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
	//Used for organizational purposes.  
	public readonly ItemType itemType;
	//Used to help load the character.  
	public readonly string itemScreenName;
	//Description of the item.  
	public readonly string itemDescription;
	//Used to get an item (not by name).  
	public readonly int localGroupID;
	//The icon of the item.  
	public readonly Sprite itemIcon;
	//The GameObject that is held by the player (or enemy).  
	public readonly GameObject playerHoldingPrefab;

	//Used for organizational convenience.  
	public enum ItemType {
		GameTool, 
		CraftingMaterial, 
		Ore, 
		Food, 
		ShipPart, 
		Other
	}

	//Note: These constructors require each folder to have naming conventions.  
	//Player holding prefab for "Mace" should be under Prefabs/(path specified here)/Mace.  
	//Drop Prefab for "Mace" should be under Prefabs/(path specified here)/MaceDrop.  
	//Custom item icon (for inventory and cursor) should be under Prefabs/(path specified here)/MaceCustomIcon.  
	//These components are loaded automatically if they exist.  If they do not, very specific errors should be returned.  

	//Constructor that loads all required components.  
	public ResourceReference (ItemType ctorItemType, string ctorItemScreenName, string ctorItemDescription, int ctorLocalGroupID, string path) {
		//Load the items that are a definite.  
		itemType = ctorItemType;
		itemScreenName = ctorItemScreenName;
		itemDescription = ctorItemDescription;
		localGroupID = ctorLocalGroupID;

		//Load the holding prefab.  
		string inGamePrefabPath = "Prefabs/" + path + itemScreenName;
		if ((Resources.Load (inGamePrefabPath) as GameObject) != null) 
			playerHoldingPrefab = Resources.Load (inGamePrefabPath) as GameObject;
		else 
			Debug.LogError ("Could not load item " + itemScreenName + " holding prefab from path " + inGamePrefabPath);

		//Set the sprite icon if it exists.  
		string customIconPath = "Prefabs/" + path + itemScreenName + "CustomIcon";
		if (Resources.Load <Sprite> (customIconPath) != null) {
			itemIcon = Resources.Load <Sprite> (customIconPath);
		} else {
			Debug.Log ("Could not load custom icon for item " + itemScreenName + " at path " + customIconPath);
			if (playerHoldingPrefab.GetComponent <SpriteRenderer> () != null) 
				itemIcon = playerHoldingPrefab.GetComponent <SpriteRenderer> ().sprite;
			else 
				Debug.Log ("Item " + itemScreenName + " does not have an item icon");
		}
	}

}

