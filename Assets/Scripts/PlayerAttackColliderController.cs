using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttackColliderController : MonoBehaviour {
    public float AttackRange = 0.1f;//Spawn offset: +x = right, -x = left, +y = up, -y = down
    public float AttackBoxWidth = 0.16f;
    public float AttackBoxHeight = 0.32f;

    BoxCollider2D AttackCollider;
    PlayerController PC;
    WeaponController WC;

    [HideInInspector]
    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();

    void Start() {
        AttackCollider = GetComponent<BoxCollider2D>();
        if (transform.parent.parent != null) {
            PC = transform.parent.parent.GetComponent<PlayerController>();
            WC = transform.parent.GetComponent<WeaponController>();
        }
    }

    void Update() {
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            if (HittedStack.Count!=0 && HittedStack.Contains(collider)) {//Prevent duplicated attacks
                return;
            }
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            DMG dmg = PC.AutoAttackDamageDeal(Enemy.CurrDefense);
            Enemy.DeductHealth(dmg,WC.crit);
            HittedStack.Push(collider);
        }
        else if (collider.tag == "Player") {
        }
    }
}
