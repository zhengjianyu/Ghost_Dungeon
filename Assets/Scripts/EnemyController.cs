using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
    public AudioClip attack;
    public AudioClip hit;
    public AudioClip die;

    public float movement_animation_interval = 1f;
    public float attack_animation_interval = 1f;
    public int AutoAttackType = 0; //0 for melee, 1 for range
    public string Name;
    public int lvl;

    public float MaxHealth;
    public float MaxMana;
    public float MaxAD;
    public float MaxMD;
    public float MaxAttkSpd; //Percantage
    public float MaxMoveSpd; //Percantage
    public float MaxDefense; //Percantage

    public float MaxCritChance = 0.3f; //Percantage
    public float MaxCritDmgBounus = 1f; //Percantage
    public float MaxLPH;
    public float MaxMPH;

    [HideInInspector]
    public float CurrHealth;
    [HideInInspector]
    public float CurrMana;
    [HideInInspector]
    public float CurrAD;
    [HideInInspector]
    public float CurrMD;
    [HideInInspector]
    public float CurrAttSpd;
    [HideInInspector]
    public float CurrMoveSpd;
    [HideInInspector]
    public float CurrDefense;
    [HideInInspector]
    public float CurrCritChance;
    [HideInInspector]
    public float CurrCritDmgBounus;
    [HideInInspector]
    public float CurrLPH;
    [HideInInspector]
    public float CurrMPH;

    Rigidbody2D rb;
    private Animator Anim;

    [HideInInspector]
    public bool Attacking = false;

    [HideInInspector]
    public bool Alive = true;

    AIController AI;

    void Awake() {
        AI = GetComponent<AIController>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        Anim = this.GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        CurrHealth = MaxHealth;
        CurrMana = MaxMana;
        CurrAD = MaxAD;
        CurrMD = MaxMD;
        CurrAttSpd = MaxAttkSpd;
        CurrMoveSpd = MaxMoveSpd;
        CurrDefense = MaxDefense;

        CurrCritChance = MaxCritChance;
        CurrCritDmgBounus = MaxCritDmgBounus;

        CurrLPH = MaxLPH;
        CurrMPH = MaxMPH;
	}
	
	// Update is called once per frame
	void Update () {
        AnimUpdate();
    }

    void FixedUpdate() {
        MoveUpdate();
    }

    //----------public

    //Combat
    public DMG AutoAttackDamageDeal(float TargetDefense) {
        DMG dmg = new DMG();
        if (Random.value < (CurrCritChance / 100)) {
            dmg.Damage += CurrAD * (CurrCritDmgBounus / 100);
            dmg.Damage += CurrMD * (CurrCritDmgBounus / 100);
            dmg.IsCrit = true;
        } else {
            dmg.Damage = CurrAD + CurrMD;
            dmg.IsCrit = false;
        }
        float reduced_dmg = dmg.Damage * (TargetDefense / 100);
        dmg.Damage = dmg.Damage - reduced_dmg;
        GenerateLPHMPH();
        return dmg;
    }

    public void GenerateLPHMPH() {
        if (CurrHealth < MaxHealth && CurrHealth + CurrLPH <= MaxHealth)
            CurrHealth += CurrLPH;
        else
            CurrHealth = MaxHealth;
        if (CurrMana < MaxMana && CurrMana + CurrMPH <= MaxMana)
            CurrMana += CurrMPH;
        else
            CurrMana = MaxMana;
    }

    public void DeductHealth(DMG dmg,AudioClip crit_hit) {
        IndicationController IC = transform.Find("Indication Board").GetComponent<IndicationController>();
        if (CurrHealth - dmg.Damage <= 0) {
            CurrHealth -= dmg.Damage;
            IC.UpdateHealthBar();
            IC.PopUpDmg(dmg);
            DieUpdate();
            return;
        }
        if (dmg.IsCrit) {
            Animator Anim = GetComponent<Animator>();
            Anim.SetFloat("PhysicsSpeedFactor", GetPhysicsSpeedFactor());
            Anim.Play("crit");
            AudioSource.PlayClipAtPoint(crit_hit, transform.position, GameManager.SFX_Volume);
        } else {
            AudioSource.PlayClipAtPoint(hit, transform.position, GameManager.SFX_Volume);
        }
        CurrHealth -= dmg.Damage;
        IC.UpdateHealthBar();
        IC.PopUpDmg(dmg);
    }


    //Animation
    public float GetMovementAnimSpeed() {
        return (CurrMoveSpd/100) / (movement_animation_interval);
    }

    public float GetAttackAnimSpeed() {
        return (CurrAttSpd/100) / (attack_animation_interval);
    }

    public float GetPhysicsSpeedFactor() {
        if (!Attacking) {
            if (CurrMoveSpd < 100)
                return 1 + CurrMoveSpd / 100;
            else if (CurrMoveSpd > 100)
                return 1 - CurrMoveSpd / 100;
            else
                return 1;
        } else {
            if (CurrAttSpd < 100)
                return 1 + CurrAttSpd / 100;
            else if (CurrMoveSpd > 100)
                return 1 - CurrAttSpd / 100;
            else
                return 1;
        }
    }

    void MoveUpdate() {
        if (AI.MoveVector != Vector2.zero)
            rb.MovePosition(rb.position + AI.MoveVector * (CurrMoveSpd/100) * Time.deltaTime);
    }

    void AnimUpdate() {
        if (Attacking) {
            Anim.speed = GetAttackAnimSpeed();
        } else {
            Anim.speed = GetMovementAnimSpeed();
        }
        if (AI.AttackVector != Vector2.zero) {
            Anim.SetBool("IsAttacking", true);
            Anim.SetInteger("Direction", AI.Direction);
            Anim.SetBool("IsMoving", false);
        }
        else if (AI.MoveVector != Vector2.zero && AI.AttackVector == Vector2.zero) {
            Anim.SetBool("IsMoving", true);
            Anim.SetInteger("Direction", AI.Direction);
            Anim.SetBool("IsAttacking", false);
        }
        else {
            Anim.SetBool("IsMoving", false);
            Anim.SetBool("IsAttacking", false);
        }
    }   

    void DieUpdate() {
        if (CurrHealth <= 0) {//Insert dead animation here
            GetComponent<DropList>().SpawnLoots();
            Destroy(transform.parent.gameObject);
        }
    }
}
