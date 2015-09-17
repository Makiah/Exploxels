
/*
 * Author: Makiah Bennett
 * Created 14 September 2015
 * Last edited: 14 September 2015
 * 
 * 9/14 - Created.  Should have a constructor with velocity, heading, and other projectile properties.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	private Rigidbody2D rb2d;
	public float destroyIfDistanceFromPlayer = 20f;
	
	private bool notificationSent = false;

	private float arrowPower;

	GameObject playerObject;

	public void InitializeProjectileWithThresholdAndDeviation(Vector3 positionToFireToward, float velocity, float currentHeading, float headingThreshold, float maxRandomDeviation, float ctorArrowPower) {
		playerObject = GameObject.Find ("ManagementFrameworks").transform.FindChild ("GameVariables").gameObject.GetComponent <VariableManagement> ().GetPlayerReference ();

		//Set physics of the projectile.  
		rb2d = GetComponent <Rigidbody2D> ();
		//Returned in radians.  
		float radianAngleToPlayer = Mathf.Atan2 ((positionToFireToward.y - transform.position.y) , (positionToFireToward.x - transform.position.x));
		float degreeAngleToPlayer = ScriptingUtilities.RadiansToDegrees (radianAngleToPlayer);

		//Used to set the threshold angles that the arrow can be shot at.  
		if (currentHeading == 0) {
			if (180 >= degreeAngleToPlayer && degreeAngleToPlayer >= headingThreshold) 
				degreeAngleToPlayer = headingThreshold;
			else if (-180 <= degreeAngleToPlayer && degreeAngleToPlayer <= -headingThreshold) 
				degreeAngleToPlayer = -headingThreshold;
		} else if (currentHeading == 180) {
			if (0 <= degreeAngleToPlayer && degreeAngleToPlayer <= 180 - headingThreshold) 
				degreeAngleToPlayer = 180 - headingThreshold;
			else if (0 >= degreeAngleToPlayer && degreeAngleToPlayer >= -180 + headingThreshold)
				degreeAngleToPlayer = -180 + headingThreshold;
		}

		degreeAngleToPlayer += Random.Range (0, maxRandomDeviation) - maxRandomDeviation / 2;
		
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToPlayer));
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degreeAngleToPlayer)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degreeAngleToPlayer)));

		arrowPower = ctorArrowPower;

		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine ("DestroyIfDistanceFromPlayer");
	}


	IEnumerator DestroyIfDistanceFromPlayer() {
		while (true) {
			if (Vector3.Distance (transform.position, playerObject.transform.position) >= destroyIfDistanceFromPlayer) {
				Destroy (this.gameObject);
			}
			yield return null;
		}
	}

	void OnTriggerEnter2D (Collider2D externalTrigger) {

		if (externalTrigger.gameObject.GetComponent <CharacterHealthController> () != null && notificationSent == false) {
			externalTrigger.gameObject.GetComponent <CharacterHealthController> ().YouHaveBeenAttacked (arrowPower);
			notificationSent = true;
		}

	}

	void OnTriggerExit2D () {
		if (notificationSent) 
			notificationSent = false;
	}

}
