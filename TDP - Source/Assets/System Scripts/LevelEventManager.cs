
/*
 * Author: Makiah Bennett
 * Last edited: 18 November 2015
 * 
 * This script manages the entire path the game takes.  Each event is static, so it only belongs to this class.  Each OnEnable() and OnDsiable() 
 * assigns and de-assigns events to this class.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelEventManager : MonoBehaviour {

	//The loading bar.  
	[SerializeField] private GameObject loadingBar = null;

	//Pretty much contains every event that does not require a parameter or a return type.  
	public delegate void BaseInitialization();

	public static event BaseInitialization CreateInventorySlots;
	public static event BaseInitialization CreateHotbarSlots;

	public static event BaseInitialization InitializeSlots;
	public static event BaseInitialization InitializeHotbarManager;

	public static event BaseInitialization EnableUIHideShow;

	public static event BaseInitialization InitializeUIHealthController;
	public static event BaseInitialization InitializeHealthPanels;

	public static event BaseInitialization InitializeInteractablePanelController;
	public static event BaseInitialization InitializeInteractablePanels;

	public static event BaseInitialization InitializeUISpeechControl;

	public static event BaseInitialization InitializeObjectiveManager;

	public delegate TerrainReferenceClass TerrainInitialization();
	public static event TerrainInitialization InitializeTerrain;

	public static event BaseInitialization CreatePlayer;

	public static event BaseInitialization InitializePlayer; //Use for initializing CostumeManager and PlayerAction, as well as the PlayerHealthController.  

	public static event BaseInitialization InitializeCostume;

	public static event BaseInitialization InitializeCameraFunctions;
	public static event BaseInitialization InitializeBackgroundManager;
	public static event BaseInitialization InitializeTimeIndicator;

	public delegate void TerrainCreation (TerrainReferenceClass maze);
	public static event TerrainCreation CreateTerrainItems;
	
	public static event BaseInitialization InitializeSystemWideParticleEffect;

	public static event BaseInitialization InitializeEnemyHealthControllers;
	public static event BaseInitialization InitializeEnemies;
	
	public static event BaseInitialization InitializeNPCPanelControllers;
	public static event BaseInitialization InitializeNPCs;
	
	public static event BaseInitialization InitializePurchasePanels;
	public static event BaseInitialization InitializePurchasePanelManager;

	public static event BaseInitialization InitializeDoors;

	public static event BaseInitialization SetInactiveObjects;

	//Used during initialization.  
	LoadingProgressBar createdLoadingBar;

	//Pretty much the only Start() method in the whole program.  
	void Start() {
		//Instantiate the loading bar.  
		GameObject instantiatedLoadingBar = (GameObject) (Instantiate(loadingBar, Vector3.zero, Quaternion.identity));
		createdLoadingBar = instantiatedLoadingBar.GetComponent <LoadingProgressBar> ();
		//A coroutine has to be used, so that the program does not continue before the level has been loaded.  
		StartCoroutine (LoadEverything());
	}

	IEnumerator LoadEverything() {
		createdLoadingBar.InitializeNewAction (.1f, "Loading Main UI");
		AsyncOperation loadingOperation = SceneManager.LoadSceneAsync ("MainGameUI", LoadSceneMode.Additive);
		while (!loadingOperation.isDone) {
			yield return null;
		}

		Debug.Log ("Loading Main Game UI is complete.");

		//Update the loading bar.  
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction (.15f, "Loading Inventory");

		//Initialize everything!!!

		//Create slots, and define 2D array values.  
		if (CreateInventorySlots != null) CreateInventorySlots (); else Debug.LogError("CreateInventorySlots was null!"); // Used with PanelLayout
		if (CreateHotbarSlots != null) CreateHotbarSlots (); else Debug.LogError("CreateHotbarSlots was null!"); //Used with HotbarPanelLayout (Otherwise createdUISlots gets the hotbarslots return).  

		//Initialize Slots
		if (InitializeSlots != null) InitializeSlots (); else Debug.LogError("InitializeSlots was null!"); //Used with SlotScript
		Debug.Log("Initialized slots");

		//UI stuff.  
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction (.2f, "Loading UI Stuff");

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
		//Objective Manager
		if (InitializeObjectiveManager != null) InitializeObjectiveManager(); else Debug.LogError("InitializeObjectiveManager was null!"); //Used for ObjectiveManager

		//Lay out the level
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction(.25f, "Creating Terrain");

		TerrainReferenceClass initializedMaze = null;
		if (InitializeTerrain != null) initializedMaze = InitializeTerrain(); else Debug.LogError("InitializeTerrain was null!"); //Used with LevelLayout

		//Player stuff.  
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction(.5f, "Initializing Player");

		if (CreatePlayer != null) CreatePlayer(); else Debug.LogError("CreatePlayer was null!"); //Used for CreateLevelItems (Instantiating player)
		//Has to be done after the player is instantiated.  
		CurrentLevelVariableManagement.SetLevelReferences ();

		if (InitializeHotbarManager != null) InitializeHotbarManager (); else Debug.LogError("InitializeHotbarItems was null!"); //Used for initializing the HotbarManager.  

		if (InitializeCostume != null) InitializeCostume(); else Debug.LogError("InitializeCostume was null!"); //Used for PlayerCostumeManager
		if (InitializeBackgroundManager != null) InitializeBackgroundManager (); else Debug.LogError("InitializeBackgroundScroller was null!"); //Initialize the BackgroundScroller class.  

		if (InitializePlayer != null) InitializePlayer (); else Debug.LogError("InitializePlayer was null!"); //Used for initializing the HumanoidBaseReferenceClass.  

		if (InitializeCameraFunctions != null) InitializeCameraFunctions (); else Debug.LogError("InitializeCameraFunctions was null!"); // Used for camera controller.  
		if (InitializeTimeIndicator != null) InitializeTimeIndicator(); else Debug.LogError("InitializeTimeIndicator was null!!"); //Used for TimeIndicator.  

		//Initialize the enemies.  
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction(.75f, "Initializing Enemies");

		if (CreateTerrainItems != null) CreateTerrainItems(initializedMaze); else Debug.LogError("CreateTerrainItems was null!"); //Used for instantiating the enemies and trees.  
		if (InitializeEnemyHealthControllers != null) InitializeEnemyHealthControllers (); else Debug.LogError("InitializeEnemyHealthControllers was null!"); //Used for initializing CharacterHealthController.  
		if (InitializeEnemies != null) InitializeEnemies(); else Debug.LogError("InitializeEnemies was null!"); //Used for all enemies (requires player being instantiated).  

		//NPCs
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction(.85f, "Initializing NPCs");

		if (InitializeNPCPanelControllers != null) InitializeNPCPanelControllers(); else Debug.LogError("InitializeNPCPanelControllers was null!");
		if (InitializeNPCs != null) InitializeNPCs(); else Debug.LogError("InitializeNPCs was null!");

		//Particle effect (world)
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction(.9f, "Initializing Particle Effects");

		if (InitializeSystemWideParticleEffect != null) InitializeSystemWideParticleEffect(); else Debug.LogError("InitializeSystemWideParticleEffect was null!");

		//Purchase panels
		if (InitializePurchasePanels != null) InitializePurchasePanels(); else Debug.LogError("InitializePurchasePanels was null!");
		if (InitializePurchasePanelManager != null) InitializePurchasePanelManager(); else Debug.LogError("InitializePurchasePanelManager is null!");

		if (SetInactiveObjects != null) SetInactiveObjects (); else Debug.LogError("HideInventories is null!");
		if (InitializeDoors != null) InitializeDoors (); else Debug.LogError("InitializeDoors was null!");

		//Just mention that EventManager has been completed successfully.  
		yield return new WaitForSeconds (.1f);
		createdLoadingBar.InitializeNewAction(1, "Completed successfully!");
		yield return new WaitForSeconds (1.5f);
		Debug.Log("Completed EventManager");

		//Delete the loading bar
		Destroy(createdLoadingBar.gameObject);
	}

}
