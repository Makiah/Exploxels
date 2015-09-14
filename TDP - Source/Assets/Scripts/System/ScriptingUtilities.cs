﻿
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script contains a few of the major utilities that are used multiple times throughout the scripts.  All of the methods should static.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class ScriptingUtilities : MonoBehaviour {

	public static Transform[] ParseChildrenFromTransform(Transform someTransform) {

		Transform[] childTransforms = new Transform[someTransform.childCount];

		int i = 0;
		foreach (Transform child in someTransform) {
			childTransforms[i] = child;
			i++;
		}

		return childTransforms;

	}
	
	public static bool CheckArraysForEquality <T,S> (T[] arrayA, S[] arrayB) {
		if (arrayA.Length != arrayB.Length)
			return false;
		
		for (int i = 0; i < arrayA.Length; i++) {
			if (!arrayA[i].Equals(arrayB[i]) && !arrayA[arrayA.Length - (i + 1)].Equals(arrayB[i])) 
				return false;
		}
		
		return true;
		
	}

	public static bool CheckUIResourceReferencesForEquality (UISlotContentReference object1, UISlotContentReference object2) {
		if (object1.uiSlotContent.itemType == object2.uiSlotContent.itemType) {
			if (object1.uiSlotContent.localGroupID == object2.uiSlotContent.localGroupID) {
				return true;
			}
		}
		return false;
	}

}