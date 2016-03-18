using UnityEngine;
using System.Collections;

public class LatestPrefabInstantiator : MonoBehaviour {

	//The object.  
	[SerializeField] private GameObject prefab = null;

	//Start at the Start() method.  
	void Start() {
		if (prefab != null) {
			//Create the prefab with all of the properties of this item.  
			GameObject instantiatedPrefab = (GameObject) (Instantiate(prefab, Vector2.zero, Quaternion.identity));
			if (transform.parent != null)
				instantiatedPrefab.transform.SetParent (transform.parent);
			instantiatedPrefab.transform.localPosition = transform.localPosition;

			//Set the scale and rotation of the object to the prefab's properties.  
			instantiatedPrefab.transform.localRotation = prefab.transform.localRotation;
			instantiatedPrefab.transform.localScale = prefab.transform.localScale;
			//Get rid of the instantiator.  
			Destroy (gameObject);
		} else {
			Debug.Log ("Unable to instantiate an item that does not exist for object " + gameObject.name);
		}
	}

}
