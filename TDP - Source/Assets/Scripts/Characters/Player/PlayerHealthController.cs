using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthController : CharacterHealthController {

	/************************* INITIALIZATION *************************/
	
	protected override void OnEnable() {
		EventManager.InitializePlayer += InitializeHealthBar;
	}
	
	protected override void OnDisable() {
		EventManager.InitializePlayer -= InitializeHealthBar;
	}

	/************************* HEALTH MANAGER *************************/
	

	protected override void InitializeHealthBar() {
		base.InitializeHealthBar ();
		createdHealthPanelPrefab.SetActive (true);
	}

}
