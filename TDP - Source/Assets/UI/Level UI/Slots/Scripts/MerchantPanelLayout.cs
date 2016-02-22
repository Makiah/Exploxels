
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script is pretty cool.  Attached to the Inventory and Hotbar objects, this script mathematically determines how to place
 * slots in a way that works well for the system.  This means that at any point in time, you can resize the panel itself, and the 
 * number of slots generated will differ.  
 * 
 * 
 */

using UnityEngine;
using System.Collections;

public class MerchantPanelLayout : MonoBehaviour {

	protected virtual void OnEnable () {
		LevelEventManager.CreateInventorySlots += AddSlotsToSystem;
	}

	protected virtual void OnDisable () {
		LevelEventManager.CreateInventorySlots -= AddSlotsToSystem;
	}

	public GameObject slotPrefab;

	protected Vector2 inventoryPanelSize, slotPanelSize;

	protected virtual void AddSlotsToSystem() {
		GetComponent <MerchantInventoryFunctions> ().AddSlotsToSystem (InitializeSlots ());
		Debug.Log ("Added slots");
	}

	protected MerchantSlotScript[,] InitializeSlots() {
		//Create the "Slots" parent.  Has to be used so that InventoryHideShow does not end its coroutine.  
		Transform slots = transform.FindChild("Slots");
		slots.SetParent (transform, false);
		slots.transform.localPosition = Vector3.zero;

		// Get sizes of the inventory panel and the slot prefab.  
		inventoryPanelSize = slots.GetComponent <RectTransform> ().sizeDelta;
		slotPanelSize = slotPrefab.GetComponent <RectTransform> ().sizeDelta;

		// Determine the maximum number of slots that can fit on either axis of the inventory panel.  
		int maxNumberOfXSlots = (int) ((inventoryPanelSize.x - (inventoryPanelSize.x % slotPanelSize.x)) / (slotPanelSize.x));
		int maxNumberOfYSlots = (int) ((inventoryPanelSize.y - (inventoryPanelSize.y % slotPanelSize.y)) / (slotPanelSize.y));

		// Determine the remaining area unaccounted for by the slots themselves.  
		float additionalPanelXSizeRemaining = inventoryPanelSize.x % slotPanelSize.x;
		float additionalPanelYSizeRemaining = inventoryPanelSize.y % slotPanelSize.y;

		//Using the previous step, determine the excess slot space that should be used for each group.  
		float paddingPerXSlot = additionalPanelXSizeRemaining / (maxNumberOfXSlots + 1);
		float paddingPerYSlot = additionalPanelYSizeRemaining / (maxNumberOfYSlots + 1);

		//MerchantSlotScript 2d array.  
		MerchantSlotScript[,] createdUISlots = new MerchantSlotScript[maxNumberOfYSlots, maxNumberOfXSlots];

		//For every column, 
		for (int y = 1; y < maxNumberOfYSlots + 1; y++) {
			//For every row, 
			for (int x = 1; x < maxNumberOfXSlots + 1; x++) {

				//Create the slot
				GameObject createdSlot = (GameObject)Instantiate (slotPrefab);
				createdSlot.transform.SetParent (slots, false);

				//Determine the coordinates and offset.  Equal to the previous padding for x/y and slot panel size (x-1) and the current size.  
				float rectTransformXCoordinate = (float)((x - 1) * (paddingPerXSlot + slotPanelSize.x) + (paddingPerXSlot + .5 * slotPanelSize.x));
				float rectTransformYCoordinate = (float)((y - 1) * (paddingPerYSlot + slotPanelSize.y) + (paddingPerYSlot + .5 * slotPanelSize.y));

				//Determine the displacement vector (equal to half of the size of the inventory panel).
				Vector2 displacementVector = (.5f * inventoryPanelSize);
				createdSlot.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (rectTransformXCoordinate, rectTransformYCoordinate) - displacementVector;

				createdSlot.name = "Slot " + x + "." + y;

				createdUISlots [y - 1, x - 1] = createdSlot.GetComponent <MerchantSlotScript> ();
			}
		}

		//Return created slot array.  
		return createdUISlots;

	}

}
