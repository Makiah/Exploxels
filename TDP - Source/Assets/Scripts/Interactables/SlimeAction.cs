
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script handles the AI of the slime.  The animation for the little guy is really cool, the transform of the upper body changes, 
 * while the lower body stays constant.  The body's scale changes.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class SlimeAction : MonoBehaviour {

	[System.Serializable]
	public class JumpParams {
		public float yMax;
		public float yMin;
		public float xMax;
		public float xMin;
	}

	//Maybe make this a parent class and create children for the ice slime and the green slime.  Projectile shooting?

	public GameObject player;
	public float attackDelay = 1f;
	public JumpParams jumpParams = new JumpParams();

	private Animator anim;
	private Rigidbody2D rb2d;
	private Transform groundCheck;

	void Awake() {
		groundCheck = transform.FindChild ("FlippingItem").FindChild ("Slime").FindChild ("GroundCheck");
		anim = transform.FindChild ("FlippingItem").FindChild ("Slime").GetComponent <Animator> ();
		rb2d = GetComponent <Rigidbody2D> ();
	}

	void Start() {
		StartCoroutine (AttackControl());
	}

	IEnumerator AttackControl() {
		while (true) {
			bool grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
			if (grounded) {
				anim.SetTrigger("Attack");

				float yForce = Random.Range(jumpParams.yMin, jumpParams.yMax);
				float xForce = Random.Range(jumpParams.xMin, jumpParams.xMax);

				if (Random.Range(0, 2) == 0)
					xForce *= -1;

				Vector2 vectorForce = new Vector2(xForce, yForce);

				yield return new WaitForSeconds(.5f);

				rb2d.AddForce(vectorForce);
			}

			yield return new WaitForSeconds (attackDelay);
		}
	}

}
