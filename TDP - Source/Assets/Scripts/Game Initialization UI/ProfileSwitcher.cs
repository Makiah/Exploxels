using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfileSwitcher : MonoBehaviour {

	void OnEnable() {
		UIEventManager.InitializeProfileSwitcher += InitializeProfileSwitcher;
	}

	void OnDisable() {
		UIEventManager.InitializeProfileSwitcher -= InitializeProfileSwitcher;
	}

	//0 means male, 1 means female.  
	[HideInInspector] public int currentGender = 0;
	[SerializeField] Sprite maleImage = null, femaleImage = null;
	Image currentProfile;

	void InitializeProfileSwitcher() {
		currentProfile = transform.FindChild ("Current Profile").GetComponent <Image> ();
		UpdateProfileSprite();
	}

	public void OnLeftButtonClick() {
		currentGender--;
		if (currentGender < 0) 
			currentGender = 1;
		UpdateProfileSprite ();
	}

	public void OnRightButtonClick() {
		currentGender++;
		if (currentGender > 1) 
			currentGender = 0;
		UpdateProfileSprite ();
	}

	void UpdateProfileSprite() {
		if (currentGender == 0) 
			currentProfile.sprite = maleImage;
		else 
			currentProfile.sprite = femaleImage;
	}

}
	