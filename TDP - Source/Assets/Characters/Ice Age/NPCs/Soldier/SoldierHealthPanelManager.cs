using UnityEngine;
using System.Collections;

public class SoldierHealthPanelManager : CharacterHealthPanelManager {

	// On player/enemy attacked.  
	public override void YouHaveBeenAttacked(float lifePointDeduction) {
		Debug.Log ("Attacked, changing from NPC to enemy.");
		base.YouHaveBeenAttacked (lifePointDeduction);

		//Enable enemy behaviour and disable npc behaviour (Enrage the soldier).  
		GetComponent <SoldierNPCBehaviour> ().DisableNPCActions ();
		GetComponent <SoldierEnemyBehaviour> ().EnableEnemyBehaviour ();

		//StartCoroutine (ReturnToNPCAfterSeconds (15));
	}

	IEnumerator ReturnToNPCAfterSeconds(float seconds) {
		yield return new WaitForSeconds (seconds);
		GetComponent <SoldierEnemyBehaviour> ().DisableEnemyBehaviour ();
		GetComponent <SoldierNPCBehaviour> ().EnableNPCActions ();
	}

}
