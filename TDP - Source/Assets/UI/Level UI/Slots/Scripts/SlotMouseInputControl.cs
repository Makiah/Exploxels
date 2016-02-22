
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

	ResourceReferenceWithStack itemInControlByMouse = null;

	ResourceReferenceWithStack pendingCombinationIngredient1;
	SlotScript assigner1;
	ResourceReferenceWithStack pendingCombinationIngredient2;
	SlotScript assigner2;

	public void AssignItemToMouseControl(ResourceReferenceWithStack assignment) {
		if (assignment.stack != 0) {
			itemInControlByMouse = assignment;
			SetCursorTexture (true);
		} else {
			Debug.LogError("Could not assign item with 0 stack!");
		}
	}

	public ResourceReferenceWithStack DeAssignItemFromMouseControl() {
		ResourceReferenceWithStack toReturn = itemInControlByMouse;
		itemInControlByMouse = null;
		SetCursorTexture (false);
		return toReturn;
	}

	public void AddIngredient(ResourceReferenceWithStack ingredient, SlotScript assigner) {
		if (pendingCombinationIngredient1 == null) {
			pendingCombinationIngredient1 = ingredient;
			assigner1 = assigner;
			assigner1.SetCombinationPending();
			Debug.Log("Added ingredient 1: " + pendingCombinationIngredient1.uiSlotContent.itemScreenName);
		} else {
			pendingCombinationIngredient2 = ingredient;
			assigner2 = assigner;

			Debug.Log("Added ingredient 2: " + pendingCombinationIngredient2.uiSlotContent.itemScreenName);

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

	//Called from the public AddIngredient function.  
	void ManageCombination() {
		// Check the createdIngredientArray to see whether the ResourceReference components match.  
		ResourceReference[] createdIngredientResourceReferenceArray = {
			pendingCombinationIngredient1.uiSlotContent,
			pendingCombinationIngredient2.uiSlotContent
		};

		//For every combination in the database.  
		for (int i = 0; i < ResourceDatabase.masterItemCombinationList.Count; i++) {
			//Convert the two database ingredients in use into an array.  
			ResourceReference[] combinationDatabaseItemRequirements = {
				ResourceDatabase.masterItemCombinationList[i].ingredients[0].uiSlotContent, 
				ResourceDatabase.masterItemCombinationList[i].ingredients[1].uiSlotContent
			};

			//Check whether the local and database arrays are equal.  
			if (ScriptingUtilities.CheckArraysForEquality(createdIngredientResourceReferenceArray, combinationDatabaseItemRequirements)) {
				//Create an integer array that defines the stack of each local item.  
				int[] createdIngredientStackArray = {
					pendingCombinationIngredient1.stack, 
					pendingCombinationIngredient2.stack
				};
				//Create an integer array that defines the stack of each database ingredient.  
				int[] combinationDatabaseStackRequirements = {
					ResourceDatabase.masterItemCombinationList[i].ingredients[0].stack, 
					ResourceDatabase.masterItemCombinationList[i].ingredients[1].stack
				};

				//Determine whether the stacks satisfy the minimum requirement.  
				if (
					createdIngredientStackArray[0] >= combinationDatabaseStackRequirements[0] && createdIngredientStackArray[1] >= combinationDatabaseStackRequirements[1]
				    ) {
					int maxPossibleItemStack = DetermineMaxPossibleStackOfItem(combinationDatabaseStackRequirements, createdIngredientStackArray);
					if (maxPossibleItemStack != 0) {
						assigner1.ModifyCurrentItemStack(-1 * maxPossibleItemStack * combinationDatabaseStackRequirements[0]);
						Debug.Log("Deducted " + maxPossibleItemStack * combinationDatabaseStackRequirements[0] + " from assigner 1.");
						assigner2.ModifyCurrentItemStack(-1 * maxPossibleItemStack * combinationDatabaseStackRequirements[1]);
						Debug.Log("Deducted " + maxPossibleItemStack * combinationDatabaseStackRequirements[1] + " from assigner 2, stack is now.");
							
						ResourceReferenceWithStack finalProduct = new ResourceReferenceWithStack(ResourceDatabase.masterItemCombinationList[i].product.uiSlotContent, ResourceDatabase.masterItemCombinationList[i].product.stack * maxPossibleItemStack);
						AssignItemToMouseControl(finalProduct);
						ResetPendingCombinationSequence();
					} else {
						Debug.Log("Max possible item stack was 0");
					}
					return;
				} else {
					Debug.LogError("Stack did not satisfy the minimum number required.");
					return;
				}
			}

			if (i == ResourceDatabase.masterItemCombinationList.Count - 1) {
				Debug.LogError("Combination ingredients not found");
				ResetPendingCombinationSequence();
			}

		}
	}

	int DetermineMaxPossibleStackOfItem(int[] ingredientBaseStack, int[] actualStackOfIngredients) {
		//Determine max for the first element.  
		int maxStackOfElement0 = ((actualStackOfIngredients[0] - actualStackOfIngredients[0] % ingredientBaseStack[0]) / (ingredientBaseStack[0]));
		int maxStackOfElement1 = ((actualStackOfIngredients[1] - actualStackOfIngredients[1] % ingredientBaseStack[1]) / (ingredientBaseStack[1]));

		if (maxStackOfElement0 <= maxStackOfElement1) {
			Debug.Log("Max of element 0 was " + maxStackOfElement0);
			return maxStackOfElement0;
		} else {
			Debug.Log("Max of element 1 was " + maxStackOfElement1);
			return maxStackOfElement1;
		}
	}

	//Used for setting mouse cursor textures.  
	void SetCursorTexture(bool assignToANewValue) {
		Texture2D cursorTexture;

		Vector2 cursorHotspot;

		if (assignToANewValue) {
			cursorTexture = itemInControlByMouse.uiSlotContent.itemIcon.texture;
			cursorHotspot = new Vector2 (cursorTexture.width / 2f, cursorTexture.height / 2f);
		} else {
			cursorHotspot = Vector2.zero;
			cursorTexture = null;

		}

		Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);
	}

	public ResourceReferenceWithStack GetItemInControlByMouse() {
		return itemInControlByMouse;
	}

	public void ChangeStackOfItemInControlByMouse(int newStack) {
		itemInControlByMouse.stack = newStack;
		if (itemInControlByMouse.stack <= 0) {
			DeAssignItemFromMouseControl();
		}
	}

}
