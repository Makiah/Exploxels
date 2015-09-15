
/*
 * Author: Makiah Bennett
 * Created 14 September 2015
 * Last edited: 14 September 2015
 * 
 * 9/14 - Created as a base to all enemies / player that will be affected by swords as well as other items.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterHealthController : MonoBehaviour {

	/************************* INITIALIZATION *************************/

	protected virtual void OnEnable() {
		EventManager.InitializeEnemies += InitializeHealthBar;
	}

	protected virtual void OnDisable() {
		EventManager.InitializeEnemies -= InitializeHealthBar;
	}

	/************************* HEALTH MANAGER *************************/
	
	public float lifePoints = 10f;

	public GameObject healthPanelPrefab;
	protected GameObject createdHealthPanelPrefab;
	protected Image characterHealthPanelImage;
	protected Slider characterHealthBar;

	protected Transform gameUI;

	public void YouHaveBeenAttacked(float lifePointDeduction) {
		Debug.Log ("Attack received " + lifePointDeduction);
		lifePoints -= lifePointDeduction;
		UpdateHealthBar ();
		if (lifePoints <= 0) {
			Destroy (createdHealthPanelPrefab);
			Destroy (this.gameObject);
		}
	}

	//Look into initializing this once the player comes into activation distance.  
	protected virtual void InitializeHealthBar() {
		//Create panel
		gameUI = GameObject.Find ("Inventory (UI)").transform; 
		RectTransform healthPanelPrefabProperties = healthPanelPrefab.GetComponent <RectTransform> ();
		createdHealthPanelPrefab = (GameObject)(Instantiate (healthPanelPrefab, Vector3.zero, Quaternion.identity));
		createdHealthPanelPrefab.transform.SetParent (gameUI, false);
		createdHealthPanelPrefab.GetComponent <RectTransform> ().anchoredPosition = healthPanelPrefabProperties.anchoredPosition;
		createdHealthPanelPrefab.GetComponent <RectTransform> ().sizeDelta = healthPanelPrefabProperties.sizeDelta;
		createdHealthPanelPrefab.transform.SetParent (gameUI, false);
		//Initialize icon
		Sprite characterHeadSprite = transform.GetChild (0).GetChild (0).FindChild ("Head").GetComponent <SpriteRenderer> ().sprite;
		characterHealthPanelImage = createdHealthPanelPrefab.transform.FindChild ("Icon").GetComponent <Image> ();
		characterHealthPanelImage.sprite = characterHeadSprite;
		//Initialize health bar
		characterHealthBar = createdHealthPanelPrefab.transform.FindChild ("Health Bar").GetComponent <Slider> ();
		characterHealthBar.maxValue = lifePoints;
		characterHealthBar.value = lifePoints;
		//So that they do not show up all at once.  
		createdHealthPanelPrefab.SetActive (false);

		if (GetComponent <EnemyBaseActionClass> () != null) {
			GetComponent <EnemyBaseActionClass>().enemyHealthBarInitialized = true;
		}
	}

	public void SetHealthBarActivation(bool activation) {
		createdHealthPanelPrefab.SetActive (activation);
	}

	void UpdateHealthBar() {
		characterHealthBar.value = lifePoints;
	}

}
