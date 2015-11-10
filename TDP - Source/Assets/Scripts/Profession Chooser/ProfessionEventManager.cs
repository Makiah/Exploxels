using UnityEngine;
using System.Collections;

public class ProfessionEventManager : MonoBehaviour {

	public delegate void BaseInitialization();

	public static event BaseInitialization InitializeProfessionChoiceManager;
	public static event BaseInitialization InitializeProfessionData;

	void Start() {
		if (InitializeProfessionChoiceManager != null) InitializeProfessionChoiceManager(); else Debug.LogError("InitializeProfessionChoiceManager was null!!!");

		Debug.Log ("ProfessionEventManager completed");

		GameObject.Find ("UI Data").GetComponent <GameData> ().DefineProfessionChooserElements ();
	}

}
