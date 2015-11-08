﻿using UnityEngine;
using System.Collections;

public class PurchasePanelReference : MonoBehaviour {

	//Initialization stuff.  
	void OnEnable() {
		LevelEventManager.InitializePurchasePanels += InitializePurchasePanelReference;
	}

	void OnDisable() {
		LevelEventManager.InitializePurchasePanels -= InitializePurchasePanelReference;
	}

	//Define required components.  
	SpriteRenderer currentItemIcon;
	TextMesh cost;
	
	//The actual content of the panel.  
	UISlotContentReference heldItem;

	//Done during the InitializePurchasePanels phase (no real dependencies).  
	void InitializePurchasePanelReference() {
		//Define required components.  
		currentItemIcon = transform.FindChild ("Animation Controller").FindChild ("Item Icon").GetComponent <SpriteRenderer> ();
		cost = transform.FindChild ("Animation Controller").FindChild ("Value").FindChild ("Cost").GetComponent <TextMesh> ();
		//Not accessible in the editor, but can be modified via code.  (Looks weird otherwise).  
		cost.GetComponent<MeshRenderer> ().sortingLayerName = "PPanelFront";
		cost.GetComponent<MeshRenderer> ().sortingOrder = 0;
	}

	//Should be called by PurchasePanelManager.  
	public void DefinePanelItem(UISlotContentReference item, int requiredCost) {
		if (item != null && item.stack != 0) {
			heldItem = item;
			//Get sprite without pivot point.  
			currentItemIcon.sprite = ScriptingUtilities.GetSpriteWithoutPivotPoint(item.uiSlotContent.itemIcon);
			Debug.Log("The purchase panel sprite is " + currentItemIcon.sprite.name + " with rect " + currentItemIcon.sprite.rect + " and pivot point " + currentItemIcon.sprite.pivot);
			Debug.Log("It also has an offset of " + currentItemIcon.sprite.textureRectOffset);
			cost.text = requiredCost.ToString();
		} else 
			Debug.LogError ("Cannot define panel item to be null or have a stack of 0!!");
	}

	//Used when the player purchases the thing on the panel.  
	public UISlotContentReference GetItemAndRemovePanel() {
		if (heldItem != null) {
			Debug.Log ("You should see a message stating that the item has been returned.  If you do not see a message, go to the PurchasePanelReference script and work on a workaround for destroying an item before returning.");
			Destroy (gameObject);
			Debug.Log ("The item is being returned.");
			return heldItem;
		} else {
			Debug.LogError("Cannot return empty item! (PurchasePanelReference)");
			return null;
		}
	}

}
