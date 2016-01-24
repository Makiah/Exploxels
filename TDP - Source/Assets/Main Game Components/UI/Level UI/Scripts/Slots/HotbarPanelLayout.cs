using UnityEngine;
using System.Collections;

public class HotbarPanelLayout : PanelLayout {

	protected override void OnEnable() {
		LevelEventManager.CreateHotbarSlots += InitializeSlots;
	}

	protected override void OnDisable() {
		LevelEventManager.CreateHotbarSlots -= InitializeSlots;
	}

}
