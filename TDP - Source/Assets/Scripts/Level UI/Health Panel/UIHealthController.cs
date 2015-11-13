
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

public class UIHealthController : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeUIHealthController += InitializeUIHealthController;
	}

	void OnDisable() {
		LevelEventManager.InitializeUIHealthController -= InitializeUIHealthController;
	}

	PlayerHealthPanelReference playerHealthPanel;
	HealthPanelReference enemyHealthPanel1, enemyHealthPanel2, enemyHealthPanel3;

	void InitializeUIHealthController() {
		playerHealthPanel = transform.FindChild ("Player Health Controller").FindChild ("HealthPanelPlayer").GetComponent <PlayerHealthPanelReference> ();
		enemyHealthPanel1 = transform.FindChild ("Enemy Health Controller").FindChild ("HealthPanel1").GetComponent <HealthPanelReference> ();
		enemyHealthPanel2 = transform.FindChild("Enemy Health Controller").FindChild("HealthPanel2").GetComponent <HealthPanelReference> ();
		enemyHealthPanel3 = transform.FindChild ("Enemy Health Controller").FindChild ("HealthPanel3").GetComponent <HealthPanelReference> ();	
	}

	public HealthPanelReference GetEnemyHealthPanelReference() {
		return GetBestAvailableEnemyHealthPanelReference ();
	}

	public PlayerHealthPanelReference GetPlayerHealthPanelReference () {
		return playerHealthPanel;
	}

	HealthPanelReference GetBestAvailableEnemyHealthPanelReference() {
		if (enemyHealthPanel1.IsEmpty ())
			return enemyHealthPanel1;
		else if (enemyHealthPanel2.IsEmpty ())
			return enemyHealthPanel2;
		else if (enemyHealthPanel3.IsEmpty ())
			return enemyHealthPanel3;
		else {
			return null;
		}
	}

}
