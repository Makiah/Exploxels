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
	Image playerIcon;
	bool speechBubbleActive = false;
	bool coroutineActive;

	protected virtual void InitializeSpeechControl() {
		textSpeechBox = transform.FindChild ("Speech").GetComponent <Text> ();
		playerIcon = transform.FindChild ("PlayerIcon").GetComponent <Image> ();
		gameObject.SetActive (false);
	}

	public void SaySomething(Sprite headIcon, string toSay) {
		gameObject.SetActive (true);
		playerIcon.sprite = headIcon;
		textSpeechBox.text = toSay;
		speechBubbleActive = true;
	}

	public void SaySomething(Sprite headIcon, string[] phrasesToSay) {
		gameObject.SetActive (true);
		playerIcon.sprite = headIcon;
		textSpeechBox.text = phrasesToSay [0];
		speechBubbleActive = true;
		coroutineActive = true;
		StartCoroutine ("BasicSpeechScrolling", phrasesToSay);
	}

	public void SaySomething(Sprite headIcon, string[] phrasesToSay, bool speakInScrollingText) {
		gameObject.SetActive (true);
		playerIcon.sprite = headIcon;
		textSpeechBox.text = phrasesToSay [0];
		speechBubbleActive = true;
		coroutineActive = true;
		if (speakInScrollingText)
			StartCoroutine ("SpeakInScrollingText", phrasesToSay);
		else
			StartCoroutine ("BasicSpeechScrolling", phrasesToSay);
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
	protected IEnumerator SpeakInScrollingText(string[] stuffToSay) {
		int phraseIndex = 0;
		while (true) {
			Debug.Log("Starting again");
			int i;
			for (i = 2; i <= stuffToSay[phraseIndex].Length + 1; i++) {
				//Changing the thing that should be said.  	
				if (Input.GetKeyDown (KeyCode.Q) && phraseIndex != 0) {
					phraseIndex--;
					i = 2;
				} else if (Input.GetKeyDown(KeyCode.W) && phraseIndex != stuffToSay.Length - 1) {
					phraseIndex++;
					i = 2;
				}

				if (i != stuffToSay[phraseIndex].Length + 1)
					textSpeechBox.text = stuffToSay[phraseIndex].Substring(0, i);
				
				yield return new WaitForSeconds(.075f);
			}

			if (phraseIndex != stuffToSay.Length - 1) {
				phraseIndex++;
				i = 2;
			} else {
				//Exit from the SpeakInScrollingText Coroutine.  
				yield break;
			}

		}
	}

	public void DeActivateSpeechBubble() {
		if (speechBubbleActive) {
			if (coroutineActive) 
				StopCoroutine("ListenForSpeechScrolling");
			gameObject.SetActive(false);
			speechBubbleActive = false;
		}
	}

}
