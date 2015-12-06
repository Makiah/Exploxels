
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
		playerObject = CurrentLevelVariableManagement.GetPlayerReference ();

		//Set physics of the projectile.  
		rb2d = GetComponent <Rigidbody2D> ();
		//Returned in radians.  
		float radianAngleToTarget = Mathf.Atan2 ((positionToFireToward.y - transform.position.y) , (positionToFireToward.x - transform.position.x));
		float degreeAngleToTarget = ScriptingUtilities.RadiansToDegrees (radianAngleToTarget);

		//Used to set the threshold angles that the arrow can be shot at.  
		if (currentHeading == 0) {
			if (180 >= degreeAngleToTarget && degreeAngleToTarget >= headingThreshold) 
				degreeAngleToTarget = headingThreshold;
			else if (-180 <= degreeAngleToTarget && degreeAngleToTarget <= -headingThreshold) 
				degreeAngleToTarget = -headingThreshold;
		} else if (currentHeading == 180) {
			if (0 <= degreeAngleToTarget && degreeAngleToTarget <= 180 - headingThreshold) 
				degreeAngleToTarget = 180 - headingThreshold;
			else if (0 >= degreeAngleToTarget && degreeAngleToTarget >= -180 + headingThreshold)
				degreeAngleToTarget = -180 + headingThreshold;
		}

		degreeAngleToTarget += Random.Range (0, maxRandomDeviation) - maxRandomDeviation / 2;
		
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToTarget));
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degreeAngleToTarget)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degreeAngleToTarget)));

		arrowPower = ctorArrowPower;

		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine (DestroyIfDistanceFromPlayer());
	}


	IEnumerator DestroyIfDistanceFromPlayer() {
		while (true) {
			if (Vector2.Distance (transform.position, playerObject.transform.position) >= destroyIfDistanceFromPlayer) {
				Destroy (this.gameObject);
			}
			yield return null;
		}
	}

	void OnTriggerEnter2D (Collider2D externalTrigger) {

		if (externalTrigger.gameObject.GetComponent <CharacterHealthPanelManager> () != null && notificationSent == false) {
			externalTrigger.gameObject.GetComponent <CharacterHealthPanelManager> ().YouHaveBeenAttacked (arrowPower);
			notificationSent = true;
			Destroy(this.gameObject);
		}

	}

}
