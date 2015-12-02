
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class TigerScript : EnemyBaseActionClass {

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Tiger");
		base.SetReferences ();
		StopCoroutine ("BasicEnemyControl");
		StartCoroutine ("ControlTigerEmotions");
	}

	IEnumerator ControlTigerEmotions() {
		bool tigerAngry = false;
		StartCoroutine ("TigerPlacid");
		float initialViewableThreshold = 0;
		while (true) {
			if (Mathf.Abs(player.transform.position.x - transform.position.x) < playerViewableThreshold && ! tigerAngry) {
				moveForce *= 2;
				initialViewableThreshold = playerViewableThreshold;
				playerViewableThreshold = 2;
				tigerAngry = true;
				Debug.Log("Tiger angry");
			} else if (Mathf.Abs(player.transform.position.x - transform.position.x) >= playerViewableThreshold && tigerAngry) {
				moveForce /= 2;
				playerViewableThreshold = initialViewableThreshold;
				tigerAngry = false;
				Debug.Log("Tiger placid");
			}

			yield return null;
		}
	}

	protected override void Attack() {
		Debug.Log ("Tiger Attack!");
	}

}
