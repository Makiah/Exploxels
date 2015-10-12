
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
		animator.gameObject.transform.parent.parent.gameObject.GetComponent <CharacterBaseActionClass> ().ResetCurrentAttackAnimationState ();
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
