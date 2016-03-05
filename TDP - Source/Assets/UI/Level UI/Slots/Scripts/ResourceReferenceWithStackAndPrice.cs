
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script handles the slot content of the inventory, pretty much only adding stack functionality.  
 * Accessed by most of the Inventory scripts.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class ResourceReferenceWithStackAndPrice {

	public ResourceReferenceWithStack mainContentReference;
	public int price;

	public ResourceReferenceWithStackAndPrice (ResourceReferenceWithStack ctorMainContentReference, int ctorPrice) {
		mainContentReference = ctorMainContentReference;
		price = ctorPrice;
	}

}
