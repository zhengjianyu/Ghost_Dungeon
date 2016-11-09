using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swipe : ActiveSkill {
    [HideInInspector]
    public float ADScale;
    [HideInInspector]
    public float Cost;
    [HideInInspector]
    public float AttackRange;
    [HideInInspector]
    public Animator Anim;

    public AudioClip SFX;
    public AudioClip Crit_SFX;

    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();

    PlayerController PC;

    protected override void Awake() {
        base.Awake();
        Anim = GetComponent<Animator>();
        if (transform.parent == null)
            return;
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        switch (this.SkillData.lvl) {
            case 0:
                break;
            case 1:
                CD = GetComponent<Swipe1>().CD;
                ADScale = GetComponent<Swipe1>().ADScale;
                AttackRange = GetComponent<Swipe1>().AttackRange;
                Anim.runtimeAnimatorController = GetComponent<Swipe1>().Anim;
                break;
            case 2:
                CD = GetComponent<Swipe2>().CD;
                ADScale = GetComponent<Swipe2>().ADScale;
                AttackRange = GetComponent<Swipe1>().AttackRange;
                Anim.runtimeAnimatorController = GetComponent<Swipe2>().Anim;
                break;
            case 3:
                CD = GetComponent<Swipe3>().CD;
                ADScale = GetComponent<Swipe3>().ADScale;
                AttackRange = GetComponent<Swipe1>().AttackRange;
                Anim.runtimeAnimatorController = GetComponent<Swipe3>().Anim;
                break;
            case 4:
                CD = GetComponent<Swipe4>().CD;
                ADScale = GetComponent<Swipe4>().ADScale;
                AttackRange = GetComponent<Swipe1>().AttackRange;
                Anim.runtimeAnimatorController = GetComponent<Swipe4>().Anim;
                break;
            case 5:
                CD = GetComponent<Swipe5>().CD;
                ADScale = GetComponent<Swipe5>().ADScale;
                AttackRange = GetComponent<Swipe1>().AttackRange;
                Anim.runtimeAnimatorController = GetComponent<Swipe5>().Anim;
                break;
        }
        PC = transform.parent.parent.GetComponent<PlayerController>();
    }

    protected override void Start() {
        base.Start();
    }


    protected override void Update() {
        base.Update();
        List<GameObject> IgnoreList = new List<GameObject> (GameObject.FindGameObjectsWithTag("MainPlayer"));
        IgnoreList.AddRange(GameObject.FindGameObjectsWithTag("FriendlyPlayer"));
        foreach (var o in IgnoreList) {
            Physics2D.IgnoreCollision(o.transform.Find("PlayerController").GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

    }

    public override void Active() {
        Anim.SetTrigger("Active");
    }

    //Unique Methods

    DMG SkillDmg(float TargetDefense) {
        DMG dmg = new DMG();
        if (Random.value < (PC.CurrCritChance / 100)) {
            dmg.Damage += PC.CurrAD * (ADScale/100) * (PC.CurrCritDmgBounus / 100);
            dmg.IsCrit = true;
        } else {
            dmg.Damage += PC.CurrAD * (ADScale / 100);
            dmg.IsCrit = false;
        }
        float reduced_dmg = dmg.Damage * (TargetDefense / 100);
        dmg.Damage = dmg.Damage - reduced_dmg;
        PC.GenerateLPHMPH();
        return dmg;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Enemy") {
            if (HittedStack.Count != 0 && HittedStack.Contains(collision.collider)) {//Prevent duplicated attacks
                return;
            }
            EnemyController Enemy = collision.collider.GetComponent<EnemyController>();
            DMG dmg = SkillDmg(Enemy.CurrDefense);
            Enemy.DeductHealth(dmg,Crit_SFX);
            HittedStack.Push(collision.collider);
        } else if (collision.transform.tag == "Player") {

        }
    }
}
