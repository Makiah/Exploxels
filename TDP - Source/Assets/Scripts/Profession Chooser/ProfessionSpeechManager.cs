using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfessionSpeechManager : SpeechControl {

	protected override void OnEnable() {
		ProfessionEventManager.InitializeProfessionSpeechManager += InitializeSpeechControl;
	}

	protected override void OnDisable() {
		ProfessionEventManager.InitializeProfessionSpeechManager -= InitializeSpeechControl;
	}

	public void SetSpeechDialogue(string[] stuffToSay) {
		SaySomething (ResourceDatabase.GetRaceByParameter ("Gatherer").male.head, "Guide", stuffToSay, true, null);
	}

}
