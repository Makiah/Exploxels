
/*
 * Author: Makiah Bennett
 * Last edited: 14 September 2015
 * 
 * 9/14 - Properties assigned pre-game.  
 * 
 * This class is only used when an object is instantiated by chopping a tree, mining a rock, etc.  The script looks for this component, and uses the
 * item to create a new UIResourceReference.   
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class DroppedItemProperties : MonoBehaviour {

	public bool working = false;

	[HideInInspector] public ResourceReference localResourceReference;
	private Transform player;


	public void Initialize() {
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		StartCoroutine (MoveTowardsPlayer());
	}

	IEnumerator MoveTowardsPlayer() {
		working = true;
		while (true) {
			if (Vector2.Distance(transform.position, player.transform.position) < 5)
				transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime / 3);

			yield return null;
		}
	}

}
