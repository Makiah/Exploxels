using UnityEngine;
using System.Collections;

public class LatestPrefabInstantiator : MonoBehaviour {

	//The object.  
	[SerializeField] private GameObject prefab;

	//Start at the Start() method.  
	void Start() {
		if (prefab != null) {
			//Create the prefab with all of the properties of this item.  
			GameObject instantiatedPrefab = (GameObject) (Instantiate(prefab, Vector2.zero, Quaternion.identity));
			if (transform.parent != null)
				instantiatedPrefab.transform.SetParent (transform.parent);
			instantiatedPrefab.transform.localPosition = transform.localPosition;
			instantiatedPrefab.transform.localRotation = transform.localRotation;
			instantiatedPrefab.transform.localScale = transform.localScale;
			//Get rid of the instantiator.  
			Destroy (gameObject);
		} else {
			Debug.Log ("Unable to instantiate an item that does not exist for object " + gameObject.name);
		}
	}

}
