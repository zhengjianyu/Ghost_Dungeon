using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {
    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 MoveVector = Vector2.zero;
    [HideInInspector]
    public Vector2 AttackVector = Vector2.zero;
    [HideInInspector]
    public int Direction = 0;

    [HideInInspector]
    public bool Stunned = false;
    [HideInInspector]
    public bool Attacking = false;
    [HideInInspector]
    public bool Alive = true;



    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;

    virtual protected void Awake() {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    virtual public DMG AutoAttackDamageDeal(float TargetDefense) {
        return null;
    }

    virtual public void DeductHealth(DMG dmg, AudioClip crit_sfx = null) {

    }

    virtual public void DeductMana(float ManaCost) {

    }


}
