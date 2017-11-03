
/*
 * Author: Makiah Bennett
 * Created 9/15
 * Last edited: 18 November 2015
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

	//Initialization
	void OnEnable() {
		LevelEventManager.InitializeUIHealthController += InitializeUIHealthController;
	}

	void OnDisable() {
		LevelEventManager.InitializeUIHealthController -= InitializeUIHealthController;
	}

	//Define Health controller components
	PlayerHealthPanelReference playerHealthPanel;
	HealthPanelReference enemyHealthPanel1, enemyHealthPanel2, enemyHealthPanel3;

	//Set references to the health panel references.  
	void InitializeUIHealthController() {
		playerHealthPanel = transform.Find ("Player Health Controller").Find ("HealthPanelPlayer").GetComponent <PlayerHealthPanelReference> ();
		enemyHealthPanel1 = transform.Find ("Enemy Health Controller").Find ("HealthPanel1").GetComponent <HealthPanelReference> ();
		enemyHealthPanel2 = transform.Find("Enemy Health Controller").Find("HealthPanel2").GetComponent <HealthPanelReference> ();
		enemyHealthPanel3 = transform.Find ("Enemy Health Controller").Find ("HealthPanel3").GetComponent <HealthPanelReference> ();	
	}
	
	public HealthPanelReference GetEnemyHealthPanelReference() {
		return GetBestAvailableEnemyHealthPanelReference ();
	}

	public PlayerHealthPanelReference GetPlayerHealthPanelReference () {
		return playerHealthPanel;
	}

	//Choose the best available health panel reference (in order of 1-3)
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
