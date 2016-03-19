using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierHealthPanelManager : CharacterHealthPanelManager {

	//The variables that will be visible in the inspector.  
	[SerializeField] private float reversionToNPCTime = 15;
	[SerializeField] private float rangeOfEnrage = 15;

	private bool running = false;

	private static List <SoldierHealthPanelManager> soldierInstances = new List <SoldierHealthPanelManager> ();

	private float timer = 0;

	//Add the current class instance to the list to be enraged later.  
	public override void InitializeHealthBar() {
		base.InitializeHealthBar ();
		soldierInstances.Add (this);
	}

	// On player/enemy attacked.  
	public override void YouHaveBeenAttacked(float lifePointDeduction) {
		Debug.Log ("Attacked, changing from NPC to enemy.");
		base.YouHaveBeenAttacked (lifePointDeduction);

		//Enrage all soldiers within range (Includes self).  
		for (int i = 0; i < soldierInstances.Count; i++) {
			if (Vector2.Distance (soldierInstances [i].transform.position, transform.position) <= rangeOfEnrage) {
				soldierInstances [i].Enrage ();
			}
		}
	}

	//Enrages the soldier.  
	public void Enrage() {
		//Enable enemy behaviour and disable npc behaviour (Enrage the soldier).  
		GetComponent <SoldierNPCBehaviour> ().DisableNPCActions ();
		GetComponent <SoldierEnemyBehaviour> ().EnableEnemyBehaviour ();

		if (running) {
			//Reset the timer
			Debug.Log("Restarted timer");
			timer = 0;
		} else {
			Debug.Log ("Started new coroutine");
			StartCoroutine (ReturnToNPCAfterSeconds(reversionToNPCTime));
		}

		running = true;
	}

	void CalmDown() {
		GetComponent <SoldierEnemyBehaviour> ().DisableEnemyBehaviour ();
		GetComponent <SoldierNPCBehaviour> ().EnableNPCActions ();
		running = false;

		Debug.Log ("Soldier reverted to NPC");
	}

	//Return the enemy soldier to an NPC after some period of time.  
	private IEnumerator ReturnToNPCAfterSeconds(float seconds) {
		while (timer <= seconds) {
			//Debug.Log ("Reverting to NPC in " + (seconds - timer));
			timer += Time.deltaTime;
			yield return null;
		}

		timer = 0;
		running = false;

		CalmDown ();
	}

	//Remove the entry from the list.  
	protected override void OnDeath() {
		soldierInstances.Remove (this);
		base.OnDeath ();
	}

}
