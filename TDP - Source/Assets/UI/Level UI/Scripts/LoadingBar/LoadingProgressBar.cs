using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour {

	Text description;
	Slider progress;

	//Used to initialize a new action (by level event manager)
	public void InitializeNewAction(float fractionCompleted, string descriptionOfAction) {
		CheckInitializationState ();
		//Set values of each.  
		progress.value = fractionCompleted;
		description.text = descriptionOfAction;
	}

	void CheckInitializationState() {
		//They are either both initialized, or both not initialized.  
		if (progress == null && description == null) {
			progress = transform.FindChild ("Progress").GetComponent <Slider> ();
			description = transform.FindChild ("Description").GetComponent <Text> ();
		}
	}
}
