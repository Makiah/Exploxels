using UnityEngine;
using System.Collections;

public class UIEventManager : MonoBehaviour {

	public delegate void UIBaseInitialization();
	public static event UIBaseInitialization InitializeProfileSwitcher;
	public static event UIBaseInitialization InitializeUI;

	void Start() {
		//Initialize the static database.  
		ResourceDatabase.InitializeDatabase ();
		//Initialize the initial UI elements.  
		if (InitializeProfileSwitcher != null) InitializeProfileSwitcher (); else Debug.LogError("InitializeProfileSwitcher was null!!");
		if (InitializeUI != null) InitializeUI (); else Debug.LogError("InitializeUI was null!!");
	}

}
