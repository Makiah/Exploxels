using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthController : CharacterHealthController {
	
	/************************* HEALTH MANAGER *************************/


	protected override void InitializeHealthBar() {
		currentHealth = lifePoints;
		//Create panel
		uiHealthController = GameObject.Find ("Inventory (UI)").transform.FindChild ("Health Controller").gameObject.GetComponent <UIHealthController> (); 
		healthPanelReference = uiHealthController.GetPlayerHealthPanelReference ();
		//Initialize icon
		characterHeadSprite = transform.GetChild (0).GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;

		healthPanelReference.Add (characterHeadSprite, lifePoints, currentHealth);

	}
}
