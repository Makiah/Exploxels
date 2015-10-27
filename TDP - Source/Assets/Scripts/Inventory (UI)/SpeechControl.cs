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

	public void DeActivateSpeechBubble() {
		if (speechBubbleActive) {
			gameObject.SetActive(false);
			speechBubbleActive = false;
		}
	}

}
