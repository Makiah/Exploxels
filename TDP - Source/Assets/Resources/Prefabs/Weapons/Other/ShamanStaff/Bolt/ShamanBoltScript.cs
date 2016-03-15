
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

public class ShamanBoltScript : ProjectileScript {
	
	ParticleSystem trail;

	public override void InitializeProjectileWithThresholdAndDeviation(Vector3 positionToFireToward, float velocity, float currentHeading, float headingThreshold, float maxRandomDeviation, float ctorArrowPower) {
		base.InitializeProjectileWithThresholdAndDeviation (positionToFireToward, velocity, currentHeading, headingThreshold, maxRandomDeviation, ctorArrowPower);

		trail = transform.FindChild ("Particle System").GetComponent <ParticleSystem> ();

		StartCoroutine (ShrinkBolt ());
	}

	//A different thing from the projectile.  
	IEnumerator ShrinkBolt() {
		float timeMultiplier = 1;
		float startTime = Time.time;
		Vector3 initialVelocity = rb2d.velocity;
		float initialLifespan = trail.startLifetime;
		float initialPower = power;
		Debug.Log ("Started bolt shrinking");
		while (timeMultiplier > .05f) {
			//Update time multiplier.  
			timeMultiplier = 1 - ((Time.time - startTime) / 1.5f);

			//Shrinking
			transform.localScale = new Vector3 (timeMultiplier, timeMultiplier, timeMultiplier);

			//Slowing down
			rb2d.velocity = initialVelocity * timeMultiplier;

			//Reducing Particle System Lifespan (doesn't seem to do anything, although the lifespan is decreasing)
			trail.startLifetime = timeMultiplier * initialLifespan;

			//Reduce damage 
			power = (int) (timeMultiplier * initialPower);

			yield return null;
		}

		//Destroy the object.  
		Destroy (gameObject);
	}
}
