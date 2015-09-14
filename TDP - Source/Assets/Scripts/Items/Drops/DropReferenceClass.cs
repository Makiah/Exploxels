
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script is only used by enemies (such as trees, skeletons, etc.) and used when the object is instantiated.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class DropReferenceClass {
	public ResourceReference dropReference;
	public int minToDrop, maxToDrop, probabilityToDrop;

	public DropReferenceClass(ResourceReference ctorDropReference, int ctorMinToDrop, int ctorMaxToDrop, int ctorProbabilityToDrop) {
		dropReference = ctorDropReference;
		minToDrop = ctorMinToDrop;
		maxToDrop = ctorMaxToDrop;
		probabilityToDrop = ctorProbabilityToDrop;
	}
}