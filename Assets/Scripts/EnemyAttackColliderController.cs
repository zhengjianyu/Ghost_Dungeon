using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttackColliderController : MonoBehaviour {
    public float AttackRange = 0.1f;//Spawn offset: +x = right, -x = left, +y = up, -y = down
    public float AttackBoxWidth = 0.16f;
    public float AttackBoxHeight = 0.32f;

    BoxCollider2D AttackCollider;
    EnemyController EC;

    [HideInInspector]
    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();
    void Start() {
        AttackCollider = GetComponent<BoxCollider2D>();
        if (transform.parent!= null) {
            EC = transform.parent.GetComponent<EnemyController>();
        }
    }

    void Update() {
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            if (HittedStack.Count != 0 && HittedStack.Contains(collider)) {//Prevent duplicated attacks
                return;
            }
            PlayerController Player = collider.GetComponent<PlayerController>();
            DMG dmg = EC.AutoAttackDamageDeal(Player.CurrDefense);
            Player.DeductHealth(dmg);
            HittedStack.Push(collider);
        } 
    }
}
