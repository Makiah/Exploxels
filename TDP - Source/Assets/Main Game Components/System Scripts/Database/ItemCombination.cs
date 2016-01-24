
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This is a relatively straightforward script, just working to reference an array of UISlotContentReference[] ingredients with a product, 
 * which will be used by the ResourceDatabase script.  
 * 
 * 
 */

using UnityEngine;
using System.Collections;

public class ItemCombination {

	public UISlotContentReference[] ingredients;
	public UISlotContentReference product;

	public ItemCombination (UISlotContentReference[] ctorIngredients, UISlotContentReference ctorProduct) {
		ingredients = ctorIngredients;
		product = ctorProduct;
	}

}
