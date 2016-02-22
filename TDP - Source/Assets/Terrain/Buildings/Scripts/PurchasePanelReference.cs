using UnityEngine;
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
	ResourceReferenceWithStack heldItem;

	//Player transform
	Transform player;
	InventoryFunctions playerInventory;

	//Done during the InitializePurchasePanels phase (no real dependencies).  
	void InitializePurchasePanelReference() {
		//Define required components.  
		currentItemIcon = transform.FindChild ("Animation Controller").FindChild ("Item Icon").GetComponent <SpriteRenderer> ();
		cost = transform.FindChild ("Animation Controller").FindChild ("Value").FindChild ("Cost").GetComponent <TextMesh> ();
		//Not accessible in the editor, but can be modified via code.  (Looks weird otherwise).  
		cost.GetComponent<MeshRenderer> ().sortingLayerName = "PPanelFront";
		cost.GetComponent<MeshRenderer> ().sortingOrder = 0;
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		playerInventory = CurrentLevelVariableManagement.GetMainInventoryReference ().gameObject.GetComponent <InventoryFunctions> ();
		StartCoroutine (CheckForPurchase());
	}

	//Coroutine that checks for the activation of a W key.  
	IEnumerator CheckForPurchase() {
		while (true) {
			if (Vector2.Distance(player.transform.position, transform.position) < 1) {
				if (Input.GetKeyDown(KeyCode.W)) {
					if (GiveMoneyToPlayer(-1 * int.Parse(cost.text))) {
						Debug.Log("Name of item is " + heldItem.uiSlotContent.itemScreenName);
						playerInventory.AssignNewItemToBestSlot(heldItem);
						RemovePanel();
						Debug.Log("Gave to player");
					} else {
						Debug.Log("Player had insufficient money");
					}
				}
			}

			yield return null;
		}
	}

	//Should be called by PurchasePanelManager.  
	public void DefinePanelItem(ResourceReferenceWithStack item, int requiredCost) {
		if (item != null && item.stack != 0) {
			heldItem = item;
			//Get sprite without pivot point.  
			currentItemIcon.sprite = ScriptingUtilities.GetSpriteWithoutPivotPoint(item.uiSlotContent.itemIcon);
			cost.text = requiredCost.ToString();
		} else 
			Debug.LogError ("Cannot define panel item to be null or have a stack of 0!!");
	}

	//Giving money to player. 
	bool GiveMoneyToPlayer(int amount) {
		return player.GetComponent <PlayerHealthPanelManager> ().GiveMoneyToPlayer (amount);
	}

	//Used when the player purchases the thing on the panel.  
	public void RemovePanel() {
		StopCoroutine ("CheckForPurchase");
		gameObject.SetActive (false);
	}

}
