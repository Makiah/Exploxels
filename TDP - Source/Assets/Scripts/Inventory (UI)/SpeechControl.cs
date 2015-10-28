using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechControl : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeUISpeechControl += InitializeSpeechControl;
	}

	void OnDisable() {
		LevelEventManager.InitializeUISpeechControl -= InitializeSpeechControl;
	}
	
	Text textSpeechBox;
	Image playerIcon;
	bool speechBubbleActive = false;
	bool coroutineActive;

	void InitializeSpeechControl() {
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
		StartCoroutine ("ListenForSpeechScrolling", phrasesToSay);
	}
	
	IEnumerator ListenForSpeechScrolling(string[] phrasesToSay) {
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

	public void DeActivateSpeechBubble() {
		if (speechBubbleActive) {
			if (coroutineActive) 
				StopCoroutine("ListenForSpeechScrolling");
			gameObject.SetActive(false);
			speechBubbleActive = false;
		}
	}

}
