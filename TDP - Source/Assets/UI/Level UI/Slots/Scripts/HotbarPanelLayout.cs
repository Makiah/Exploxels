using UnityEngine;
using System.Collections;

public class HotbarPanelLayout : PanelLayout {

	protected override void OnEnable() {
		LevelEventManager.CreateHotbarSlots += AddSlotsToSystem;
	}

	protected override void OnDisable() {
		LevelEventManager.CreateHotbarSlots -= AddSlotsToSystem;
	}

}
