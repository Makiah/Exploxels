using UnityEngine;
using System.Collections;

public class ActivateParticleEffectDependingOnPlayerDistance : MonoBehaviour {

	Transform player;
	ParticleSystem mainParticleSystem;
	[SerializeField] private float distanceRequirement = 0;

	//Required to initialize.  
	public void StartPlayerDistanceChecking() {
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		mainParticleSystem = GetComponent <ParticleSystem> ();

		//Start the coroutine.  
		StartCoroutine (ActivityIsDependentOnPlayerDistance());
	}

	//Works if the player is close enough.  
	IEnumerator ActivityIsDependentOnPlayerDistance() {
		while (true) {
			//Check the distance to the player, then clear the particle effect or play the particle effect depending on whether the player is close enough.  
			if (Mathf.Abs (transform.position.x - player.position.x) < distanceRequirement && mainParticleSystem.isStopped) {
				mainParticleSystem.Play ();
			} else if (Mathf.Abs (transform.position.x - player.position.x) >= distanceRequirement && mainParticleSystem.isPlaying) {
				mainParticleSystem.Stop ();
				mainParticleSystem.Clear ();
			}

			//Processing purposes.  
			yield return new WaitForSeconds(0.5f);
		}
	}

}
