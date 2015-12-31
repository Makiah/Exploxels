using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfessionSpeechManager : SpeechControl {
	//Initialization
	protected override void OnEnable() {
		ProfessionEventManager.InitializeProfessionSpeechManager += InitializeSpeechControl;
	}

	protected override void OnDisable() {
		ProfessionEventManager.InitializeProfessionSpeechManager -= InitializeSpeechControl;
	}

	//Set speech dialogue: does not have assigner.  
	public IEnumerator SetSpeechDialogue(Sprite icon, string[] stuffToSay) {
		gameObject.SetActive (true);
		yield return StartCoroutine(SaySomething (icon, "Bertie", stuffToSay));
		gameObject.SetActive (false);
	}

}
