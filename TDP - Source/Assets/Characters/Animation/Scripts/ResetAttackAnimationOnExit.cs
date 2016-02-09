
/*
 * Author: Makiah Bennett
 * Last edited: 14 September 2015
 * 
 * 9/14 - Added functionality to reset attack animation for any character, not just the player.  
 * 
 * This script is fairly straightforward.  Once the animation has completed doing whatever it was doing, the animation proceeds to reset 
 * the boolean that prevents actions after chopping a tree, slashing, etc.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class ResetAttackAnimationOnExit : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		//Simple solution.  Get ICombatant mainly because of the soldier.  
		animator.gameObject.GetComponent <AttackAndAnimationRerouter> ().OnAttackAnimationCompleted();

		//This works, but seems like overkill.  
		/*
		//Set the initial transform.  
		Transform currentTransform;
		currentTransform = animator.gameObject.transform;
		//C# works with short-circuiting, which means that if the first condition is false, it will break.  
		while (currentTransform != null) {
			//Exit the loop if it finds the required script
			if (currentTransform.GetComponent <CharacterBaseActionClass> () != null) {
				//Exit the loop
				break;
			}
			//Go to the next available transform.  
			currentTransform = currentTransform.parent;
		}

		//Call the animation completed on the caller.  
		if (currentTransform.GetComponent <CharacterBaseActionClass> () != null) {
			Debug.Log ("Found character base action class");
			currentTransform.GetComponent <CharacterBaseActionClass> ().ResetCurrentAttackAnimationState ();
		} else {
			Debug.LogError ("Could not find character base action class!");
		}
		*/
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
