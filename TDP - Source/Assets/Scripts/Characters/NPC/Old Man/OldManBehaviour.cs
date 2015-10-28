using UnityEngine;
using System.Collections;

public class OldManBehaviour : NPCBaseScript {

	protected override void SetReferences() {
		base.SetReferences ();
		StartCoroutine ("WalkAroundAimlessly");
	}

	IEnumerator WalkAroundAimlessly() {
		while (true) {
			anim.SetFloat("Speed", 1);
			rb2d.velocity = new Vector2(GetFacingDirection() * moveForce * -1, 0);
			yield return new WaitForSeconds(3f);
			anim.SetFloat("Speed", 0);
			rb2d.velocity = Vector2.zero;
			yield return new WaitForSeconds(3f);
			Flip ();

			yield return null;
		}
	}

}
