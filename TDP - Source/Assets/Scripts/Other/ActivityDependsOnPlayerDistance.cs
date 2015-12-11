using UnityEngine;
using System.Collections;

public class ActivityDependsOnPlayerDistance : MonoBehaviour {

	Transform player;
	[SerializeField] private float distanceRequirement;

	//Required to initialize.  
	public void StartPlayerDistanceChecking() {
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;

		//Start the coroutine.  
		StartCoroutine (ActivityIsDependentOnPlayerDistance());
	}

	//Works if the player is close enough.  
	IEnumerator ActivityIsDependentOnPlayerDistance() {
		bool gameObjectActive = true;

		while (true) {
			if (Mathf.Abs (transform.position.x - player.position.x) < distanceRequirement && gameObjectActive == false) {
				gameObject.SetActive (true);
				gameObjectActive = true;
				Debug.Log ("Set active: distance is " + Mathf.Abs (transform.position.x - player.position.x));
			} else if (Mathf.Abs (transform.position.x - player.position.x) >= distanceRequirement && gameObjectActive) {
				gameObject.SetActive (false);
				gameObjectActive = false;
				Debug.Log ("Set inactive: distance is " + Mathf.Abs (transform.position.x - player.position.x));
			}

			//Processing purposes.  
			yield return new WaitForSeconds(3);
		}
	}

}
