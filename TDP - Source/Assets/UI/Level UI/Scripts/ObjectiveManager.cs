using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

	//Initialization
	void OnEnable() {
		LevelEventManager.InitializeObjectiveManager += InitializeObjectiveReferences;
	}

	void OnDisable() {
		LevelEventManager.InitializeObjectiveManager -= InitializeObjectiveReferences;
	}

	//Define what an objective is in the scene.  
	class ObjectiveReference {
		public readonly Image icon;
		public readonly Image panel;
		public readonly Text description;
		public bool completed = false;
		//Constructor
		public ObjectiveReference(Transform transformToUse) {
			icon = transformToUse.FindChild("Icon").GetComponent <Image> ();
			panel = transformToUse.GetComponent <Image> ();
			description = transformToUse.FindChild("Description Panel").FindChild("Description").GetComponent <Text> ();
		}

		public void Completed() {
			completed = true;
			panel.color = Color.green;
		}

		public void Reset() {
			icon.sprite = null;
			panel.color = Color.red;
			description.text = "";
			completed = false;
		}
	}

	//Define objectives.  
	ObjectiveReference[] objectives;
	Button continueToNextLevel;

	//Initialize Objective References
	void InitializeObjectiveReferences() {
		int totalNumberOfObjectives = 2;
		objectives = new ObjectiveReference[totalNumberOfObjectives];
		//Set objective references
		for (int i = 0; i < totalNumberOfObjectives; i++) {
			objectives[i] = new ObjectiveReference(transform.FindChild("Objective " + (i + 1)));
			objectives [i].Reset ();
		}
		//Set the button reference.
		continueToNextLevel = transform.FindChild ("Level Continue").FindChild("Button").GetComponent <Button> ();
		continueToNextLevel.interactable = false;

		//Set initial values.  
		switch (GameData.GetLevel()) {
		case 0:
			objectives [0].icon.sprite = ResourceDatabase.GetItemByParameter ("Subsidiary Reactor Core").itemIcon;
			objectives [0].description.text = "Subsidiary Reactor Core";

			objectives [1].icon.sprite = ResourceDatabase.GetItemByParameter ("Wooden Pickaxe").itemIcon;
			objectives [1].description.text = "Pickaxe";
			break;
		default:
			Debug.LogError("The current level has not been added in ObjectiveManager!");
			break;
		}
	}

	//Sort of a medium for AllObjectivesComplete.  
	void CheckWhetherAllObjectivesAreComplete() {
		if (AllObjectivesComplete()) 
			continueToNextLevel.interactable = true;
	}

	//Called by HunterNPCBehaviour to see whether fire has been created.  
	public bool CheckStateOfObjective(int objectiveIndex) {
		if (objectives.Length < objectiveIndex)
			return objectives [objectiveIndex].completed;
		else
			return false;
	}

	bool AllObjectivesComplete() {
		//Go through all objectives.  
		for (int i = 0; i < objectives.Length; i++) {
			if (objectives[i].completed == false) {
				return false;
			}
		}
		//When no objectives are false.  
		return true;
	}

	//When the next level is selected.  
	public void OnContinueToNextLevelButtonPressed() {
		Debug.Log ("Would load next level");
		CurrentLevelVariableManagement.GetMainGameControl ().OnCurrentLevelCompleted ();
	}

	//When an objective is achieved
	public void OnObjectiveHasBeenCompleted(int completedObjective) {
		if (objectives [completedObjective - 1].completed == false) {
			Debug.Log ("Objective " + completedObjective + " has been completed");
			objectives [completedObjective - 1].Completed ();
			CheckWhetherAllObjectivesAreComplete();
		} else {
			Debug.LogError("Objective " + completedObjective + " has already been completed!");
		}
	}

	//When a new item is added.  
	public void OnNewItemAddedToPlayerInventory() {
		switch (GameData.GetLevel()) {
		case 0:
			//Check to make sure the objective has not already been completed
			//Wooden Pickaxe Objective
			if (objectives [0].completed == false) {
				//Check whether the player has the hatchet.  
				if (CurrentLevelVariableManagement.GetMainInventoryReference().GetComponent <InventoryFunctions> ().CheckForCertainInventoryItem (new ResourceReferenceWithStack (ResourceDatabase.GetItemByParameter ("Wooden Pickaxe"), 1)) != null) {
					OnObjectiveHasBeenCompleted(1);
				}
			}
//	
//			//Wooden Sword Objective
//			if (objectives [3].completed == false) {
//				if (ModifiesSlotContent.DetermineWhetherPlayerHasCertainInventoryItem(new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wooden Pickaxe"), 1)) != null) {
//					OnObjectiveHasBeenCompleted(4);
//				}
//			}
			break;
		default: 
			Debug.LogError("Objective Manager does not have a definition for this level!");
			break;
		}
	}

	//When a tree is chopped.  Possibly consider counting this as a stat: i.e. trees chopped during game.  
	public void OnTreeChopped() {
		switch (GameData.GetLevel()) {
		case 0:
//			if (objectives [1].completed == false) {
//				OnObjectiveHasBeenCompleted (2);
//			}
			break;
		default: 
			Debug.LogError("Objective Manager does not have a definition for this level!");
			break;
		}
	}

	//When some amount of money is earned or taken.  
	public void OnMoneyModified(int amount) {
		switch (GameData.GetLevel()) {
		case 0:
//			if (amount > 0) {
//				if (objectives[2].completed == false) {
//					OnObjectiveHasBeenCompleted(3);
//				}
//			}
			break;
		default: 
			Debug.LogError("Objective Manager does not have a definition for this level!");
			break;
		}

	}

	public void OnFireBuilt() {
		switch (GameData.GetLevel()) {
		case 0: 
//			if (objectives[4].completed == false) 
//				OnObjectiveHasBeenCompleted(5);
			break;
		}
	}

}
