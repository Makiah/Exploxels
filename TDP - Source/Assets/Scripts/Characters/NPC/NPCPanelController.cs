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
	bool alreadySpeakingToPlayer = false;
	[SerializeField] float minDistanceRequiredForInteraction = 5f;
	InteractablePanelController mainInteractablePanelController;
	InteractablePanelReference interactablePanel;

	string[] dialogueForPlayer;

	//Set references to the playerIcon, start necessary coroutines, etc.  
	void InitializeNPCPanelController() {
		playerTransform = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		playerIcon = transform.FindChild ("FlippingItem").FindChild ("Character").FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
		mainSpeechControl = CurrentLevelVariableManagement.GetLevelUIReference ().transform.FindChild ("Speech Bubble").GetComponent <SpeechControl> ();
		mainInteractablePanelController = CurrentLevelVariableManagement.GetLevelUIReference().transform.FindChild ("InteractablePanels").gameObject.GetComponent <InteractablePanelController> (); 
		StartCoroutine ("CheckForAndAttemptToSpeakToPlayer");
	}

	//Loops continuously and checks each frame whether or not the player is close enough to the NPC.  If so, it checks whether an interactable panel has a reference set.
	IEnumerator CheckForAndAttemptToSpeakToPlayer() {
		while (true) {//Vector 3 distance includes PLAYER Z COORDINATE!!!! HOLY **** YES!!!!
			if (Vector2.Distance(transform.position, playerTransform.position) <= minDistanceRequiredForInteraction) {
				if (interactablePanel == null)
					OnActivateInteractablePanel();

				if (Input.GetKeyDown (KeyCode.X) && ! alreadySpeakingToPlayer && interactablePanel != null) {
					if (dialogueForPlayer.Length != 0) {
						GetComponent <NPCBaseScript> ().StopWalkingAround();
						GetComponent <NPCBaseScript> ().FlipToFacePlayer();
						GetComponent <NPCBaseScript> ().NPCActionBeforeSpeaking();
						SpeakToPlayer(dialogueForPlayer, GetComponent <NPCBaseScript> ().npcName);
						alreadySpeakingToPlayer = true;
					}
				}
			} else if (Vector2.Distance (transform.position, playerTransform.position) > minDistanceRequiredForInteraction) {
				if (interactablePanel != null) {
					ClearInteractablePanel();
				}
				if (speechBubbleActive) {
					ClearSpeechBubble();
					GetComponent <NPCBaseScript> ().ResumeWalkingAround();
				}
				if (alreadySpeakingToPlayer) {
					alreadySpeakingToPlayer = false;
				}
			}

			yield return null;
		}
	}

	//Makes a call to SpeechControl on the UI with the arguments that determine what to say and the icon that is saying it.
	public virtual void SpeakToPlayer(string[] toSay, string name) {
		mainSpeechControl.SaySomething (playerIcon, name, toSay, true, this);
		speechBubbleActive = true;
	}

	public void OnCompletedSpeakingToPlayer() {
		GetComponent <NPCBaseScript> ().NPCActionAfterSpeaking();
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

	public void SetCharacterDialogue(string[] customDialogue) {
		if (alreadySpeakingToPlayer) {
			ClearSpeechBubble();
			SpeakToPlayer(customDialogue, GetComponent <NPCBaseScript> ().npcName);
		}
		dialogueForPlayer = customDialogue;
	}

}
