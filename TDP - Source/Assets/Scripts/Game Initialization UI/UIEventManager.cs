using UnityEngine;
using System.Collections;

public class UIEventManager : MonoBehaviour {

	public delegate void UIBaseInitialization();
	public static event UIBaseInitialization InitializeResourceDatabase;
	public static event UIBaseInitialization InitializeProfileSwitcher;
	public static event UIBaseInitialization InitializeUI;

	void Start() {
		InitializeResourceDatabase ();
		InitializeProfileSwitcher ();
		InitializeUI ();
	}

}
