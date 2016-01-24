using UnityEngine;
using System.Collections;

public class ProfessionEventManager : MonoBehaviour {

	public delegate void BaseInitialization();

	public static event BaseInitialization InitializeProfessionChoiceManager;
	public static event BaseInitialization InitializeProfessionSpeechManager;

	void Start() {
		if (InitializeProfessionChoiceManager != null) InitializeProfessionChoiceManager(); else Debug.LogError("InitializeProfessionChoiceManager was null!!!");
		if (InitializeProfessionSpeechManager != null) InitializeProfessionSpeechManager(); else Debug.LogError("InitializeProfessionSpeechManager was null!");

		Debug.Log ("ProfessionEventManager completed");

		CurrentLevelVariableManagement.GetMainGameControl().InitializeProfessionObjects ();
	}

}
