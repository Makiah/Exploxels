
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

	//Pretty much contains every event that does not require a parameter or a return type.  
	public delegate void BaseInitialization();

	public delegate SlotScript[,] InventorySlotInitialization ();
	public static event InventorySlotInitialization CreateInventorySlots;
	public static event InventorySlotInitialization CreateHotbarSlots;

	public static event BaseInitialization InitializeSlots;
	public static event BaseInitialization InitializeHotbarManager;

	public static event BaseInitialization EnableUIHideShow;

	public static event BaseInitialization InitializeUIHealthController;
	public static event BaseInitialization InitializeHealthPanels;

	public static event BaseInitialization InitializeInteractablePanelController;
	public static event BaseInitialization InitializeInteractablePanels;

	public static event BaseInitialization InitializeUISpeechControl;

	public static event BaseInitialization InitializeProfessionChoiceManager;

	public delegate TerrainReferenceClass TerrainInitialization();
	public static event TerrainInitialization InitializeTerrain;

	public static event BaseInitialization CreatePlayer;
	public static event BaseInitialization CreatePlayerReference;

	public static event BaseInitialization InitializeLightingSystem;

	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  

	public delegate void SlotControlSystem(SlotScript[,] inventorySlots);
	public static event SlotControlSystem InitializeSlotControlSystem;

	public static event BaseInitialization InitializeCostume;

	public static event BaseInitialization InitializeCameraFunctions;
	public static event BaseInitialization InitializeBackgroundScroller;

	public delegate void TerrainCreation (TerrainReferenceClass maze);
	public static event TerrainCreation CreateTerrainItems;

	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeEnemies;
	
	public static event BaseInitialization InitializeNPCPanelControllers;
	public static event BaseInitialization InitializeNPCs;
	
	public static event BaseInitialization InitializePurchasePanels;
	public static event BaseInitialization InitializePurchasePanelManager;

	void Start() {

		//Note: This would be a lot easier if I could figure out a way to pass an event in as a method parameter, but all attempts have not worked.  

		//Inventory UI Initialization
		SlotScript[,] createdInventorySlots = null;
		SlotScript[,] createdHotbarSlots = null;
		SlotScript[,] totalNumberOfInventorySlots = null;

		//Initialize everything!!!

		//Create slots, and define 2D array values.  
		if (CreateInventorySlots != null) createdInventorySlots = CreateInventorySlots (); else Debug.LogError("CreateInventorySlots was null!"); // Used with PanelLayout
		if (CreateHotbarSlots != null) createdHotbarSlots = CreateHotbarSlots (); else Debug.LogError("CreateHotbarSlots was null!"); //Used with HotbarPanelLayout (Otherwise createdUISlots gets the hotbarslots return).  
		//BIG ARRAY - Note: This array assumes that the x dimension is the same for both arrays.  (Goes y then x).  
		totalNumberOfInventorySlots = new SlotScript[createdInventorySlots.GetLength (0) + createdHotbarSlots.GetLength (0),createdInventorySlots.GetLength(1)];
		//Add the inventory slots to the master array.  
		for (int i = 0; i < createdInventorySlots.GetLength(0); i++) {
			for (int j = 0; j < createdInventorySlots.GetLength(1); j++) {
				totalNumberOfInventorySlots[i, j] = createdInventorySlots[i, j];
			}
		}
		//Add the hotbar slots to the master array.  
		for (int i = 0; i < createdHotbarSlots.GetLength(0); i++) {
			for (int j = 0; j < createdHotbarSlots.GetLength(1); j++) {
				totalNumberOfInventorySlots[i + createdInventorySlots.GetLength(0), j] = createdHotbarSlots[i, j];
			}
		}
		//Initialize Slots
		if (InitializeSlots != null) InitializeSlots (); else Debug.LogError("InitializeSlots was null!"); //Used with SlotScript

		//UI stuff.  
		//Hide/Show
		if (EnableUIHideShow != null) EnableUIHideShow (); else Debug.LogError("EnableUIHideShow was null!");//Used with InventoryHideShow
		//Health Panels
		if (InitializeUIHealthController != null) InitializeUIHealthController(); else Debug.LogError("InitializeUIHealthController was null!"); //Used for UIHealthController
		if (InitializeHealthPanels != null) InitializeHealthPanels (); else Debug.LogError("InitializeHealthPanels was null!"); //Used for HealthPanelReference and PlayerHealthPanelReference.  
		//Interactable Panels
		if (InitializeInteractablePanelController != null) InitializeInteractablePanelController(); else Debug.LogError("InitializeInteractablePanelController was null!");
		if (InitializeInteractablePanels != null) InitializeInteractablePanels(); else Debug.LogError("InitializeInteractablePanels was null!");
		//Speech control
		if (InitializeUISpeechControl != null) InitializeUISpeechControl (); else Debug.LogError("InitializeUISpeechControl was null!");

		//Lay out the level
		TerrainReferenceClass initializedMaze = null;
		if (InitializeTerrain != null) initializedMaze = InitializeTerrain(); else Debug.LogError("InitializeTerrain was null!"); //Used with LevelLayout

		//Player stuff.  
		if (CreatePlayer != null) CreatePlayer(); else Debug.LogError("CreatePlayer was null!"); //Used for CreateLevelItems (Instantiating player)
		if (CreatePlayerReference != null) CreatePlayerReference (); else Debug.LogError("CreatePlayerReference was null!"); //Used for CreateLevelItems
		if (InitializeLightingSystem != null) InitializeLightingSystem (); else Debug.LogError("InitializeLightingSystem was null!"); //Used for LightingManager.  
		if (InitializeCostume != null) InitializeCostume(); else Debug.LogError("InitializeCostume was null!"); //Used for PlayerCostumeManager
		if (InitializeHotbarManager != null) InitializeHotbarManager (); else Debug.LogError("InitializeHotbarItems was null!"); //Used for initializing the HotbarManager.  
		if (InitializePlayer != null) InitializePlayer (); else Debug.LogError("InitializePlayer was null!"); //Used for initializing the HumanoidBaseReferenceClass.  

		if (InitializeSlotControlSystem != null) InitializeSlotControlSystem(totalNumberOfInventorySlots); else Debug.LogError("InitializePlayerDropSystem was null!"); //Used for DropHandler
		if (InitializeCameraFunctions != null) InitializeCameraFunctions (); else Debug.LogError("InitializeCameraFunctions was null!"); // Used for camera controller.  
		if (InitializeBackgroundScroller != null) InitializeBackgroundScroller (); else Debug.LogError("InitializeBackgroundScroller was null!"); //Initialize the BackgroundScroller class.  

		//Requires that the costume manager has been set up.  
		if (InitializeProfessionChoiceManager != null) InitializeProfessionChoiceManager(); else Debug.LogError("InitializeProfessionChoiceManager was null!!!");

		//Initialize the enemies.  
		if (CreateTerrainItems != null) CreateTerrainItems(initializedMaze); else Debug.LogError("CreateTerrainItems was null!"); //Used for instantiating the enemies and trees.  
		if (InitializeEnemyHealthControllers != null) InitializeEnemyHealthControllers (); else Debug.LogError("InitializeEnemyHealthControllers was null!"); //Used for initializing CharacterHealthController.  
		if (InitializeEnemies != null) InitializeEnemies(); else Debug.LogError("InitializeEnemies was null!"); //Used for all enemies (requires player being instantiated).  

		if (InitializeNPCPanelControllers != null) InitializeNPCPanelControllers(); else Debug.LogError("InitializeNPCPanelControllers was null!");
		if (InitializeNPCs != null) InitializeNPCs(); else Debug.LogError("InitializeNPCs was null!");

		if (InitializePurchasePanels != null) InitializePurchasePanels(); else Debug.LogError("InitializePurchasePanels was null!");
		if (InitializePurchasePanelManager != null) InitializePurchasePanelManager(); else Debug.LogError("InitializePurchasePanelManager is null!");

		//Just mention that EventManager has been completed successfully.  
		Debug.Log("Completed EventManager");

	}

}
