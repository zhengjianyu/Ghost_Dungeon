using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Charge : ActiveSkill {
    [HideInInspector]
    public float ADScale;
    //[HideInInspector]
    public float Force;

    ObjectController OC;
    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();

    protected override void Awake() {
        base.Awake();
    }
    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        
        OC = transform.parent.parent.GetComponent<ObjectController>();
    }

    public override void Active() {
        base.Active();
        Vector2 charge_direction = Vector2.zero;
        switch (OC.Direction) {
            case 0:
                charge_direction = new Vector2(0, -1);
                break;
            case 1:
                charge_direction = new Vector2(-1, 0);
                break;
            case 2:
                charge_direction = new Vector2(1, 0);
                break;
            case 3:
                charge_direction = new Vector2(0, 1);
                break;
        }
        transform.GetComponent<Collider2D>().enabled = true;
        OC.rb.AddForce(charge_direction*Force,ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            if (HittedStack.Count != 0 && HittedStack.Contains(collider)) {//Prevent duplicated attacks
                return;
            }
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            Enemy.rb.mass = 1000;
            //DMG dmg = SkillDmg(Enemy.CurrDefense);
            //Enemy.DeductHealth(dmg, Crit_SFX);
            //HittedStack.Push(collider);
            transform.GetComponent<Collider2D>().enabled = false;
        }
    }
}