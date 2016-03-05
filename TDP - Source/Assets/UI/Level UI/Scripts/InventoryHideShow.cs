
/*
 * Author: Makiah Bennett
 * Created 14 September 2015
 * Last edited: 14 September 2015
 * 
 * This script hides and shows the inventory when the 'm' key is pressed.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class InventoryHideShow : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/
	
	void OnEnable() {
		LevelEventManager.EnableUIHideShow += CheckForHideShow;
	}
	
	void OnDisable() {
		LevelEventManager.EnableUIHideShow -= CheckForHideShow;
	}
	
	
	/************************************************** HOTBAR MANAGEMENT **************************************************/

	GameObject inventory;

	void CheckForHideShow() {
		inventory = transform.FindChild ("Slots").gameObject;
		//Start off with the inventory hidden.  
		StartCoroutine (ListenForHideShow());
	}

	IEnumerator ListenForHideShow() {
		while (true) {
			if (Input.GetKeyDown(KeyCode.M)) 
				inventory.SetActive(!inventory.activeSelf);
			yield return null;
		}
	}

}
