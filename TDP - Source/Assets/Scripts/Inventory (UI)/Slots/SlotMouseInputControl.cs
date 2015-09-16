
/*
 * Author: Makiah Bennett
 * Last edited: 13 September 2015
 * 
 * 9/13 - Added support for centrally-placed pivot points, as well as for Cursor Texture2Ds.  
 * 9/12 - Added support for right-clicking gestures, halving, placing.  
 * 9/11 - Added support for stacking items.  
 * 
 * This script handles all the mouse-related components of the script (yet is assigned to by the SlotScript).  This script also handles combinations, 
 * by checking the ResourceDatabase for the correct ingredients and product.  
 * 
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlotMouseInputControl : MonoBehaviour {

	UISlotContentReference itemInControlByMouse = null;

	UISlotContentReference pendingCombinationIngredient1;
	SlotScript assigner1;
	UISlotContentReference pendingCombinationIngredient2;
	SlotScript assigner2;

	public void AssignItemToMouseControl(UISlotContentReference assignment) {
		if (assignment.stack != 0) {
			itemInControlByMouse = assignment;
			SetCursorTexture (true);
		} else {
			Debug.LogError("Could not assign item with 0 stack!");
		}
	}

	public UISlotContentReference DeAssignItemFromMouseControl() {
		UISlotContentReference toReturn = itemInControlByMouse;
		itemInControlByMouse = null;
		SetCursorTexture (false);
		return toReturn;
	}

	public void AddIngredient(UISlotContentReference ingredient, SlotScript assigner) {
		if (pendingCombinationIngredient1 == null) {
			pendingCombinationIngredient1 = ingredient;
			assigner1 = assigner;
			assigner1.SetCombinationPending();
			Debug.Log("Added ingredient: " + pendingCombinationIngredient1.uiSlotContent.itemScreenName);
		} else {
			pendingCombinationIngredient2 = ingredient;
			assigner2 = assigner;

			ManageCombination();

		}
	}

	public void ResetPendingCombinationSequence() {
		if (assigner1 != null)
			assigner1.DisableCombinationPending();

		if (assigner2 != null) 
			assigner2.DisableCombinationPending();

		pendingCombinationIngredient1 = null;
		assigner1 = null;
		pendingCombinationIngredient2 = null;
		assigner2 = null;
	}

	void ManageCombination() {
		// Check the createdIngredientArray to see whether the ResourceReference components match.  
		ResourceReference[] createdIngredientResourceReferenceArray = {
			pendingCombinationIngredient1.uiSlotContent,
			pendingCombinationIngredient2.uiSlotContent
		};
		for (int i = 0; i < ResourceDatabase.masterItemCombinationList.Count; i++) {
			
			ResourceReference[] combinationDatabaseItemRequirements = {
				ResourceDatabase.masterItemCombinationList[i].ingredients[0].uiSlotContent, 
				ResourceDatabase.masterItemCombinationList[i].ingredients[1].uiSlotContent
			};
			
			if (ScriptingUtilities.CheckArraysForEquality(createdIngredientResourceReferenceArray, combinationDatabaseItemRequirements)) {
				//Determine whether the stacks contain the correct amount of item.  
				int[] createdIngredientStackArray = {
					pendingCombinationIngredient1.stack, 
					pendingCombinationIngredient2.stack
				};
				int[] combinationDatabaseStackRequirements = {
					ResourceDatabase.masterItemCombinationList[i].ingredients[0].stack, 
					ResourceDatabase.masterItemCombinationList[i].ingredients[1].stack
				};

				if (
					createdIngredientStackArray[0] >= combinationDatabaseStackRequirements[0] && createdIngredientStackArray[1] >= combinationDatabaseStackRequirements[1]
				    ) {
					int maxPossibleItemStack = DetermineMaxPossibleStackOfItem(combinationDatabaseStackRequirements, createdIngredientStackArray);
					if (maxPossibleItemStack != 0) {
						assigner1.ModifyCurrentItemStack(-1 * maxPossibleItemStack * combinationDatabaseStackRequirements[0]);
						Debug.Log("Deducted " + maxPossibleItemStack * combinationDatabaseStackRequirements[0] + " from assigner 1.");
						assigner2.ModifyCurrentItemStack(-1 * maxPossibleItemStack * combinationDatabaseStackRequirements[1]);
						Debug.Log("Deducted " + maxPossibleItemStack * combinationDatabaseStackRequirements[1] + " from assigner 2, stack is now.");
							
						UISlotContentReference finalProduct = new UISlotContentReference(ResourceDatabase.masterItemCombinationList[i].product.uiSlotContent, ResourceDatabase.masterItemCombinationList[i].product.stack * maxPossibleItemStack);
						AssignItemToMouseControl(finalProduct);
						ResetPendingCombinationSequence();
					} else {
						Debug.Log("Max possible item stack was 0");
					}
					return;
				} else {
					Debug.LogError("Stack did not satisfy the minimum number required.");
					ResetPendingCombinationSequence();
					return;
				}
			}

			if (i == ResourceDatabase.masterItemCombinationList.Count - 1) 
				Debug.LogError("Combination ingredients not found");

			ResetPendingCombinationSequence();

		}
	}

	int DetermineMaxPossibleStackOfItem(int[] ingredientBaseStack, int[] actualStackOfIngredients) {
		//Determine max for the first element.  
		int maxStackOfElement0 = ((actualStackOfIngredients[0] - actualStackOfIngredients[0] % ingredientBaseStack[0]) / (ingredientBaseStack[0]));
		int maxStackOfElement1 = ((actualStackOfIngredients[1] - actualStackOfIngredients[1] % ingredientBaseStack[1]) / (ingredientBaseStack[1]));


		Debug.Log ("Max stack of element 0: " + maxStackOfElement0);
		
		Debug.Log ("Max stack of element 1: " + maxStackOfElement1);

		if (maxStackOfElement0 <= maxStackOfElement1) {
			Debug.Log("Max was " + maxStackOfElement0);
			return maxStackOfElement0;
		} else {
			Debug.Log("Max was " + maxStackOfElement1);
			return maxStackOfElement1;
		}
	}
	
	void SetCursorTexture(bool assignToANewValue) {
		Texture2D cursorTexture;

		Vector2 cursorHotspot;

		if (assignToANewValue) {
			cursorTexture = itemInControlByMouse.uiSlotContent.itemIcon.texture;
			Debug.Log ("Used default item icon texture for mouse cursor");
			cursorHotspot = new Vector2 (cursorTexture.width / 2f, cursorTexture.height / 2f);
		} else {
			cursorHotspot = Vector2.zero;
			cursorTexture = null;

		}

		Debug.Log (cursorHotspot);
		Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);
	}

	public UISlotContentReference GetItemInControlByMouse() {
		return itemInControlByMouse;
	}

	public void ChangeStackOfItemInControlByMouse(int newStack) {
		itemInControlByMouse.stack = newStack;
		if (itemInControlByMouse.stack <= 0) {
			DeAssignItemFromMouseControl();
		}
	}

}
