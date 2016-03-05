using UnityEngine;
using System.Collections;

public class EnemyExpDropper : DropsItems {

	public void OnEnemyDeath() {
		DropItems ();
	}

}
