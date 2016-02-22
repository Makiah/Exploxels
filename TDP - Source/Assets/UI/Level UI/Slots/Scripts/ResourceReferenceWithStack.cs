
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

public class ResourceReferenceWithStack {

	public ResourceReference uiSlotContent;
	public int stack;

	public ResourceReferenceWithStack (ResourceReference ctorUISlotContent, int ctorStack) {
		uiSlotContent = ctorUISlotContent;
		stack = ctorStack;
	}

}
