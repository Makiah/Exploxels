using UnityEngine;
using System.Collections;

public class UIEventManager : MonoBehaviour {

	public delegate void UIBaseInitialization();
	public static event UIBaseInitialization InitializeProfileSwitcher;

	void Start() {
		CurrentLevelVariableManagement.SetGameUIReferences ();
		//Initialize the static database.  
		ResourceDatabase.InitializeDatabase ();
		//Initialize the initial UI elements.  
		if (InitializeProfileSwitcher != null) InitializeProfileSwitcher (); else Debug.LogError("InitializeProfileSwitcher was null!!");

		CurrentLevelVariableManagement.GetMainGameControl ().DefineInitialLevelElements ();
	}

}
