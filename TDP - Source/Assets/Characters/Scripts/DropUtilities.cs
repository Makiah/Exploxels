using UnityEngine;
using System.Collections;

public class DropUtilities : MonoBehaviour {

	//Used to instantiate a dropped item.  
	public static GameObject InstantiateDroppedItem(ResourceReferenceWithStack itemReference, Transform initialPosition, float xForce) {

		GameObject basicDrop = Resources.Load ("Prefabs/Items/Other/BasicDrop") as GameObject;

		GameObject createdObject = (GameObject) (Instantiate (basicDrop, 
			initialPosition.position, 
			Quaternion.identity));
		//Give the object the spriterenderer.  
		createdObject.transform.FindChild("SpriteAnimation").GetComponent <SpriteRenderer> ().sprite = itemReference.uiSlotContent.itemIcon;
		//Add the object info to the created object.  
		createdObject.AddComponent <DroppedItemProperties> ();
		//Drop one of the items.  
		createdObject.GetComponent <DroppedItemProperties> ().localResourceReference = new ResourceReferenceWithStack(itemReference.uiSlotContent, 1);
		//Give the rigidbody a bit of initial velocity.  
		createdObject.GetComponent <Rigidbody2D> ().AddForce(new Vector2(xForce, 0));
		createdObject.transform.position = initialPosition.position + new Vector3 (Mathf.Sign (xForce), 0, 0);
		//Initialize the droppd item.  
		createdObject.GetComponent <DroppedItemProperties> ().Initialize();

		return createdObject;
	}
}
