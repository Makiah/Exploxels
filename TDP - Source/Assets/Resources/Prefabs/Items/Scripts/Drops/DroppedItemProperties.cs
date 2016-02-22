
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

	[HideInInspector] public ResourceReferenceWithStack localResourceReference;
	private Transform player;


	public void Initialize() {
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;
		StartCoroutine (MoveTowardsPlayer());
	}

	IEnumerator MoveTowardsPlayer() {
		while (true) {
			if (Mathf.Abs(player.transform.position.x - transform.position.x) < 5) {
				if (player.transform.position.x > transform.position.x)
					transform.position += new Vector3(0.02f, 0, 0);
				else 
					transform.position += new Vector3(-0.02f, 0, 0);
			}

			yield return null;
		}
	}

}
