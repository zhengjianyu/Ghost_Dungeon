using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackState : StateMachineBehaviour {

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        WeaponController WC = animator.transform.GetComponent<WeaponController>();
        if (WC.Type == 0 || WC.Type == 1) {//GreatSwaord or Axes, has 3 combo
            MeleeAttackEnter(animator, stateInfo, WC);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        WeaponController WC = animator.transform.GetComponent<WeaponController>();
        if ((WC.Type == 0 || WC.Type == 1) && stateInfo.normalizedTime >= 0.5) {
            MeleeAttacExit(animator, stateInfo, WC);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

    //OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK(inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

    void MeleeAttackEnter(Animator animator, AnimatorStateInfo stateInfo, WeaponController WC) {
        PlayerController PC = animator.transform.parent.GetComponent<PlayerController>();
        Transform T_AttackCollider = animator.transform.Find("AttackCollider");
        PC.Attacking = true;
        BoxCollider2D AttackCollider = T_AttackCollider.GetComponent<BoxCollider2D>();
        float AttackRange = T_AttackCollider.GetComponent<PlayerAttackColliderController>().AttackRange;
        float AttackBoxWidth = T_AttackCollider.GetComponent<PlayerAttackColliderController>().AttackBoxWidth;
        float AttackBoxHeight = T_AttackCollider.GetComponent<PlayerAttackColliderController>().AttackBoxHeight;

        //Combo 1
        if (stateInfo.IsName("combo1_left")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(-AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo1_right")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo1_up")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo1_down")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, -AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_1, animator.transform.position, GameManager.SFX_Volume);
        }
        //Combo 2
        else if (stateInfo.IsName("combo2_left")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(-AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo2_right")) {
            AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
            AttackCollider.offset = new Vector2(AttackRange, 0);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo2_up")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo2_down")) {
            AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
            AttackCollider.offset = new Vector2(0, -AttackRange);
            AudioSource.PlayClipAtPoint(WC.combo_2, animator.transform.position, GameManager.SFX_Volume);
        }
        //Combo 3
        else if (stateInfo.IsName("combo3_left")) {
            if (WC.Type == 0) {//GreatSword
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
                AttackCollider.offset = new Vector2(-AttackRange, 0);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo3_right")) {
            if (WC.Type == 0) {//GreatSword
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxWidth, AttackBoxHeight);
                AttackCollider.offset = new Vector2(AttackRange, 0);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo3_up")) {
            if (WC.Type == 0) {
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
                AttackCollider.offset = new Vector2(0, AttackRange);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        } else if (stateInfo.IsName("combo3_down")) {
            if (WC.Type == 0) {
                AttackCollider.offset = Vector2.zero;
                float AttackRadius = AttackBoxHeight >= AttackBoxWidth ? AttackBoxWidth + AttackRange * 2 : AttackBoxHeight + AttackRange * 2;
                AttackCollider.size = new Vector2(AttackRadius, AttackRadius);
            } else if (WC.Type == 1) {//Axe
                AttackCollider.size = new Vector2(AttackBoxHeight, AttackBoxWidth);
                AttackCollider.offset = new Vector2(0, -AttackRange);
            }
            AudioSource.PlayClipAtPoint(WC.combo_3, animator.transform.position, GameManager.SFX_Volume);
        }
        AttackCollider.enabled = true;
    }

    void MeleeAttacExit(Animator animator, AnimatorStateInfo stateInfo, WeaponController WC) {
        PlayerController PC = animator.transform.parent.GetComponent<PlayerController>();
        Transform T_AttackCollider = animator.transform.Find("AttackCollider");
        BoxCollider2D AttackCollider = T_AttackCollider.GetComponent<BoxCollider2D>();
        PC.Attacking = false;
        AttackCollider.enabled = false;
        Stack<Collider2D> HittedStack = T_AttackCollider.GetComponent<PlayerAttackColliderController>().HittedStack;
        if (HittedStack.Count != 0) {
            HittedStack.Clear();
        }
        
    }
}
