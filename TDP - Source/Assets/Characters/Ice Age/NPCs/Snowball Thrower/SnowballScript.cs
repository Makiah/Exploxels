using UnityEngine;
using System.Collections;

public class SnowballScript : MonoBehaviour {
	private GameObject playerObject;
	private bool notificationSent = false;
	[SerializeField] private int damageFactor = 5;

	//Initialize the snowball.  
	public void Initialize(Vector2 force) {
		playerObject = CurrentLevelVariableManagement.GetPlayerReference ();
		GetComponent <Rigidbody2D> ().AddForce (force);
		StartCoroutine (DestroyAfterTime (22));
	}

	//If the distance to the player is too large, then destroy the current game object.  
	IEnumerator DestroyAfterTime(float time) {
		yield return new WaitForSeconds (time);
		DestroySnowball ();
	}

	void OnTriggerEnter2D (Collider2D externalTrigger) {
		//Make sure that the second parent of the transform exists.  
		if (externalTrigger.transform.parent != null && externalTrigger.transform.parent.parent != null) {
			//Check to see whether it exists.  
			if (externalTrigger.transform.parent.parent.GetComponent <ICombatant> () != null && notificationSent == false) {
				//Damage the health panel.  
				externalTrigger.transform.parent.parent.GetComponent <CharacterHealthPanelManager> ().YouHaveBeenAttacked (damageFactor);
				notificationSent = true;

				//Knock the character backward.  
				externalTrigger.transform.parent.parent.GetComponent <ICombatant> ().ApplyKnockback(GetComponent <Rigidbody2D> ().velocity * 200);

				//Destroy the snowball
				DestroySnowball ();
			}
		}

	}

	void DestroySnowball() {
		//Do some snowball exploding animation here.  
		Destroy(this.gameObject);
	}
}
