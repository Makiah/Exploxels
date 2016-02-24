/*
 * Author: Makiah Bennett
 * Last edited: 13 September 2015
 * 
 * This script controls the UI component of the slot, and manages the stacking, moving, combining, and adding items to this slot.  
 * Each slot instance has it's own SlotScript.  This class also serves as a base class to HotbarSlotScript.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MerchantSlotScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	/******************************* INITIALIZATION *******************************/

	protected void OnEnable() {
		LevelEventManager.InitializeSlots += ReferenceChildren;
	}

	protected void OnDisable() {
		LevelEventManager.InitializeSlots -= ReferenceChildren;
	}


	/******************************** SLOT SCRIPT *******************************/

	// Main properties of each slot.  
	protected ResourceReferenceWithStackAndPrice currentlyAssigned;

	//Components
	private Image childIcon;
	private Transform tooltip;
	private Text stackIndicator;
	private Text priceIndicator;

	//References
	private InventoryFunctions playerInventory;

	public void ReferenceChildren() {
		Debug.Log ("Initialized merchant inventory " + gameObject.name);
		childIcon = transform.FindChild ("Icon").GetComponent <Image> ();
		childIcon.enabled = false;
		tooltip = transform.FindChild ("Tooltip");
		stackIndicator = transform.FindChild ("Stack Indicator").GetComponent <Text> ();
		priceIndicator = transform.FindChild ("Price Indicator").GetComponent <Text> ();
		UpdateStackIndicator ();
		UpdatePriceIndicator ();
	}

	/******************************* MOUSE CLICK MANAGER *******************************/

	public void OnPointerClick(PointerEventData data) {
		if (data.button == PointerEventData.InputButton.Left) {
			if (currentlyAssigned != null) {
				if (CurrentLevelVariableManagement.GetPlayerReference ().GetComponent <PlayerHealthPanelManager> ().GiveMoneyToPlayer (-currentlyAssigned.price)) {
					//Add the deassigned item to the player inventory and deduct the price of the item.  
					CurrentLevelVariableManagement.GetMainInventoryReference ().GetComponent <InventoryFunctions> ().AssignNewItemToBestSlot (new ResourceReferenceWithStack (currentlyAssigned.mainContentReference.uiSlotContent, 1));
					ModifyCurrentItemStack (-1);
				}
			}
		}
	}

	/******************************* ASSIGNING *******************************/

	public virtual void AssignNewItem(ResourceReferenceWithStackAndPrice itemToAssign) {
		if (itemToAssign.mainContentReference.stack > 0) {
			Sprite itemWithoutPivotPoint = ScriptingUtilities.GetSpriteWithoutPivotPoint(itemToAssign.mainContentReference.uiSlotContent.itemIcon);
			childIcon.enabled = true;
			currentlyAssigned = itemToAssign;
			childIcon.sprite = itemWithoutPivotPoint;
			UpdateStackIndicator ();
			UpdatePriceIndicator ();
		} else {
			Debug.LogError("Could not assign item with 0 or less stack!");
		}
	}

	public virtual ResourceReferenceWithStackAndPrice DeAssignItem() {
		ResourceReferenceWithStackAndPrice toReturn = currentlyAssigned;
		currentlyAssigned = null;
		childIcon.sprite = null;
		childIcon.enabled = false;
		UpdateStackIndicator ();
		UpdatePriceIndicator ();
		return toReturn;
	}

	public ResourceReferenceWithStackAndPrice GetCurrentlyAssigned() {
		return currentlyAssigned;
	}


	/******************************* MOUSE INPUT *******************************/

	public void OnPointerEnter(PointerEventData data) {
		if (currentlyAssigned != null) {
			ShowTooltip ();
		}
	}

	public void OnPointerExit(PointerEventData data) {
		if (tooltip.gameObject.activeSelf == true) {
			HideTooltip();
		}
	}

	/******************************* TOOLTIP *******************************/

	void ShowTooltip() {
		tooltip.gameObject.SetActive (true);
		tooltip.FindChild ("Title").GetComponent <Text> ().text = currentlyAssigned.mainContentReference.uiSlotContent.itemScreenName;
		tooltip.FindChild ("Description").GetComponent <Text> ().text = currentlyAssigned.mainContentReference.uiSlotContent.itemDescription;

		//Otherwise this panel is not visible.  
		transform.SetAsLastSibling();
	}

	void HideTooltip() {
		tooltip.gameObject.SetActive (false);
	}


	/******************************* STACK MANAGER *******************************/

	public void ModifyCurrentItemStack(int newStackValue) {
		currentlyAssigned.mainContentReference.stack += newStackValue;
		if (currentlyAssigned.mainContentReference.stack <= 0) {
			DeAssignItem();
		}
		UpdateStackIndicator ();
		UpdatePriceIndicator ();
	}

	public void UpdateStackIndicator() {
		if (currentlyAssigned != null) {
			if (currentlyAssigned.mainContentReference.stack <= 1) {
				stackIndicator.text = "";
				stackIndicator.gameObject.SetActive (false);
			} else {
				stackIndicator.gameObject.SetActive (true);
				stackIndicator.text = currentlyAssigned.mainContentReference.stack.ToString ();
			}
		} else {
			stackIndicator.text = "";
			stackIndicator.gameObject.SetActive(false);
		}
	}

	/******************************* PRICE MANAGER *******************************/
	public void UpdatePriceIndicator() {
		if (currentlyAssigned != null) {
			priceIndicator.gameObject.SetActive (true);
			priceIndicator.text = "" + currentlyAssigned.price;
		} else {
			priceIndicator.text = "";
			priceIndicator.gameObject.SetActive (false);
		}
	}

}
