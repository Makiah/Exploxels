
/*
 * Author: Makiah Bennett
 * Last edited: 18 November 2015
 * 
 * This script contains a few of the major utilities that are used multiple times throughout the scripts.  All of the methods should static.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class ScriptingUtilities : MonoBehaviour {

	//For some reason, there is no method for this in the Transform class.  Go figure.  
	public static Transform[] ParseChildrenFromTransform(Transform someTransform) {
		Transform[] childTransforms = new Transform[someTransform.childCount];

		int i = 0;
		foreach (Transform child in someTransform) {
			childTransforms[i] = child;
			i++;
		}

		return childTransforms;
	}

	//Used for checking arrays for equality.  
	public static bool CheckArraysForEquality <T,S> (T[] arrayA, S[] arrayB) {
		if (arrayA.Length != arrayB.Length)
			return false;
		for (int i = 0; i < arrayA.Length; i++) {
			if (!arrayA[i].Equals(arrayB[i])) 
				return false;
		}
		return true;
	}

	//Used for checking UIResourceReference classes for equality.  
	public static bool CheckUIResourceReferencesForEquality (ResourceReferenceWithStack object1, ResourceReferenceWithStack object2) {
		if (object1.uiSlotContent.itemType == object2.uiSlotContent.itemType) {
			if (object1.uiSlotContent.localGroupID == object2.uiSlotContent.localGroupID) {
				return true;
			}
		}
		return false;
	}

	//Used for converting degrees to radians.  
	public static float DegreesToRadians (float degrees) {
		float toReturn = degrees * (float)(Mathf.PI / 180f);
		return toReturn;
	}

	//Used for converting radians to degrees.  
	public static float RadiansToDegrees (float radians) {
		float toReturn = radians * (float)(180f / Mathf.PI);
		return toReturn;
	}

	//Uses generics to get any random object from an array.  
	public static T GetRandomObjectFromArray <T> (T[] array) {
		if (array.Length != 0) {
			return array [Random.Range (0, array.Length)];
		} else {
			Debug.LogError("Array length was 0!");
			//return null does not work for some reason.  
			return default(T);
		}
	}

	//Used for getting sprites without a pivot point.  
	public static Sprite GetSpriteWithoutPivotPoint(Sprite originalSprite) {
		if (originalSprite != null) 
			return Sprite.Create (originalSprite.texture, originalSprite.rect, new Vector2(0.5f, 0.5f));
		else {
			Debug.LogError("Cannot get new sprite from null.");
			return null;
		}
	}

	//Pauses game.  
	public static void PauseGame() {
		Time.timeScale = 0;
		Debug.Log ("Game is paused!");
	}

	//Resumes game.  
	public static void ResumeGame() {
		Time.timeScale = 1;
		Debug.Log ("Game has been re-started!");
	}

	//If the game is playing in the Unity Editor, it has to quit a different way.  
	public static void QuitGame() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

}
