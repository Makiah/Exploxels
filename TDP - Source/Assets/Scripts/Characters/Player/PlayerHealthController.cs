
/*
 * Author: Makiah Bennett
 * Created 16 September 2015
 * Last edited: 18 September 2015
 * 
 * 9/18 - Created override to OnDeath, so that app quits on player death.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthController : CharacterHealthController {
	
	/************************* HEALTH MANAGER *************************/
	
	protected override void InitializeHealthBar() {
		lifePoints = 10f;
		currentHealth = lifePoints;
		//Create panel
		uiHealthController = GameObject.Find ("Inventory (UI)").transform.FindChild ("Health Controller").gameObject.GetComponent <UIHealthController> (); 
		healthPanelReference = uiHealthController.GetPlayerHealthPanelReference ();
		//Initialize icon
		characterHeadSprite = transform.GetChild (0).GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;

		healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);

	}

	protected override void OnDeath() {
		Debug.Log ("Player OnDeath called");
		healthPanelReference.Clear ();
		//Note: Application.Quit() does not work for the Web Player or the Unity Editor.  
		Application.Quit ();
		//The following does work for the editor.  
		//UnityEditor.EditorApplication.isPlaying = false;
		Destroy (this.gameObject);
	}
}
