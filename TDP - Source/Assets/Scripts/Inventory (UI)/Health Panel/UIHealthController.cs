
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
		LevelEventManager.InitializeUIHealthController += InitializeUIHealthController;
	}

	void OnDisable() {
		LevelEventManager.InitializeUIHealthController -= InitializeUIHealthController;
	}

	PlayerHealthPanelReference playerHealthPanel;
	HealthPanelReference enemyHealthPanel1, enemyHealthPanel2, enemyHealthPanel3;

	List <CharacterHealthPanelManager> pendingHealthControllerList = new List<CharacterHealthPanelManager>();

	void InitializeUIHealthController() {
		playerHealthPanel = new PlayerHealthPanelReference(transform.FindChild("Player Health Controller").FindChild("HealthPanelPlayer").transform, this);
		enemyHealthPanel1 = new HealthPanelReference(transform.FindChild("Enemy Health Controller").FindChild("HealthPanel1").transform, this);
		enemyHealthPanel2 = new HealthPanelReference(transform.FindChild("Enemy Health Controller").FindChild("HealthPanel2").transform, this);
		enemyHealthPanel3 = new HealthPanelReference(transform.FindChild("Enemy Health Controller").FindChild("HealthPanel3").transform, this);
		playerHealthPanel.Clear ();
		enemyHealthPanel1.Clear ();
		enemyHealthPanel2.Clear ();
		enemyHealthPanel3.Clear ();
	}

	public HealthPanelReference GetEnemyHealthPanelReference(CharacterHealthPanelManager someHealthController) {
		return GetBestAvailableEnemyHealthPanelReference (someHealthController);
	}

	public PlayerHealthPanelReference GetPlayerHealthPanelReference () {
		return playerHealthPanel;
	}

	public void OnHealthPanelReset() {
		Debug.Log ("OnHealthPanelReset called");
		if (pendingHealthControllerList.Count > 0) {
			if (pendingHealthControllerList [0] != null) {
				pendingHealthControllerList [0].HealthPanelNewlyAvailable (GetBestAvailableEnemyHealthPanelReference (pendingHealthControllerList [0]));
				Debug.Log(pendingHealthControllerList[0].gameObject.name + " has been given a health panel");
				pendingHealthControllerList.RemoveAt (0);
			}
		}
	}

	HealthPanelReference GetBestAvailableEnemyHealthPanelReference(CharacterHealthPanelManager someHealthController) {
		Debug.Log (someHealthController.gameObject.name + " is accessing the uihealthcontroller");
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
