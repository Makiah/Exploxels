
/*
 * Author: Makiah Bennett
 * Created 9/15
 * Last edited: 15 September 2015
 * 
 * 
 * This script controls the health bars and icons of the UI, and includes a public function for adding enemies to the system.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIHealthController : MonoBehaviour {

	void OnEnable() {
		EventManager.EnableHealthBars += InitializeHealthControllers;
	}

	void OnDisable() {
		EventManager.EnableHealthBars -= InitializeHealthControllers;
	}


	HealthPanelReference playerHealthPanel, enemyHealthPanel1, enemyHealthPanel2, enemyHealthPanel3;

	List <CharacterHealthController> pendingHealthControllerList = new List<CharacterHealthController>();

	void InitializeHealthControllers() {
		playerHealthPanel = new HealthPanelReference(transform.FindChild("Player Health Controller").FindChild("HealthPanelPlayer").transform, this);
		enemyHealthPanel1 = new HealthPanelReference(transform.FindChild("Enemy Health Controller").FindChild("HealthPanel1").transform, this);
		enemyHealthPanel2 = new HealthPanelReference(transform.FindChild("Enemy Health Controller").FindChild("HealthPanel2").transform, this);
		enemyHealthPanel3 = new HealthPanelReference(transform.FindChild("Enemy Health Controller").FindChild("HealthPanel3").transform, this);
		playerHealthPanel.Clear ();
		enemyHealthPanel1.Clear ();
		enemyHealthPanel2.Clear ();
		enemyHealthPanel3.Clear ();
	}

	public HealthPanelReference GetEnemyHealthPanelReference(CharacterHealthController someHealthController) {
		return GetBestAvailableEnemyHealthPanelReference (someHealthController);
	}

	public HealthPanelReference GetPlayerHealthPanelReference () {
		return playerHealthPanel;
	}

	public void OnHealthPanelReset() {
		if (pendingHealthControllerList.Count > 0) {
			if (pendingHealthControllerList [0] != null) {
				pendingHealthControllerList [0].HealthBarNowAvailable (GetBestAvailableEnemyHealthPanelReference (pendingHealthControllerList [0]));
				pendingHealthControllerList.RemoveAt (0);
			}
		}
	}

	HealthPanelReference GetBestAvailableEnemyHealthPanelReference(CharacterHealthController someHealthController) {
		//Debug.Log (someHealthController.gameObject.name + " is accessing the script");
		if (enemyHealthPanel1.IsEmpty ())
			return enemyHealthPanel1;
		else if (enemyHealthPanel2.IsEmpty ())
			return enemyHealthPanel2;
		else if (enemyHealthPanel3.IsEmpty ())
			return enemyHealthPanel3;
		else {
			Debug.Log ("No available enemy health panel, adding to waiting list");
			pendingHealthControllerList.Add(someHealthController);
			return null;
		}
	}

}
