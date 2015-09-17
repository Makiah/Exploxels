
/*
 * Author: Makiah Bennett
 * Created September 15
 * Last edited: 15 September 2015 
 * 
 * This script should be used as a reference to the health bars displayed on-screen, mainly for the UIHealthController script.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthPanelReference {

	GameObject panel;
	Image headIcon;
	Slider healthBar;

	bool occupied = false;

	UIHealthController masterController;

	public HealthPanelReference(Transform objectToReference, UIHealthController ctorMasterController) {
		panel = objectToReference.gameObject;
		headIcon = objectToReference.FindChild ("Icon").gameObject.GetComponent <Image> ();
		healthBar = objectToReference.FindChild ("Health Bar").gameObject.GetComponent <Slider> ();
		masterController = ctorMasterController;
	}

	public bool IsEmpty() {
		return !occupied;
	}

	public void Reset() {
		Clear ();
		masterController.OnHealthPanelReset ();
	}

	public void Clear() {
		headIcon.sprite = null;
		healthBar.maxValue = 1;
		healthBar.value = 0;
		occupied = false;
		panel.SetActive (false);
		Debug.Log ("Panel was cleared");
	}

	public void Add(Sprite image, float totalHealth, float currentHealth) {
		headIcon.sprite = image;
		healthBar.maxValue = totalHealth;
		healthBar.value = currentHealth;
		Debug.Log("Set panel to " + image.name);
		occupied = true;
		panel.SetActive (true);
		Debug.Log ("Panel was added of " + image.name);
	}

	public void Update(float currentHealth) {
		if (headIcon.sprite != null) 
			healthBar.value = currentHealth;
		else
			Debug.Log ("Cannot update empty health panel");
	}

}
