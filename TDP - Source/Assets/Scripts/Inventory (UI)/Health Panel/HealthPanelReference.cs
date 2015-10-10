
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

	protected GameObject panel;
	protected Image headIcon;
	protected Slider healthBar;
	protected Image healthBarFillImage;

	bool occupied = false;

	protected UIHealthController masterController;

	public HealthPanelReference(Transform objectToReference, UIHealthController ctorMasterController) {
		panel = objectToReference.gameObject;
		headIcon = objectToReference.FindChild ("Icon").gameObject.GetComponent <Image> ();
		healthBar = objectToReference.FindChild ("Health Bar").gameObject.GetComponent <Slider> ();
		masterController = ctorMasterController;
		healthBarFillImage = healthBar.gameObject.transform.FindChild ("Fill Area").FindChild ("Fill").GetComponent <Image> ();
	}

	public bool IsEmpty() {
		return !occupied;
	}

	//Called when a panel is reset (i.e. OnDeath()).  
	public void Reset() {
		Clear ();
		masterController.OnHealthPanelReset ();
	}

	//Called when a panel is cleared.  
	public void Clear() {
		headIcon.sprite = null;
		healthBar.maxValue = 1;
		healthBar.value = 0;
		occupied = false;
		panel.SetActive (false);
		Debug.Log ("Panel was cleared");
	}

	//Called by CharacterHealthPanelManager when it is given a panel.  
	public void InitializePanel(Sprite image, float totalHealth, float currentHealth) {
		headIcon.sprite = image;
		healthBar.maxValue = totalHealth;
		healthBar.value = currentHealth;
		occupied = true;
		panel.SetActive (true);
		UpdateColor ();
	}

	//Used when a potion is added or object is attacked (called by CharacterHealthPanelManager).  
	public virtual void UpdateHealth(float currentHealth) {
		if (headIcon.sprite != null) {
			healthBar.value = currentHealth;
			UpdateColor();
		}
		else
			Debug.Log ("Cannot update empty health panel");
	}

	protected void UpdateColor() {
		if (healthBar.value / healthBar.maxValue < 0.4f) {
			healthBarFillImage.color = new Color (1, 0, 0);
		} else {
			healthBarFillImage.color = new Color(0, 1, 0);
		}
	}

}
