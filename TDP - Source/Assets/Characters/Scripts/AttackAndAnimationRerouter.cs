using UnityEngine;
using System.Collections;

//This class is here mainly to bypass the limitations of the animation event system in Unity (has to be called on the same GameObject).  
public class AttackAndAnimationRerouter : MonoBehaviour {

	//The character that this is currently attached to.  
	ICombatant character;

	public void OnAttack() {
		if (character == null) {
			character = transform.parent.parent.GetComponent <ICombatant> ();
		}

		character.OnAttack ();
	}

	public void OnAttackAnimationCompleted() {
		if (character == null) {
			character = transform.parent.parent.GetComponent <ICombatant> ();
		}

		character.OnAttackAnimationCompleted ();
	}
}
