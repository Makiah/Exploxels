using UnityEngine;
using System.Collections;

public class OreScript : DropsItems {

	void OnEnable() {
		EventManager.InitializeEnemies += ReferenceOreComponents;
	}
	
	void OnDisable() {
		EventManager.InitializeEnemies -= ReferenceOreComponents;
	}

	public int hitsUntilDrop = 2;
	private int currentHits;

	void ReferenceOreComponents() {
		DropReferenceClass oreDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter ("Rock"), 1, 1, 1);
		drops = new DropReferenceClass[]{oreDrop};
	}

	public void OnOreChipped() {
		currentHits++;
		if (currentHits >= hitsUntilDrop) {
			DropItems();
			Destroy(this.gameObject);
		}
	}

}
