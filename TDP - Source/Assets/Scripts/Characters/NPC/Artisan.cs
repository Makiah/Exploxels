using UnityEngine;
using System.Collections;

public abstract class Artisan : NPCBaseScript {

	//The local position should be (0, 0) if the artisan is directly at the location.  
	protected override IEnumerator WalkAround() {
		while (true) {
			if (transform.localPosition.x > 0) {
				if (GetFacingDirection() == 1)
					Flip ();
				Debug.Log("Chose scenario 1");
			}
			else if (transform.localPosition.x <= 0) {
				if (GetFacingDirection() == -1)
					Flip ();
			}

			anim.SetFloat("Speed", 1);
			rb2d.velocity = new Vector2(GetFacingDirection() * moveForce, rb2d.velocity.y);

			yield return new WaitForSeconds(3f);

			anim.SetFloat("Speed", 0);
			rb2d.velocity = Vector2.zero;

			yield return new WaitForSeconds(3f);
		}
	}

}
