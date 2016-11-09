using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeState : StateMachineBehaviour {

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.transform.GetComponent<CircleCollider2D>().radius = animator.transform.GetComponent<Swipe>().AttackRange;
        animator.transform.GetComponent<Collider2D>().enabled = true;
        AudioSource.PlayClipAtPoint(animator.transform.GetComponent<Swipe>().SFX, animator.transform.position, GameManager.SFX_Volume);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime >= 0.5) {
            animator.transform.GetComponent<Collider2D>().enabled = false;
            Stack<Collider2D> HittedStack = animator.transform.GetComponent<Swipe>().HittedStack;
            if (HittedStack.Count != 0) {
                HittedStack.Clear();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
