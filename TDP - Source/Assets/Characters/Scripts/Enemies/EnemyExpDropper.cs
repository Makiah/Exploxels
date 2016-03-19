using UnityEngine;
using System.Collections;

public class EnemyExpDropper : DropsItems {

	public void InitializeExpDropper() {
		MakeReferences ();
	}

	public void OnEnemyDeath() {
		DropItems ();
	}

}
