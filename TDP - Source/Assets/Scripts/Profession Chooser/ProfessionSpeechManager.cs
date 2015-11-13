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
		SaySomething (ResourceDatabase.GetRaceByParameter ("Gatherer").male.head, "Guide", new string[]{
			"Welcome, young wanderer, to the world of Exploxels!", 
			"Our world is undergoing rapid changes, so prepare yourself.",
			"You will face both monsters and evil humans.",
			"But be warned, this is no easy feat.", 
			"If you would still like to continue, choose a profession above."
		}, true);
	}

}
