using UnityEngine;
using System.Collections;

public class NPCPanelController : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeNPCPanelControllers += InitializeNPCPanelController;
	}

	void OnDisable() {
		LevelEventManager.InitializeNPCPanelControllers -= InitializeNPCPanelController;
	}

	//Class properties
	Transform playerTransform;
	Sprite playerIcon;
	SpeechControl mainSpeechControl;
	private bool speechBubbleActive = false;
	private bool alreadySpeakingToPlayer = false;
	[SerializeField] float minDistanceRequiredForInteraction = 5f;
	InteractablePanelController mainInteractablePanelController;
	InteractablePanelReference interactablePanel;
	private string[] dialogueForPlayer;

	//Set references to the playerIcon, start necessary coroutines, etc.  
	void InitializeNPCPanelController() {
		playerTransform = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		playerIcon = transform.FindChild ("FlippingItem").FindChild ("Character").FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
		mainSpeechControl = CurrentLevelVariableManagement.GetLevelUIReference ().transform.FindChild ("Speech Bubble").GetComponent <SpeechControl> ();
		mainInteractablePanelController = CurrentLevelVariableManagement.GetLevelUIReference().transform.FindChild ("InteractablePanels").gameObject.GetComponent <InteractablePanelController> (); 
		StartCoroutine (CheckForAndAttemptToSpeakToPlayer());
	}

	//Loops continuously and checks each frame whether or not the player is close enough to the NPC.  If so, it checks whether an interactable panel has a reference set.
	IEnumerator CheckForAndAttemptToSpeakToPlayer() {
		while (true) {//Vector 3 distance includes PLAYER Z COORDINATE!!!! HOLY **** YES!!!!
			if (Vector2.Distance(transform.position, playerTransform.position) <= minDistanceRequiredForInteraction) {
				if (interactablePanel == null) {
					interactablePanel = mainInteractablePanelController.GetAvailableInteractablePanel ();
					if (interactablePanel != null)
						interactablePanel.InitializePanel (playerIcon, "Press X to Interact");
				} 

				if (Input.GetKeyDown (KeyCode.X) && ! alreadySpeakingToPlayer && interactablePanel != null && dialogueForPlayer.Length > 0) {
					GetComponent <NPCBaseScript> ().StopWalkingAround();
					GetComponent <NPCBaseScript> ().FlipToFacePlayer();
					GetComponent <NPCBaseScript> ().NPCActionBeforeSpeaking();
					//has to be a different coroutine so that it waits for it to finish.
					StartCoroutine (SpeakToPlayer (dialogueForPlayer, GetComponent <NPCBaseScript> ().npcName));
					alreadySpeakingToPlayer = true;
				}

			} else if (Vector2.Distance (transform.position, playerTransform.position) > minDistanceRequiredForInteraction) {
				if (interactablePanel != null) {
					interactablePanel.Clear ();
					interactablePanel = null;
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
	private IEnumerator SpeakToPlayer(string[] toSay, string name) {
		dialogueForPlayer = toSay;
		speechBubbleActive = true;
		yield return StartCoroutine(mainSpeechControl.SaySomething (playerIcon, name, toSay));
		//Do whatever action should be done for the NPC that is attached.  
		GetComponent <NPCBaseScript> ().NPCActionAfterSpeaking ();
	}

	//Makes a call to SpeechControl on the UI with the argument that determine what to say and the icon that says it.  
	public virtual void ClearSpeechBubble() {
		mainSpeechControl.DeActivateSpeechBubble ();
		speechBubbleActive = false;
	}

	//Used for setting future dialogue to speak to the player.  
	public void SetCharacterDialogue(string[] customDialogue) {
		dialogueForPlayer = customDialogue;
		//Otherwise, start the dialogue.  
		if (alreadySpeakingToPlayer) {
			ClearSpeechBubble();
			SpeakToPlayer(customDialogue, GetComponent <NPCBaseScript> ().npcName);
		}
	}

}
