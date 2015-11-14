using UnityEngine;
using System.Collections;

public class CurrentLevelVariableManagement : MonoBehaviour {

	//Initial screen
	static GameData mainGameData;
	static GameControl mainGameControl;

	public static void SetGameUIReferences() {
		mainGameData = GameObject.Find ("Game Controller").GetComponent <GameData> ();
		mainGameControl = GameObject.Find ("Game Controller").GetComponent <GameControl> ();
	}
	
	public static GameData GetMainGameData() {
		if (mainGameData == null) {
			Debug.LogError("GameData was null!");
		}
		return mainGameData;
	}
	
	public static GameControl GetMainGameControl() {
		if (mainGameControl == null) {
			Debug.LogError("GameControl was null!");
		}
		return mainGameControl;
	}

	//Level
	static GameObject playerObject;
	static GameObject mainCamera;
	static GameObject levelUI;
	static float levelLengthX;
	static ObjectiveManager mainObjectiveManager;

	public static void SetLevelReferences() {
		playerObject = GameObject.Find ("PlayerReferenceObject(Clone)");
		mainCamera = playerObject.transform.FindChild ("Main Camera").gameObject;
		levelUI = GameObject.Find ("UI");
		mainObjectiveManager = GetLevelUIReference ().transform.FindChild ("Player Objectives").GetComponent <ObjectiveManager> ();
	}

	public static ObjectiveManager GetMainObjectiveManager() {
		return mainObjectiveManager;
	}

	public static GameObject GetPlayerReference() {
		if (playerObject == null) 
			Debug.LogError ("Player reference was null!!!");
		return playerObject;
	}

	public static GameObject GetMainCameraReference() {
		if (mainCamera == null) 
			Debug.LogError ("Main Camera was null!!!");
		return mainCamera;
	}

	public static GameObject GetLevelUIReference() {
		if (levelUI == null) 
			Debug.LogError ("Level UI was null!!!");
		return levelUI;
	}

	public static void SetLevelLengthX(float value) {
		levelLengthX = value;
	}

	public static float GetLevelLengthX () {
		if (levelLengthX == 0)
			Debug.LogError ("WARNING: Returning 0 as level length!");
		return levelLengthX;
	}
}
