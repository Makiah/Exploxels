using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIData : MonoBehaviour {

	void OnEnable() {
		UIEventManager.InitializeUI += InitializeUIElements;
	}

	void OnDisable() {
		UIEventManager.InitializeUI -= InitializeUIElements;
	}

	//This will be the data that the game later receives from the UI.  
	public int chosenRace;

	GameObject gameUI;
	InputField inputField;

	public void InitializeUIElements() {
		DontDestroyOnLoad (this.gameObject);
		gameUI = GameObject.Find ("Game Initialization UI");
		inputField = gameUI.transform.FindChild ("InputField").gameObject.GetComponent <InputField> ();
	}

	public void OnButtonPress() {
		chosenRace = QueryTextField (inputField.text);
		Application.LoadLevel (1);
	}

	int QueryTextField(string someNumber) {
		int outVal;
		int.TryParse (someNumber, out outVal);
		return outVal;
	}

}
