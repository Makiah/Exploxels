
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

 	public void SetProjectileParameters (float velocity, float headingDegrees) {
		rb2d = GetComponent <Rigidbody2D> ();
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians (headingDegrees)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians (headingDegrees)));
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, headingDegrees));
	}

	public void SetProjectileParametersWithAutomaticHeading(float velocity) {
		rb2d = GetComponent <Rigidbody2D> ();
		GameObject player = GameObject.Find ("PlayerReferenceObject");
		float angleToPlayer = Mathf.Atan2 ((player.transform.position.y - transform.position.y) , (player.transform.position.x - transform.position.x));
		Debug.Log ("Angle to player was " + angleToPlayer);
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (angleToPlayer), velocity * Mathf.Sin (angleToPlayer));
		float degreeAngleToPlayer = ScriptingUtilities.RadiansToDegrees (angleToPlayer);
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToPlayer));
	}

	IEnumerator DestroyIfDistanceFromPlayer() {
		if (Vector3.Distance (transform.position, GameObject.Find ("PlayerReferenceObject").transform.position) >= destroyIfDistanceFromPlayer) {
			Destroy(this.gameObject);
		}
		yield return null;
	}

}
