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

	protected override void InitializeSpeechControl() {
		base.InitializeSpeechControl ();
		SaySomething (ResourceDatabase.GetRaceByParameter ("Gatherer").male.head, new string[]{
			"Welcome, young traveler, to the world of Exploxels!", 
			"Here, you will face terrifying beasts as well as other players for control of different time periods.", 
			"But be warned, this is no easy feat.", 
			"If you would still like to continue, choose a profession above."
		}, true);
	}

}
