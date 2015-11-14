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
	public void SetSpeechDialogue(string[] stuffToSay) {
		SaySomething (ResourceDatabase.GetRaceByParameter ("Gatherer").male.head, "Guide", stuffToSay, true, null);
	}

	//Modify what happens when the player has clicked through each message.  
	protected override void CompletedSpeakingToPlayer() {
		gameObject.SetActive (false);
		//When the guide has completed speaking.  
		CurrentLevelVariableManagement.GetMainGameControl ().OnSpeechHasBeenCompleted ();
	}

}
