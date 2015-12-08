using UnityEngine;
using System.Collections;

public class DropUtilities : MonoBehaviour {

	//Used to instantiate a dropped item.  
	public static GameObject InstantiateDroppedItem(UISlotContentReference itemReference, float xOffset) {

		GameObject basicDrop = Resources.Load ("Prefabs/Items/Other/BasicDrop") as GameObject;

		GameObject createdObject = (GameObject) (Instantiate (basicDrop, 
		                                                      CurrentLevelVariableManagement.GetPlayerReference().transform.position + Vector3.right * xOffset + basicDrop.transform.localPosition, 
		                                                      Quaternion.identity));
		//Give the object the spriterenderer.  
		createdObject.transform.FindChild("SpriteAnimation").GetComponent <SpriteRenderer> ().sprite = itemReference.uiSlotContent.itemIcon;
		//Add the object info to the created object.  
		createdObject.AddComponent <DroppedItemProperties> ();
		createdObject.GetComponent <DroppedItemProperties> ().localResourceReference = new UISlotContentReference(itemReference.uiSlotContent, itemReference.stack);
		createdObject.GetComponent <DroppedItemProperties> ().Initialize();

		return createdObject;
	}
}
