
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

	public void SetProjectileParametersWithAutomaticHeading(float velocity) {
		//Set physics of the projectile.  
		rb2d = GetComponent <Rigidbody2D> ();
		GameObject player = GameObject.Find ("PlayerReferenceObject");
		//Returned in radians.  
		float angleToPlayer = Mathf.Atan2 ((player.transform.position.y - transform.position.y) , (player.transform.position.x - transform.position.x));
		Debug.Log ("Angle to player was " + angleToPlayer);
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (angleToPlayer), velocity * Mathf.Sin (angleToPlayer));
		float degreeAngleToPlayer = ScriptingUtilities.RadiansToDegrees (angleToPlayer);
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToPlayer));


		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine ("DestroyIfDistanceFromPlayer");
	}

	//Beta stage: Should set a threshold via parameters that the arrow can shoot from, and randomly deviate from that position.  
	public void SetProjectileParametersWithAutomaticThreshold(float velocity, float currentHeading, float headingThreshold) {
		//Set physics of the projectile.  
		rb2d = GetComponent <Rigidbody2D> ();
		GameObject player = GameObject.Find ("PlayerReferenceObject");
		//Returned in radians.  
		float radianAngleToPlayer = Mathf.Atan2 ((player.transform.position.y - transform.position.y) , (player.transform.position.x - transform.position.x));
		Debug.Log ("Angle to player was " + radianAngleToPlayer);
		float degreeAngleToPlayer = ScriptingUtilities.RadiansToDegrees (radianAngleToPlayer);

		if (degreeAngleToPlayer >= currentHeading + headingThreshold) {
			degreeAngleToPlayer = currentHeading + headingThreshold;
		} else if (degreeAngleToPlayer <= currentHeading - headingThreshold) {
			degreeAngleToPlayer = currentHeading - headingThreshold;
		}

		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToPlayer));
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degreeAngleToPlayer)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degreeAngleToPlayer)));

		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine ("DestroyIfDistanceFromPlayer");
	}

	public void SetProjectileParametersWithAutomaticThresholdAndDeviation(float velocity, float currentHeading, float headingThreshold, float maxRandomDeviation) {
		//Set physics of the projectile.  
		rb2d = GetComponent <Rigidbody2D> ();
		GameObject player = GameObject.Find ("PlayerReferenceObject");
		//Returned in radians.  
		float radianAngleToPlayer = Mathf.Atan2 ((player.transform.position.y - transform.position.y) , (player.transform.position.x - transform.position.x));
		Debug.Log ("Angle to player was " + radianAngleToPlayer);
		float degreeAngleToPlayer = ScriptingUtilities.RadiansToDegrees (radianAngleToPlayer);
		
		if (degreeAngleToPlayer >= currentHeading + headingThreshold) {
			degreeAngleToPlayer = currentHeading + headingThreshold;
		} else if (degreeAngleToPlayer <= currentHeading - headingThreshold) {
			degreeAngleToPlayer = currentHeading - headingThreshold;
		}

		degreeAngleToPlayer += Random.Range (0, maxRandomDeviation) - maxRandomDeviation / 2;
		
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToPlayer));
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degreeAngleToPlayer)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degreeAngleToPlayer)));
		
		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine ("DestroyIfDistanceFromPlayer");
	}


	IEnumerator DestroyIfDistanceFromPlayer() {
		if (Vector3.Distance (transform.position, GameObject.Find ("PlayerReferenceObject").transform.position) >= destroyIfDistanceFromPlayer) {
			Destroy(this.gameObject);
		}
		yield return null;
	}

}
