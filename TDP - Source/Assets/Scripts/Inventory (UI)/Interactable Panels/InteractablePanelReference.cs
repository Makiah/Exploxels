using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractablePanelReference : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeInteractablePanels += InitializeInteractablePanel;
	}

	void OnDisable() {
		LevelEventManager.InitializeInteractablePanels -= InitializeInteractablePanel;
	}

	GameObject panel;
	Image headIcon;
	Text message;

	bool occupied = false;

	//Initialize required components of child objects.  
	void InitializeInteractablePanel() {
		panel = gameObject;
		headIcon = transform.FindChild ("Icon").GetComponent <Image> ();
		message = transform.FindChild ("Message").GetComponent <Text> ();
		Clear ();
	}

	public bool IsEmpty() {
		return !occupied;
	}
	
	//Called when a panel is cleared.  
	public void Clear() {
		headIcon.sprite = null;
		message.text = "";
		occupied = false;
		panel.SetActive (false);
	}
	
	//Called by CharacterHealthPanelManager when it is given a panel.  
	public void InitializePanel(Sprite image, string text) {
		headIcon.sprite = image;
		message.text = text;
		occupied = true;
		panel.SetActive (true);
	}

}
