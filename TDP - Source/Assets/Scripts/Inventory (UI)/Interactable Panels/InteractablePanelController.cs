using UnityEngine;
using System.Collections;

public class InteractablePanelController : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeInteractablePanelController += InitializeInteractablePanelController;
	}

	void OnDisable() {
		LevelEventManager.InitializeInteractablePanelController -= InitializeInteractablePanelController;
	}

	InteractablePanelReference interactablePanel1;

	//Reference the Interactable Panel References.  
	void InitializeInteractablePanelController() {
		interactablePanel1 = transform.FindChild ("InteractablePanel1").GetComponent <InteractablePanelReference> ();
	}

	public InteractablePanelReference GetAvailableInteractablePanel() {
		if (interactablePanel1.IsEmpty ()) {
			return interactablePanel1;
		} else {
			return null;
		}
	}

}
