using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechControl : MonoBehaviour {

	protected virtual void OnEnable() {
		LevelEventManager.InitializeUISpeechControl += InitializeSpeechControl;
	}

	protected virtual void OnDisable() {
		LevelEventManager.InitializeUISpeechControl -= InitializeSpeechControl;
	}
	
	Text textSpeechBox;
	Text speakerName;
	Image playerIcon;
	bool speechBubbleActive = false;
	bool coroutineActive;

	protected virtual void InitializeSpeechControl() {
		textSpeechBox = transform.FindChild ("Speech").GetComponent <Text> ();
		speakerName = transform.FindChild ("NamePanel").FindChild ("SpeakerName").GetComponent <Text> ();
		playerIcon = transform.FindChild ("PlayerIcon").GetComponent <Image> ();
		gameObject.SetActive (false);
	}

	public void SaySomething(Sprite headIcon, string speaker, string[] phrasesToSay) {
		gameObject.SetActive (true);
		playerIcon.sprite = headIcon;
		speakerName.text = speaker;
		textSpeechBox.text = phrasesToSay [0];
		speechBubbleActive = true;
		coroutineActive = true;
		StartCoroutine ("BasicSpeechScrolling", phrasesToSay);
	}

	public void SaySomething(Sprite headIcon, string speaker, string[] phrasesToSay, bool speakInScrollingText) {
		gameObject.SetActive (true);
		playerIcon.sprite = headIcon;
		speakerName.text = speaker;
		speechBubbleActive = true;
		coroutineActive = true;
		if (speakInScrollingText)
			StartCoroutine ("SpeakInScrollingText", phrasesToSay);
		else {
			textSpeechBox.text = phrasesToSay [0];
			StartCoroutine ("BasicSpeechScrolling", phrasesToSay);
		}
	}
	
	IEnumerator BasicSpeechScrolling(string[] phrasesToSay) {
		int phraseIndex = 0;
		while (true) {
			//Scrolling down through the list.  
			if (Input.GetKeyDown (KeyCode.Q) && phraseIndex != 0) {
				phraseIndex--;
				textSpeechBox.text = phrasesToSay[phraseIndex];
			} else if (Input.GetKeyDown(KeyCode.W) && phraseIndex != phrasesToSay.Length - 1) {
				phraseIndex++;
				textSpeechBox.text = phrasesToSay[phraseIndex];
			}

			yield return null;
		}
	}

	//Speak in scrolling text.  
	//For some reason, this coroutine does not work on child classes unless set as protected.  Weird.  
	protected IEnumerator SpeakInScrollingText(string[] stuffToSay) {
		//Otherwise the coroutine uses the x press used to initialize the coroutine itself.  
		yield return null;
		int currentPhraseIndex = 0;
		bool completedDialogue = false;
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
			yield return new WaitForSeconds(.05f);
		}
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
