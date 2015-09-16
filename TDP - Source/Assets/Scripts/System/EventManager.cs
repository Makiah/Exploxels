
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script manages the entire path the game takes.  Each event is static, so it only belongs to this class.  Each OnEnable() and OnDsiable() 
 * assigns and de-assigns events to this class.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	public delegate void BaseInitialization();
	public static event BaseInitialization ItemDatabaseInitialization;

	public delegate SlotScript[,] InventorySlotInitialization ();
	public static event InventorySlotInitialization CreateInventorySlots;

	public static event BaseInitialization EnableHealthBars;
	public static event BaseInitialization ReferenceUIChildren;

	public static event BaseInitialization ReferenceLocalClasses;

	public delegate Transform[] TerrainInitialization ();
	public static event TerrainInitialization InitializeTerrain;

	public delegate void LevelItemCreation(Transform[] mazeSegments);
	public static event LevelItemCreation CreateTerrainItems;

	public static event BaseInitialization InitializeEnemies;

	//Includes player UI stuff (health bar).  
	public static event BaseInitialization InitializePlayer;

	public delegate void PlayerCostumeSetup(Race costumeParamsSent);
	public static event PlayerCostumeSetup InitializeCostume;
	public static event PlayerCostumeSetup InitializeHotbarItems;

	//Multi-dimensional array to store rows and columns.  
	public delegate void InitializePlayerInventoryScripts(SlotScript[,] uiSlots);
	public static event InitializePlayerInventoryScripts InitializePickupSystem;

	public static event BaseInitialization EnableUIHideShow;

	void Start() {
		//UI Stuff
		ItemDatabaseInitialization ();
		SlotScript[,] createdUISlots = CreateInventorySlots ();
		ReferenceUIChildren ();

		//Reference general classes
		ReferenceLocalClasses ();

		EnableHealthBars ();

		//Create level
		Transform[] initializedMaze = InitializeTerrain ();
		CreateTerrainItems (initializedMaze);
		InitializeEnemies ();

		//Normally, the UI would occur now, and we would get costume parameters from that.  

		//Create the player costume
		Race minecrafter = ResourceDatabase.GetRaceByParameter ("Minecrafter");
		minecrafter.SetHeadVariation (0);
		InitializePlayer ();
		InitializeCostume (minecrafter);
		InitializeHotbarItems (minecrafter);

		InitializePickupSystem (createdUISlots);
		EnableUIHideShow ();
	}

}
