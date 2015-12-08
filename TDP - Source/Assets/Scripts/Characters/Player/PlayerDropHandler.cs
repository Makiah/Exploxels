
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script pretty much just sends a continual linecast in front of the player, and if an object is found and matches the specified
 * criteria, the script will then assign that item to the best available slot.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class PlayerDropHandler : MonoBehaviour {

	/************************************************** DROP HANDLER **************************************************/
	
	//When an item drop hits the player.  
	void OnTriggerEnter2D(Collider2D externalTrigger) {
		if ((externalTrigger.gameObject.GetComponent <DroppedItemProperties> () != null || externalTrigger.gameObject.CompareTag("ExpNodule")) && ModifiesSlotContent.IsInitialized()) 
			PickupItem (externalTrigger.gameObject);
	}

	public void PickupItem(GameObject item) {
		//This does not check the resourcereference property of the attached script as a comparison, only the tag.  Consider changing later.  
		if (!(item.CompareTag ("ExpNodule"))) {
			UISlotContentReference pendingObject = item.GetComponent <DroppedItemProperties> ().localResourceReference;
			if (! ModifiesSlotContent.AssignNewItemToBestSlot(pendingObject)) {
				Debug.LogError("ERROR WHEN ASSIGNING OBJECT");
			} else {
				Destroy (item);
			}
		} else {
			transform.parent.gameObject.GetComponent <PlayerHealthPanelManager> ().OnExperienceNodulePickedUp(1);
			Destroy(item);
		}
	}

}
