
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * The ResourceDatabase controls the items that are used during the game, by defining the items beforehand during the 
 * InitializeDatabase part of the EventManager script. 
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceDatabase : MonoBehaviour {

	void OnEnable() {
		UIEventManager.InitializeResourceDatabase += InitializeDatabase;
	}

	void OnDisable() {
		UIEventManager.InitializeResourceDatabase -= InitializeDatabase;
	}

	public static List <ResourceReference> masterItemList = new List<ResourceReference> ();
	public static List <ItemCombination> masterItemCombinationList = new List<ItemCombination> ();

	public static List <Race> gameRaces = new List <Race> ();
	
	void InitializeDatabase() {

		DontDestroyOnLoad (this.gameObject);

		/******************************************* ITEMS *******************************************/
		//Tools
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Sword", "A weak sword, but useful for survival.", 0, "Weapons/WoodenSword/WoodenSword"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Hatchet", "A weak axe made for strong choppers.", 1, "Weapons/WoodenHatchet/WoodenHatchet"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Pickaxe", "A weak axe to gather ore and rock.", 2, "Weapons/WoodenPickaxe/WoodenPickaxe"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Bow", "A weak bow, useful for long range attacks.", 3, "Weapons/WoodenBow/WoodenBow"));

		//Crafting materials
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.CraftingMaterial, "Wood", "A vital material for any player", 0, "Items/Wood", "Items/UIWood"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.CraftingMaterial, "Rock", "Useful for crafting arrow tips", 1, "Items/Rock", "Items/UIRock"));

		string[] minecrafterMaleHeads = {"MinecrafterMaleHead1"};
		ResourceReference[] minecrafterMaleInitialItems = {
			GetItemByParameter ("Wooden Sword"), 
			GetItemByParameter ("Wooden Hatchet"), 
		};
		gameRaces.Add (new Race ("Races/MinecrafterMale/", minecrafterMaleHeads, "MinecrafterMale", 0, minecrafterMaleInitialItems));

		string[] minecrafterFemaleHeads = {"MinecrafterFemaleHead1"};
		ResourceReference[] minecrafterFemaleInitialItems = {
			GetItemByParameter ("Wooden Bow"), 
			GetItemByParameter ("Wooden Pickaxe"), 
		};
		gameRaces.Add (new Race ("Races/MinecrafterFemale/", minecrafterFemaleHeads, "MinecrafterFemale", 1, minecrafterFemaleInitialItems));

		/******************************************* COMBINATIONS *******************************************/
		masterItemCombinationList.Add(new ItemCombination (new UISlotContentReference[] {
			new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wood"), 2),
			new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wood"), 1)
		}, 

		new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wooden Sword"), 2)));

	}

	public static Race GetRaceByParameter(string specifiedName) {
		for (int i = 0; i < gameRaces.Count; i++) {
			if (gameRaces[i].name == specifiedName) 
				return gameRaces[i];
		}

		return null;
	}

	public static Race GetRaceByParameter(int specifiedID) {
		for (int i = 0; i < gameRaces.Count; i++) {
			if (gameRaces[i].raceID == specifiedID) 
				return gameRaces[i];
		}
		
		return null;
	}

	public static ResourceReference GetItemByParameter(string specifiedName) {
		for (int i = 0; i < masterItemList.Count; i++) {
			if (masterItemList[i].itemScreenName == specifiedName) 
				return masterItemList[i];
		}
		
		return null;
	}
	
	public static ResourceReference GetItemByParameter(ResourceReference.ItemType toolType, int specifiedID) {
		for (int i = 0; i < masterItemList.Count; i++) {
			if (toolType == masterItemList[i].itemType)
				if (masterItemList[i].localGroupID == specifiedID) 
					return masterItemList[i];
		}
		
		return null;
	}

	public static int GetNumberOfItemsInGame() {
		return masterItemList.Count;
	}

	public static int GetNumberOfRacesInGame() {
		return gameRaces.Count;
	}

}
