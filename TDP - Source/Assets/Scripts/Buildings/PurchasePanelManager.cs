using UnityEngine;
using System.Collections;

public class PurchasePanelManager : MonoBehaviour {

	//Initialization Stuff
	void OnEnable() {
		LevelEventManager.InitializePurchasePanelManager += InitializePurchasePanelManager;
	}

	void OnDisable() {
		LevelEventManager.InitializePurchasePanelManager -= InitializePurchasePanelManager;
	}

	//Array of purchase panels.  
	PurchasePanelReference[] purchasePanels;

	//Initialize Purchase Panel Manager.  
	void InitializePurchasePanelManager() {
		Transform purchasePanelsTransform = transform.FindChild("Purchase Panels");
		purchasePanels = new PurchasePanelReference[purchasePanelsTransform.childCount];
		for (int i = 0; i < purchasePanelsTransform.childCount; i++) {
			//Define purchase panel.  
			purchasePanels[i] = purchasePanelsTransform.GetChild(i).GetComponent <PurchasePanelReference> ();

			//Initialize purchase panel.  
			purchasePanels [i].DefinePanelItem (
				new UISlotContentReference (ResourceDatabase.GetItemByParameter ("Wooden Sword"), 1), //Item
				25 //Cost 
			);

		}
	}

}
