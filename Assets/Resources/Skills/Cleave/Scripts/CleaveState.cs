using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleaveState : StateMachineBehaviour {
    float box_width = 0.35f;
    float box_height = 0.8f;
    float offSet = 0.15f;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float RangeScale = animator.transform.GetComponent<Cleave>().RangeScale;
        BoxCollider2D collider = animator.transform.GetComponent<BoxCollider2D>();
        Transform T_Cleave = animator.transform;
        if (stateInfo.IsName("cleave_left")) {
            collider.size = new Vector2(box_width, box_height);
            //T_Cleave.transform.localPosition = new Vector3(-offSet * RangeScale, 0, 0);
            collider.offset = new Vector2(-offSet, 0);
        } else if (stateInfo.IsName("cleave_right")) {
            collider.size = new Vector2(box_width, box_height);
            //T_Cleave.transform.localPosition = new Vector3(offSet * RangeScale, 0, 0);
            collider.offset = new Vector2(offSet, 0);
        } else if (stateInfo.IsName("cleave_up")) {
            collider.size = new Vector2(box_height, box_width);
            //T_Cleave.transform.localPosition = new Vector3(0, offSet * RangeScale, 0);
            collider.offset = new Vector2(0, offSet);
        } else if (stateInfo.IsName("cleave_down")) {
            collider.size = new Vector2(box_height, box_width);
            //T_Cleave.transform.localPosition = new Vector3(0, -offSet * RangeScale, 0);
            collider.offset = new Vector2(0, -offSet);
        }
        animator.transform.GetComponent<Collider2D>().enabled = true;
        AudioSource.PlayClipAtPoint(animator.transform.GetComponent<Cleave>().SFX, animator.transform.position, GameManager.SFX_Volume);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime >= 0.5) {
            animator.transform.GetComponent<Collider2D>().enabled = false;
            //animator.transform.localPosition = Vector3.zero;
            Stack<Collider2D> HittedStack = animator.transform.GetComponent<Cleave>().HittedStack;
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
