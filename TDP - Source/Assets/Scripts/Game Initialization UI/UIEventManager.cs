using UnityEngine;
using System.Collections;

public class UIEventManager : MonoBehaviour {

	public delegate void UIBaseInitialization();
	public static event UIBaseInitialization InitializeResourceDatabase;
	public static event UIBaseInitialization InitializeProfileSwitcher;
	public static event UIBaseInitialization InitializeUI;

	void Start() {
		if (InitializeResourceDatabase != null) InitializeResourceDatabase (); else Debug.LogError ("Database was not initialized");
		if (InitializeProfileSwitcher != null) InitializeProfileSwitcher (); else Debug.LogError("InitializeProfileSwitcher was null!!");
		if (InitializeUI != null) InitializeUI (); else Debug.LogError("InitializeUI was null!!");
	}

}
