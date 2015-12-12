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
			if (Mathf.Abs (transform.position.x - player.position.x) < distanceRequirement && mainParticleSystem.isStopped) {
				mainParticleSystem.Play ();
				Debug.Log ("Set active: distance is " + Mathf.Abs (transform.position.x - player.position.x));
			} else if (Mathf.Abs (transform.position.x - player.position.x) >= distanceRequirement && mainParticleSystem.isPlaying) {
				mainParticleSystem.Stop ();
				mainParticleSystem.Clear ();
				Debug.Log ("Set inactive: distance is " + Mathf.Abs (transform.position.x - player.position.x));
			}

			//Processing purposes.  
			yield return new WaitForSeconds(0.5f);
		}
	}

}
