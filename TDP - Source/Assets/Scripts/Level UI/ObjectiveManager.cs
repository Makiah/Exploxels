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
		public Toggle objectiveToggle;
		public Text name;
		public ObjectiveReference(Transform location) {
			objectiveToggle = location.GetComponent <Toggle> ();
			name = location.FindChild("Label").GetComponent <Text> ();
		}
	}

	//Define objectives.  
	ObjectiveReference[] objectives;

	Button continueToNextLevel;

	//Initialize Objective References
	void InitializeObjectiveReferences() {
		int totalNumberOfObjectives = 5;
		objectives = new ObjectiveReference[totalNumberOfObjectives];
		//Set objective references
		for (int i = 0; i < totalNumberOfObjectives; i++) {
			objectives[i] = new ObjectiveReference(transform.FindChild("Objective " + (i + 1)));
			objectives[i].objectiveToggle.isOn = false;
		}
		//Set the button reference.
		continueToNextLevel = transform.FindChild ("Level Continue").FindChild("Yes").GetComponent <Button> ();
		continueToNextLevel.interactable = false;

		SetObjectiveText ();
	}

	//Initial objective text
	void SetObjectiveText() {
		switch (CurrentLevelVariableManagement.GetMainGameData().currentLevel) {
		case 0:
			objectives [0].name.text = "Get a hatchet.";
			objectives [1].name.text = "Chop a tree";
			objectives [2].name.text = "Earn money.";
			objectives [3].name.text = "Get a pickaxe.";
			objectives [4].name.text = "Build a fire.";
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
		return objectives [objectiveIndex - 1].objectiveToggle.isOn;
	}

	bool AllObjectivesComplete() {
		for (int i = 0; i < objectives.Length; i++) {
			if (objectives[i].objectiveToggle.isOn == false) {
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
		if (objectives [completedObjective - 1].objectiveToggle.isOn == false) {
			Debug.Log ("Objective " + completedObjective + " has been completed");
			objectives [completedObjective - 1].objectiveToggle.isOn = true;
			CheckWhetherAllObjectivesAreComplete();
		} else {
			Debug.LogError("Objective has already been completed");
		}
	}

	//When a new item is added.  
	public void OnNewItemAddedToPlayerInventory() {
		switch (CurrentLevelVariableManagement.GetMainGameData().currentLevel) {
		case 0:
			//Check to make sure the objective has not already been completed
			//Wooden Hatchet Objective
			if (objectives [0].objectiveToggle.isOn == false) {
				//Check whether the player has the hatchet.  
				if (ModifiesSlotContent.DetermineWhetherPlayerHasCertainInventoryItem (new UISlotContentReference (ResourceDatabase.GetItemByParameter ("Wooden Hatchet"), 1)) != null) {
					OnObjectiveHasBeenCompleted(1);
				}
			}
	
			//Wooden Sword Objective
			if (objectives [3].objectiveToggle.isOn == false) {
				if (ModifiesSlotContent.DetermineWhetherPlayerHasCertainInventoryItem(new UISlotContentReference(ResourceDatabase.GetItemByParameter ("Wooden Pickaxe"), 1)) != null) {
					OnObjectiveHasBeenCompleted(4);
				}
			}
			break;
		default: 
			Debug.LogError("Objective Manager does not have a definition for this level!");
			break;
		}
	}

	//When a tree is chopped.  Possibly consider counting this as a stat: i.e. trees chopped during game.  
	public void OnTreeChopped() {
		switch (CurrentLevelVariableManagement.GetMainGameData ().currentLevel) {
		case 0:
			if (objectives [1].objectiveToggle.isOn == false) {
				OnObjectiveHasBeenCompleted (2);
			}
			break;
		default: 
			Debug.LogError("Objective Manager does not have a definition for this level!");
			break;
		}
	}

	//When some amount of money is earned or taken.  
	public void OnMoneyModified(int amount) {
		switch (CurrentLevelVariableManagement.GetMainGameData ().currentLevel) {
		case 0:
			if (amount > 0) {
				if (objectives[2].objectiveToggle.isOn == false) {
					OnObjectiveHasBeenCompleted(3);
				}
			}
			break;
		default: 
			Debug.LogError("Objective Manager does not have a definition for this level!");
			break;
		}

	}

	public void OnFireBuilt() {
		switch (CurrentLevelVariableManagement.GetMainGameData ().currentLevel) {
		case 0: 
			if (objectives[4].objectiveToggle.isOn == false) 
				OnObjectiveHasBeenCompleted(5);
			break;
		}
	}

}
