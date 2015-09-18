using UnityEngine;
using System.Collections;

public class HotbarPanelLayout : PanelLayout {

	protected override void OnEnable() {
		EventManager.CreateHotbarSlots += InitializeSlots;
	}

	protected override void OnDisable() {
		EventManager.CreateHotbarSlots -= InitializeSlots;
	}

}
