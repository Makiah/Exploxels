using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechControl : MonoBehaviour {

	/********************** INITIALIZATION **********************/
	protected virtual void OnEnable() {
		LevelEventManager.InitializeUISpeechControl += InitializeSpeechControl;
	}

	protected virtual void OnDisable() {
		LevelEventManager.InitializeUISpeechControl -= InitializeSpeechControl;
	}

	/********************** SCRIPT **********************/

	//Required components
	Text textSpeechBox;
	Text speakerName;
	Image playerIcon;
	bool speechBubbleActive = false;
	bool coroutineActive;

	//Set when the NPCPanelController
	NPCPanelController currentlyAssignedTo;

	//Initial initialization.  Overriden by ProfessionSpeechManager.  
	protected void InitializeSpeechControl() {
		textSpeechBox = transform.FindChild ("Speech").GetComponent <Text> ();
		speakerName = transform.FindChild ("NamePanel").FindChild ("SpeakerName").GetComponent <Text> ();
		playerIcon = transform.FindChild ("PlayerIcon").GetComponent <Image> ();
		gameObject.SetActive (false);
	}

	//On dialogue completed.  
	protected virtual void CompletedSpeakingToPlayer() {
		if (currentlyAssignedTo != null) {
			currentlyAssignedTo.OnCompletedSpeakingToPlayer ();
		} else {
			Debug.Log("Currently not assigned to NPCPanelController!");
		}
	}

	//Called when something should be said.  
	public void SaySomething(Sprite headIcon, string speaker, string[] phrasesToSay, NPCPanelController assignee) {
		playerIcon.sprite = headIcon;
		speakerName.text = speaker;
		speechBubbleActive = true;
		coroutineActive = true;
		currentlyAssignedTo = assignee;
		gameObject.SetActive (true);
		StartCoroutine (SpeakInScrollingText(phrasesToSay));
	}

	//Speak in scrolling text.  
	//For some reason, this coroutine does not work on child classes unless set as protected.  Weird.  

	//Set by both coroutines.  
	bool completedDialogue = false;

	protected IEnumerator SpeakInScrollingText(string[] stuffToSay) {
		//Otherwise the coroutine uses the x press used to initialize the coroutine itself.  
		yield return null;
		int currentPhraseIndex = 0;
		StartCoroutine ("ScrollText", stuffToSay [currentPhraseIndex]);
		while (true) {
			if (Input.GetKeyDown(KeyCode.X)) {
				//Update the scroll text coroutine.  
				if (completedDialogue) {
					if (currentPhraseIndex != stuffToSay.Length - 1) {		
						//Increment the scroll coroutine's phrase.  
						currentPhraseIndex++;
						StartCoroutine("ScrollText", stuffToSay[currentPhraseIndex]);
						completedDialogue = false;
					} else {
						//Exit the coroutine.  
						CompletedSpeakingToPlayer();
						yield break;
				 	}
				} else {
					//When the x key is pressed, and the text is not finished scrolling.  
					StopCoroutine("ScrollText");
					textSpeechBox.text = stuffToSay[currentPhraseIndex];
					completedDialogue = true;
				}
			}

			yield return null;
		}
	}

	//Scrolls the actual text, given a string value.  
	protected IEnumerator ScrollText(string toSay) {
		for (int i = 2; i <= toSay.Length; i++) {
			textSpeechBox.text = toSay.Substring(0, i);
			yield return new WaitForSeconds(.02f);
		}
		completedDialogue = true;
	}

	public void DeActivateSpeechBubble() {
		if (speechBubbleActive) {
			if (coroutineActive) 
				StopCoroutine("ListenForSpeechScrolling");
			textSpeechBox.text = "";
			playerIcon.sprite = null;
			speakerName.text = "";
			gameObject.SetActive(false);
			speechBubbleActive = false;
		}
	}

}
