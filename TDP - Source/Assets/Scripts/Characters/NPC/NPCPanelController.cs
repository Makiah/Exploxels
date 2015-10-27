using UnityEngine;
using System.Collections;

public class NPCPanelController : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeNPCPanelControllers += InitializeNPCPanelController;
	}

	void OnDisable() {
		LevelEventManager.InitializeNPCPanelControllers -= InitializeNPCPanelController;
	}

	Transform playerTransform;
	Sprite playerIcon;
	SpeechControl mainSpeechControl;
	bool speechBubbleActive = false;
	[SerializeField] float minDistanceRequiredForInteraction = 5f;
	InteractablePanelController mainInteractablePanelController;
	InteractablePanelReference interactablePanel;

	void InitializeNPCPanelController() {
		playerTransform = VariableManagement.GetPlayerReference ().transform;
		playerIcon = transform.FindChild ("FlippingItem").FindChild ("Character").FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
		mainSpeechControl = VariableManagement.GetLevelUIReference ().transform.FindChild ("Speech Bubble").GetComponent <SpeechControl> ();
		mainInteractablePanelController = VariableManagement.GetLevelUIReference().transform.FindChild ("InteractablePanels").gameObject.GetComponent <InteractablePanelController> (); 
		Debug.Log (mainInteractablePanelController.name);
		StartCoroutine ("CheckForAndAttemptToSpeakToPlayer");
	}

	//Loops continuously and checks each frame whether or not the player is close enough to the NPC.  If so, it checks whether an interactable panel has a reference set.
	IEnumerator CheckForAndAttemptToSpeakToPlayer() {
		while (true) {
			if (Vector3.Distance(transform.position, playerTransform.position) <= minDistanceRequiredForInteraction) {
				if (interactablePanel == null) 
					OnActivateInteractablePanel();
				if (Input.GetKeyDown (KeyCode.X)) {
					SpeakToPlayer("Hey there!");
				}
			} else if (Vector3.Distance (transform.position, playerTransform.position) > minDistanceRequiredForInteraction) {
				if (interactablePanel != null)
					ClearInteractablePanel();
				if (speechBubbleActive) {
					ClearSpeechBubble();
				}
			}

			yield return null;
		}
	}

	//Makes a call to SpeechControl on the UI with the arguments that determine what to say and the icon that is saying it.  
	public virtual void SpeakToPlayer(string toSay) {
		mainSpeechControl.SaySomething (playerIcon, toSay);
		speechBubbleActive = true;
	}

	//Makes a call to SpeechControl on the UI with the argument that determine what to say and the icon that says it.  
	public virtual void ClearSpeechBubble() {
		mainSpeechControl.DeActivateSpeechBubble ();
		speechBubbleActive = false;
	}
	
	// Called when player enters radius of the character health controller.  
	void OnActivateInteractablePanel() {
		interactablePanel = mainInteractablePanelController.GetAvailableInteractablePanel ();
		if (interactablePanel != null)
			interactablePanel.InitializePanel (playerIcon, "Press X to Interact");
	}

	//Called when the object is de-activated, or on death.  
	void ClearInteractablePanel() {
		if (interactablePanel != null) {
			interactablePanel.Clear ();
			interactablePanel = null;
		}
	}

}
