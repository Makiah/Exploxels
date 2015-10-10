
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
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
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Sword", "A weak sword, but useful for survival.", 0, "Weapons/Wooden/WoodenSword/WoodenSword"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Hatchet", "A weak axe made for strong choppers.", 1, "Weapons/Wooden/WoodenHatchet/WoodenHatchet"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Pickaxe", "A weak axe to gather ore and rock.", 2, "Weapons/Wooden/WoodenPickaxe/WoodenPickaxe"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Bow", "A weak bow, useful for long range attacks.", 3, "Weapons/Wooden/WoodenBow/WoodenBow"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Diamond Sword", "A strong monster-chopping sword.", 4, "Weapons/Diamond/DiamondSword/DiamondSword"));

		//Crafting materialsi
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.CraftingMaterial, "Wood", "A vital material for any player", 0, "Items/Wood/Wood", "Items/Wood/UIWood"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.CraftingMaterial, "Wood Plank", "A vital wood refinement", 1, "Items/Wood/WoodPlank", "Items/Wood/UIWoodPlank"));


		//Ores
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Coal", "Useful for crafting arrow tips", 0, "Items/Ores/Coal/Coal", "Items/Ores/Coal/UICoal"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Diamond", "A tough material useful in epic battles", 1, "Items/Ores/Diamond/Diamond", "Items/Ores/Diamond/UIDiamond"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Emerald", "A pretty green gem", 2, "Items/Ores/Emerald/Emerald", "Items/Ores/Emerald/UIEmerald"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Gold", "A weak but easily enchantable item.", 3, "Items/Ores/Gold/Gold", "Items/Ores/Gold/UIGold"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Ruby", "A nice red gem", 4, "Items/Ores/Ruby/Ruby", "Items/Ores/Ruby/UIRuby"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Silver", "A good material for strong weapons", 5, "Items/Ores/Silver/Silver", "Items/Ores/Silver/UISilver"));

		//Other 
		masterItemList.Add (new ResourceReference(ResourceReference.ItemType.Other, "ExpNodule", "", 0, "Items/Other/ExpNodule"));

		/******************************************* RACES *******************************************/
		//Male Minecrafter
		string[] minecrafterMaleHeads = {"MinecrafterMaleHead1"};
		ResourceReference[] minecrafterMaleInitialItems = {
			GetItemByParameter ("Wooden Sword"), 
			GetItemByParameter ("Wooden Hatchet"),
			GetItemByParameter ("Wooden Pickaxe")
		};
		gameRaces.Add (new Race ("Races/MinecrafterMale/", minecrafterMaleHeads, "MinecrafterMale", 0, minecrafterMaleInitialItems));

		//Female Minecrafter
		string[] minecrafterFemaleHeads = {"MinecrafterFemaleHead1"};
		ResourceReference[] minecrafterFemaleInitialItems = {
			GetItemByParameter ("Wooden Bow"), 
			GetItemByParameter ("Diamond Sword")
		};
		gameRaces.Add (new Race ("Races/MinecrafterFemale/", minecrafterFemaleHeads, "MinecrafterFemale", 1, minecrafterFemaleInitialItems));

		/******************************************* COMBINATIONS *******************************************/
		//Wooden Sword
		masterItemCombinationList.Add(new ItemCombination (new UISlotContentReference[] {
			new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wood"), 1),
			new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wood"), 1)
		}, 
		new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wood Plank"), 1)));

		//Diamond Sword
		masterItemCombinationList.Add(new ItemCombination(new UISlotContentReference[] {
			new UISlotContentReference(ResourceDatabase.GetItemByParameter("Wood Plank"), 3), 
			new UISlotContentReference(ResourceDatabase.GetItemByParameter("Diamond"), 2)
		},
		new UISlotContentReference(ResourceDatabase.GetItemByParameter("Diamond Sword"), 1)));

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
