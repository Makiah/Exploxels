
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
 * 
 * This script manages the entire path the game takes.  Each event is static, so it only belongs to this class.  Each OnEnable() and OnDsiable() 
 * assigns and de-assigns events to this class.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class LevelEventManager : MonoBehaviour {

	public delegate void BaseInitialization();

	public delegate SlotScript[,] InventorySlotInitialization ();
	public static event InventorySlotInitialization CreateInventorySlots;
	public static event InventorySlotInitialization CreateHotbarSlots;

	public static event BaseInitialization InitializeSlots;

	public static event BaseInitialization EnableUIHideShow;

	public static event BaseInitialization InitializeUIHealthController;

	public delegate Transform[] TerrainInitialization();
	public static event TerrainInitialization InitializeTerrain;

	public static event BaseInitialization CreatePlayer;
	public static event BaseInitialization CreatePlayerReference;

	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  

	public delegate void InitializeDropSystem(SlotScript[,] inventorySlots);
	public static event InitializeDropSystem InitializePlayerDropSystem;

	public delegate void InitializePlayerCostume(Race playerRace);
	public static event InitializePlayerCostume InitializeCostume;
	public static event InitializePlayerCostume InitializeHotbarItems;

	public static event BaseInitialization InitializeCameraFunctions;
	public static event BaseInitialization InitializeBackgroundScroller;

	public delegate void TerrainCreation (Transform[] maze);
	public static event TerrainCreation CreateTerrainItems;

	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeEnemies;

	void Start() {

		//Inventory UI Initialization
		SlotScript[,] createdUISlots = CreateInventorySlots (); // Used with PanelLayout
		CreateHotbarSlots (); //Used with HotbarPanelLayout (Otherwise createdUISlots gets the hotbarslots return).  
		InitializeSlots (); //Used with SlotScript
		EnableUIHideShow (); //Used with InventoryHideShow
		InitializeUIHealthController(); //Used for UIHealthController

		//Lay out the level
		Transform[] initializedMaze = InitializeTerrain(); //Used with LevelLayout

		//Instantiate the player and initialize costumeManager
		bool useAltRace = GameObject.Find ("UI Data").GetComponent <UIData> ().chosenRace == 1 ? true : false;
		Race raceToUse;
		//Purpose: Get race from ResourceDatabase.  Requirements: Database initialized.
		if (useAltRace) 
			raceToUse = ResourceDatabase.GetRaceByParameter ("MinecrafterFemale");
		else 
			raceToUse = ResourceDatabase.GetRaceByParameter ("MinecrafterMale");  
		raceToUse.SetHeadVariation (0);   
		CreatePlayer(); //Used for CreateLevelItems (Instantiating player)
		CreatePlayerReference (); //Used for CreateLevelItems
		InitializeCostume(raceToUse);//Used for PlayerCostumeManager
		InitializeHotbarItems (raceToUse); //Used for initializing the HotbarManager.  
		InitializePlayer (); //Used for initializing the HumanoidBaseReferenceClass.  
		InitializePlayerDropSystem(createdUISlots); //Used for DropHandler
		InitializeCameraFunctions (); // Used for camera controller.  
		InitializeBackgroundScroller (); //Initialize the BackgroundScroller class.  

		//Initialize the enemies.  
		CreateTerrainItems(initializedMaze); //Used for instantiating the enemies and trees.  
		InitializeEnemyHealthControllers (); //Used for initializing CharacterHealthController.  
		InitializeEnemies(); //Used for all enemies (requires player being instantiated).  

		Debug.Log("Completed EventManager");

	}

}
