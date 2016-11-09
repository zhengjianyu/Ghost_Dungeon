using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cleave : ActiveSkill {
    [HideInInspector]
    public float ADScale;
    [HideInInspector]
    public float Cost;
    [HideInInspector]
    public float RangeScale;
    [HideInInspector]
    public Animator Anim;

    public AudioClip SFX;
    public AudioClip Crit_SFX;

    public Stack<Collider2D> HittedStack = new Stack<Collider2D>();

    PlayerController PC;

    protected override void Awake() {
        base.Awake();
        Anim = GetComponent<Animator>();
        Physics2D.GetIgnoreLayerCollision(8, 9);
        if (transform.parent == null)
            return;
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
        switch (this.SkillData.lvl) {
            case 0:
                break;
            case 1:
                CD = GetComponent<Cleave1>().CD;
                Cost = GetComponent<Cleave1>().Cost;
                ADScale = GetComponent<Cleave1>().ADScale;
                RangeScale = GetComponent<Cleave1>().RangeScale;
                break;
            case 2:
                CD = GetComponent<Cleave2>().CD;
                Cost = GetComponent<Cleave2>().Cost;
                ADScale = GetComponent<Cleave2>().ADScale;
                RangeScale = GetComponent<Cleave2>().RangeScale;
                break;
            case 3:
                CD = GetComponent<Cleave3>().CD;
                Cost = GetComponent<Cleave3>().Cost;
                ADScale = GetComponent<Cleave3>().ADScale;
                RangeScale = GetComponent<Cleave3>().RangeScale;
                break;
            case 4:
                CD = GetComponent<Cleave4>().CD;
                Cost = GetComponent<Cleave4>().Cost;
                ADScale = GetComponent<Cleave4>().ADScale;
                RangeScale = GetComponent<Cleave4>().RangeScale;
                break;
            case 5:
                CD = GetComponent<Cleave5>().CD;
                Cost = GetComponent<Cleave5>().Cost;
                ADScale = GetComponent<Cleave5>().ADScale;
                RangeScale = GetComponent<Cleave5>().RangeScale;
                break;
        }
        transform.localScale = new Vector2(RangeScale, RangeScale);
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
        if (PC.CurrMana - Cost >= 0) {
            Anim.SetInteger("Direction", PC.Direction);
            Anim.SetTrigger("Active");
            PC.DeductMana(Cost);
        } else {
            Debug.Log("Not enough mana");
        }
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

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            if (HittedStack.Count != 0 && HittedStack.Contains(collider)) {//Prevent duplicated attacks
                return;
            }
            EnemyController Enemy = collider.GetComponent<EnemyController>();
            Vector2 BouceOffDirection = (Vector2)Vector3.Normalize(Enemy.transform.position - PC.transform.position);
            Enemy.rb.mass = 1;
            Enemy.rb.AddForce(BouceOffDirection * SkillData.lvl * 2, ForceMode2D.Impulse);
            DMG dmg = SkillDmg(Enemy.CurrDefense);
            Enemy.DeductHealth(dmg, Crit_SFX);
            HittedStack.Push(collider);
        } else if (collider.transform.tag == "Player") {

        }
    }
}
