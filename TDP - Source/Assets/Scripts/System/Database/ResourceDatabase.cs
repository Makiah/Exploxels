
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

	public static List <ResourceReference> masterItemList = new List<ResourceReference> ();
	public static List <ItemCombination> masterItemCombinationList = new List<ItemCombination> ();

	public static List <Profession> gameProfessions = new List <Profession> ();
	
	public static void InitializeDatabase() {

		/******************************************* ITEMS *******************************************/
		//Tools
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Sword", "A weak sword, but useful for survival.", 0, "Weapons/Wooden/WoodenSword/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Hatchet", "A weak axe made for strong choppers.", 1, "Weapons/Wooden/WoodenHatchet/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Pickaxe", "A weak axe to gather ore and rock.", 2, "Weapons/Wooden/WoodenPickaxe/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Bow", "A weak bow, useful for long range attacks.", 3, "Weapons/Wooden/WoodenBow/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Diamond Sword", "A strong monster-chopping sword.", 4, "Weapons/Diamond/DiamondSword/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Spear", "A tough caveman implement", 5, "Weapons/Other/Spear/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Mace", "A weapon that strikes fear into the hearts of all", 6, "Weapons/Other/Mace"));

		//Crafting materials
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.CraftingMaterial, "Wood", "A vital material for any player", 0, "Items/Wood/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.CraftingMaterial, "Wood Plank", "A vital wood refinement", 1, "Items/Wood/"));

		//Ores
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Coal", "Useful for crafting arrow tips", 0, "Items/Ores/Coal/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Diamond", "A tough material useful in epic battles", 1, "Items/Ores/Diamond/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Emerald", "A pretty green gem", 2, "Items/Ores/Emerald/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Gold", "A weak but easily enchantable item.", 3, "Items/Ores/Gold/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Ruby", "A nice red gem", 4, "Items/Ores/Ruby/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Silver", "A good material for strong weapons", 5, "Items/Ores/Silver/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Ore, "Iron", "A dull yet tough metal.", 6, "Items/Ores/Iron/"));

		//Food
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Food, "Apple", "A yummy snack.", 0, "Items/Food/Apple/"));
		masterItemList.Add(new ResourceReference(ResourceReference.ItemType.Food, "Sprout", "The start of a new beginning", 1, "Items/Food/Sprout/"));

		//Other 
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Other, "ExpNodule", "", 0, "Items/Other/ExpNodule/"));
		masterItemList.Add (new ResourceReference (ResourceReference.ItemType.Other, "Fire", "A life changing implement of cavemen", 1, "Items/Other/Fire/"));

		/******************************************* RACES *******************************************/
		//Gatherer
		UISlotContentReference[] gathererInitialItems = new UISlotContentReference[]{};
		gameProfessions.Add (new Profession("Professions/Gatherer/", "Gatherer", 0, gathererInitialItems));

		//Hunter
		UISlotContentReference[] hunterInitialItems = new UISlotContentReference[]{
			new UISlotContentReference(GetItemByParameter("Spear"), 1)
		};
		gameProfessions.Add (new Profession ("Professions/Hunter/", "Hunter", 1, hunterInitialItems));

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

		//Fire
		masterItemCombinationList.Add (new ItemCombination (new UISlotContentReference[] {
			new UISlotContentReference(ResourceDatabase.GetItemByParameter("Wood"), 5), 
			new UISlotContentReference(ResourceDatabase.GetItemByParameter("Coal"), 2)
		}, 
		new UISlotContentReference (ResourceDatabase.GetItemByParameter ("Fire"), 1)));

	}

	public static Profession GetRaceByParameter(string specifiedName) {
		for (int i = 0; i < gameProfessions.Count; i++) {
			if (gameProfessions[i].name == specifiedName) 
				return gameProfessions[i];
		}

		return null;
	}

	public static Profession GetRaceByParameter(int specifiedID) {
		for (int i = 0; i < gameProfessions.Count; i++) {
			if (gameProfessions[i].professionID == specifiedID) 
				return gameProfessions[i];
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
		return gameProfessions.Count;
	}

}
