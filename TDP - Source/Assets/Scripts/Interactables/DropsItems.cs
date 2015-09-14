
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script is the base class for all objects that drop items when "killed", which includes trees (wood), 
 * boars (meat), and skeletons (bones, exp, coins).  These drops should be defined through the DropReferenceClass.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class DropsItems : MonoBehaviour {

	protected DropReferenceClass[] drops;

	protected void DropItems() {
		if (drops != null) {
			for (int i = 0; i < drops.Length; i++) {
				if (Random.Range (0, drops [i].probabilityToDrop) == 0) {
					for (int q = 0; q < Random.Range(drops[i].minToDrop, drops[i].maxToDrop + 1); q++) {
						if (drops[i].dropReference != null) {
							GameObject instantiatedDrop = (GameObject) (Instantiate (drops [i].dropReference.inGamePrefab, transform.position, Quaternion.identity));
							instantiatedDrop.AddComponent <DroppedItemProperties> ();
							instantiatedDrop.GetComponent <DroppedItemProperties> ().localResourceReference = drops[i].dropReference;
							Debug.Log ("Instantiated drops");
						} else {
							Debug.Log("DropReference was null!!!");
						}
					}
				}
			}
		} else {
			Debug.Log("Drops were null");
		}
	}

}
