
/*
 * Author: Makiah Bennett
 * Last edited: 8 October 2015
 * 
 * This script is the base class for all objects that drop items when "killed", which includes trees (wood), 
 * boars (meat), and skeletons (bones, exp, coins).  These drops should be defined through the DropReferenceClass.
 * 
 * 10/8 - Added OnEnable and OnDisable to the base class, and made MakeReferences an abstract function.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public abstract class DropsItems : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeEnemies += MakeReferences;
	}

	void OnDisable() {
		LevelEventManager.InitializeEnemies -= MakeReferences;
	}

	protected DropReferenceClass[] drops;

	private DropReferenceClass expDrop, coinDrop;

	[SerializeField] protected int experienceToDrop = 0, cashToDrop = 0;

	protected virtual void MakeReferences() {
		expDrop = new DropReferenceClass(ResourceDatabase.GetItemByParameter ("ExpNodule"), 1, 1, 1);
		coinDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter ("Coin"), 1, 1, 1);
	}

	protected void DropItems() {
		if (drops != null) {
			for (int i = 0; i < drops.Length; i++) {
				if (Random.Range (0, drops [i].probabilityToDrop) == 0) {
					for (int q = 0; q < Random.Range(drops[i].minToDrop, drops[i].maxToDrop + 1); q++) {
						if (drops[i].dropReference != null) {
							DropUtilities.InstantiateDroppedItem(new ResourceReferenceWithStack(drops[i].dropReference, 1), transform, 0);
						} else {
							Debug.Log("DropReference was null!!! (DropsItems)");
						}
					}
				}
			}
		} else {
			Debug.Log("Drops were null (DropsItems)");
		}

		if (experienceToDrop > 0) {
			for (int i = 0; i < experienceToDrop; i++) {
				GameObject expDropped = (GameObject) (Instantiate (expDrop.dropReference.playerHoldingPrefab, transform.position, Quaternion.identity));
				expDropped.AddComponent <DroppedItemProperties> ();
				expDropped.GetComponent <DroppedItemProperties> ().localResourceReference = new ResourceReferenceWithStack(expDrop.dropReference, 1);
				expDropped.GetComponent <DroppedItemProperties> ().Initialize();
			}
		} else {
			Debug.Log("Did not drop any experience, experience to drop was 0. (DropsItems)");
		}

		if (cashToDrop > 0) {
			for (int i = 0; i < cashToDrop; i++) {
				GameObject cashDropped = (GameObject) (Instantiate (coinDrop.dropReference.playerHoldingPrefab, transform.position, Quaternion.identity));
				cashDropped.AddComponent <DroppedItemProperties> ();
				cashDropped.GetComponent <DroppedItemProperties> ().localResourceReference = new ResourceReferenceWithStack(coinDrop.dropReference, 1);
				cashDropped.GetComponent <DroppedItemProperties> ().Initialize();
			}
		} else {
			Debug.Log("Did not drop any experience, experience to drop was 0. (DropsItems)");
		}

	}

}
