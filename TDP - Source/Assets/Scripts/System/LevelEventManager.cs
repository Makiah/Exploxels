
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
	public static event BaseInitialization InitializeHealthPanels;

	public static event BaseInitialization InitializeInteractablePanelController;
	public static event BaseInitialization InitializeInteractablePanels;

	public static event BaseInitialization InitializeUISpeechControl;

	public delegate TerrainReferenceClass TerrainInitialization();
	public static event TerrainInitialization InitializeTerrain;

	public static event BaseInitialization CreatePlayer;
	public static event BaseInitialization CreatePlayerReference;

	public static event BaseInitialization InitializeLightingSystem;

	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  

	public delegate void InitializeDropSystem(SlotScript[,] inventorySlots);
	public static event InitializeDropSystem InitializePlayerDropSystem;

	public delegate void InitializePlayerCostume(Race playerRace);
	public static event InitializePlayerCostume InitializeCostume;
	public static event InitializePlayerCostume InitializeHotbarItems;

	public static event BaseInitialization InitializeCameraFunctions;
	public static event BaseInitialization InitializeBackgroundScroller;

	public delegate void TerrainCreation (TerrainReferenceClass maze);
	public static event TerrainCreation CreateTerrainItems;

	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeEnemies;
	
	public static event BaseInitialization InitializeNPCPanelControllers;
	public static event BaseInitialization InitializeNPCs;

	void Start() {

		//Note: This would be a lot easier if I could figure out a way to pass an event in as a method parameter, but all attempts have not worked.  

		//Inventory UI Initialization
		SlotScript[,] createdUISlots = null;
		if (CreateInventorySlots != null) createdUISlots = CreateInventorySlots (); else Debug.LogError("CreateInventorySlots was null!"); // Used with PanelLayout
		if (CreateHotbarSlots != null) CreateHotbarSlots (); else Debug.LogError("CreateHotbarSlots was null!"); //Used with HotbarPanelLayout (Otherwise createdUISlots gets the hotbarslots return).  
		if (InitializeSlots != null) InitializeSlots (); else Debug.LogError("InitializeSlots was null!");//Used with SlotScript
		if (EnableUIHideShow != null) EnableUIHideShow (); else Debug.LogError("EnableUIHideShow was null!");//Used with InventoryHideShow
		if (InitializeUIHealthController != null) InitializeUIHealthController(); else Debug.LogError("InitializeUIHealthController was null!"); //Used for UIHealthController
		if (InitializeHealthPanels != null) InitializeHealthPanels (); else Debug.LogError("InitializeHealthPanels was null!"); //Used for HealthPanelReference and PlayerHealthPanelReference.  

		if (InitializeInteractablePanelController != null) InitializeInteractablePanelController(); else Debug.LogError("InitializeInteractablePanelController was null!");
		if (InitializeInteractablePanels != null) InitializeInteractablePanels(); else Debug.LogError("InitializeInteractablePanels was null!");

		if (InitializeUISpeechControl != null) InitializeUISpeechControl (); else Debug.LogError("InitializeUISpeechControl was null!");

		//Lay out the level
		TerrainReferenceClass initializedMaze = null;
		if (InitializeTerrain != null) initializedMaze = InitializeTerrain(); else Debug.LogError("InitializeTerrain was null!"); //Used with LevelLayout

		//Instantiate the player and initialize costumeManager
		bool useAltRace = GameObject.Find ("UI Data").GetComponent <UIData> ().chosenRace == 1 ? true : false;
		//Purpose: Get race from ResourceDatabase.  Requirements: Database initialized.
		Race raceToUse = useAltRace ? ResourceDatabase.GetRaceByParameter ("MinecrafterFemale") : ResourceDatabase.GetRaceByParameter ("MinecrafterMale");
		raceToUse.SetHeadVariation (0);

		if (CreatePlayer != null) CreatePlayer(); else Debug.LogError("CreatePlayer was null!"); //Used for CreateLevelItems (Instantiating player)
		if (CreatePlayerReference != null) CreatePlayerReference (); else Debug.LogError("CreatePlayerReference was null!"); //Used for CreateLevelItems
		if (InitializeLightingSystem != null) InitializeLightingSystem (); else Debug.LogError("InitializeLightingSystem was null!"); //Used for LightingManager.  
		if (InitializeCostume != null) InitializeCostume(raceToUse); else Debug.LogError("InitializeCostume was null!"); //Used for PlayerCostumeManager
		if (InitializeHotbarItems != null) InitializeHotbarItems (raceToUse); else Debug.LogError("InitializeHotbarItems was null!"); //Used for initializing the HotbarManager.  
		if (InitializePlayer != null) InitializePlayer (); else Debug.LogError("InitializePlayer was null!"); //Used for initializing the HumanoidBaseReferenceClass.  
		if (InitializePlayerDropSystem != null) InitializePlayerDropSystem(createdUISlots); else Debug.LogError("InitializePlayerDropSystem was null!"); //Used for DropHandler
		if (InitializeCameraFunctions != null) InitializeCameraFunctions (); else Debug.LogError("InitializeCameraFunctions was null!"); // Used for camera controller.  
		if (InitializeBackgroundScroller != null) InitializeBackgroundScroller (); else Debug.LogError("InitializeBackgroundScroller was null!"); //Initialize the BackgroundScroller class.  

		//Initialize the enemies.  
		if (CreateTerrainItems != null) CreateTerrainItems(initializedMaze); else Debug.LogError("CreateTerrainItems was null!"); //Used for instantiating the enemies and trees.  
		if (InitializeEnemyHealthControllers != null) InitializeEnemyHealthControllers (); else Debug.LogError("InitializeEnemyHealthControllers was null!"); //Used for initializing CharacterHealthController.  
		if (InitializeEnemies != null) InitializeEnemies(); else Debug.LogError("InitializeEnemies was null!"); //Used for all enemies (requires player being instantiated).  

		if (InitializeNPCPanelControllers != null) InitializeNPCPanelControllers(); else Debug.LogError("InitializeNPCPanelControllers was null!");
		if (InitializeNPCs != null) InitializeNPCs(); else Debug.LogError("InitializeNPCs was null!");

		Debug.Log("Completed EventManager");

	}

}
