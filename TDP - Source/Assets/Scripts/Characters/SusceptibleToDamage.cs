
/*
 * Author: Makiah Bennett
 * Created 14 September 2015
 * Last edited: 14 September 2015
 * 
 * 9/14 - Created as a base to all enemies / player that will be affected by swords as well as other items.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public abstract class SusceptibleToDamage : MonoBehaviour {

	int hits = 0;
	public int hitsUntilDeath = 3;

	public void YouHaveBeenAttacked() {
		Debug.Log ("Attack recognized on " + gameObject.name);
		hits++;
		if (hits >= hitsUntilDeath) 
			Destroy (this.gameObject);
	}

}
