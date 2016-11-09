using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : ObjectController {
    private int NextLevelExp = -999;

    public AudioClip hit;
    public AudioClip default_crit_hit;
    public AudioClip die;
    public AudioClip lvlup;

    [HideInInspector]
    public CharacterDataStruct PlayerData;

    [HideInInspector]
    public string Name;

    [HideInInspector]
    public float MaxHealth;
    [HideInInspector]
    public float MaxMana;
    [HideInInspector]
    public float MaxAD;
    [HideInInspector]
    public float MaxMD;
    [HideInInspector]
    public float MaxAttkSpd;
    [HideInInspector]
    public float MaxMoveSpd;
    [HideInInspector]
    public float MaxDefense;

    [HideInInspector]
    public float MaxCritChance; //Percantage
    [HideInInspector]
    public float MaxCritDmgBounus; //Percantage
    [HideInInspector]
    public float MaxLPH;
    [HideInInspector]
    public float MaxMPH;


    public float CurrHealth;
    public float CurrMana;
    public float CurrAD;
    public float CurrMD;
    public float CurrAttSpd;
    public float CurrMoveSpd;
    public float CurrDefense;
    public float CurrCritChance;    
    public float CurrCritDmgBounus;    
    public float CurrLPH;    
    public float CurrMPH;

    Dictionary<string, GameObject> EquipPrefabs;

    public ControllerManager CM;
    private SaveLoadManager SLM;
    private IndicationController IC;
    private PlayerUIController PUIC;

    private GameObject BaseModel;

    private GameObject PickedTarget = null;

    public GameObject FirstWeapon;

    [HideInInspector]
    public GameObject SkillTree;
    [HideInInspector]
    public Transform Actives;
    [HideInInspector]
    public Transform Passives;

    protected override void Awake() {
        base.Awake();

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;

        if (FirstWeapon!=null)
            FirstWeapon.GetComponent<EquipmentController>().InstantiateLoot(transform);
        EquipPrefabs = new Dictionary<string, GameObject>();
        
        if (transform.parent.name == "MainPlayer") {
            if (ControllerManager.Instance) {
                CM = ControllerManager.Instance;
            } else
                CM = FindObjectOfType<ControllerManager>();
            if (SaveLoadManager.Instance)
                SLM = SaveLoadManager.Instance;
            else
                SLM = FindObjectOfType<SaveLoadManager>();
            PUIC = transform.parent.Find("PlayerUI").GetComponent<PlayerUIController>();
            PlayerData = SLM.LoadPlayerInfo(SLM.SlotIndexToLoad);
        }
        IC = transform.Find("Indication Board").GetComponent<IndicationController>();
        Actives = transform.Find("Actives");
        Passives = transform.Find("Passives");
        InitPlayer();
    }

    void Start() {
        //InitPlayer();
    }

    // Update is called once per frame
    void Update() {
        ControlUpdate();
        PickUpInUpdate();
        EquiPrefabsUpdate();
        BaseModelUpdate();
    }

    void FixedUpdate() {
        MoveUpdate();
    }

    void ControlUpdate() {
        if (Stunned)
            return;
        if (CM != null) {
            AttackVector = CM.AttackVector;
            MoveVector = CM.MoveVector;
            Direction = CM.Direction;
        }
    }

    void MoveUpdate() {
        if (MoveVector != Vector2.zero) {
            //rb.MovePosition(rb.position + MoveVector * (CurrMoveSpd / 100) * Time.deltaTime);
            rb.AddForce(MoveVector * (CurrMoveSpd / 100) * rb.drag);
        }
    }

    void OnTriggerStay2D(Collider2D collider) {
        if (collider.tag == "Lootable") {
            PickedTarget = collider.transform.parent.gameObject;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "Lootable" && PickedTarget!=null) {
            PickedTarget = null;
        }
    }

    //----------public
    //Skills Handling
    public ActiveSkill GetActiveSlotSkillTransform(int Slot) {
        if (PlayerData.ActiveSlotData[Slot] == null ||Actives.childCount == 0)
            return null;
        for (int i = 0; i < Actives.childCount; i++) {
            if (Actives.GetChild(i).GetComponent<ActiveSkill>().SkillData.Name == PlayerData.ActiveSlotData[Slot].Name)
                return Actives.GetChild(i).GetComponent<ActiveSkill>();
        }
        return null;
    }

    //EXP handling
    public void AddEXP(int exp) {
        if (PlayerData.lvl < LvlExpModule.LvlCap) {
            PlayerData.exp += exp;
            CheckLevelUp();
            //if(PUIC)
            //    PUIC.UpdateExpBar();
        }
        SLM.SaveCurrentPlayerInfo();
    }

    public int GetNextLvlExp() {
        return NextLevelExp;
    }

    //Combat Methods
    override public DMG AutoAttackDamageDeal(float TargetDefense) {
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

    public override void DeductMana(float ManaCost) {
        CurrMana -= ManaCost;
        if (transform.parent.tag == "MainPlayer") {
            PUIC.UpdateHealthManaBar();
        }
    }

    override public void DeductHealth(DMG dmg, AudioClip crit_sfx = null) {
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
            if(crit_sfx==null)
                AudioSource.PlayClipAtPoint(default_crit_hit, transform.position, GameManager.SFX_Volume);
            else
                AudioSource.PlayClipAtPoint(crit_sfx, transform.position, GameManager.SFX_Volume);
        } else {
            AudioSource.PlayClipAtPoint(hit, transform.position, GameManager.SFX_Volume);
        }
        CurrHealth -= dmg.Damage;
        if (transform.parent.tag == "MainPlayer") {
            PUIC.UpdateHealthManaBar();
        } else {
            IC.UpdateHealthBar();
        }
        IC.PopUpDmg(dmg);
    }

    //Animation Handling
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

    //Equipment/Inventory Handling
    public bool InventoryIsFull() {
        return FirstAvailbleInventorySlot() == PlayerData.Inventory.Length;
    }
    public Equipment GetEquippedItem(string Slot) {
        return PlayerData.Equipments[Slot];
    }
    public Equipment GetInventoryItem(int Slot) {
        return PlayerData.Inventory[Slot];
    }

    public bool Compatible(Equipment E) {
        if (E == null)
            return false;
        if (E.Class == "All")//Trinket
            return PlayerData.lvl >= E.LvlReq;
        return (PlayerData.lvl >= E.LvlReq && PlayerData.Class == E.Class);
    }

    public int FirstAvailbleInventorySlot() {
        for(int i = 0; i < PlayerData.Inventory.Length; i++) {
            if (PlayerData.Inventory[i] == null)
                return i;
        }
        return PlayerData.Inventory.Length;
    }

    public void Equip(Equipment E) {
        PlayerData.Equipments[E.Type] = E;
        GameObject equipPrefab = EquipmentController.ObtainPrefab(E, transform);
        EquipPrefabs[E.Type] = equipPrefab;
        UpdateStats();
        SLM.SaveCurrentPlayerInfo();
    }

    public void UnEquip(string Slot) {
        Destroy(EquipPrefabs[Slot]);
        PlayerData.Equipments[Slot] = null;
        UpdateStats();
        SLM.SaveCurrentPlayerInfo();
    }

    public void AddToInventory(int Slot, Equipment E) {
        PlayerData.Inventory[Slot] = E;
        SLM.SaveCurrentPlayerInfo();
    }

    public void RemoveFromInventory(int Slot, Equipment E) {
        PlayerData.Inventory[Slot] = null;
        SLM.SaveCurrentPlayerInfo();
    }

    //-------private
    void InitPlayer() {
        if(PlayerData.lvl<LvlExpModule.LvlCap)
            NextLevelExp = LvlExpModule.GetRequiredExp(PlayerData.lvl + 1);
        InitSkills();
        InitStats();
        InstaniateEquipment();
        //PUIC.UpdateExpBar();
    }

    void InitSkills() {
        if (PlayerData.Class == "Warrior") {
            SkillTree = Instantiate(Resources.Load("SkillPrefabs/WarriorSkillTree"), transform) as GameObject;
            SkillTree.name = "SkillTree";
        }
        else if(PlayerData.Class == "Mage") {

        }else if(PlayerData.Class == "Rogue") {

        }
        SkillTree.GetComponent<SkillTreeController>().InstantiateSkills();
        if (PUIC) {//MainPlayer UI

        }
    }

    void InitStats() {
        Name = PlayerData.Name;

        MaxHealth = PlayerData.BaseHealth;
        MaxMana = PlayerData.BaseMana;
        MaxAD = PlayerData.BaseAD;
        MaxMD = PlayerData.BaseMD;
        MaxAttkSpd = PlayerData.BaseAttkSpd;
        MaxMoveSpd = PlayerData.BaseMoveSpd;
        MaxDefense = PlayerData.BaseDefense;
        MaxCritChance = PlayerData.BaseCritChance;
        MaxCritDmgBounus = PlayerData.BaseCritDmgBounus;
        MaxLPH = PlayerData.BaseLPH;
        MaxMPH = PlayerData.BaseMPH;
        foreach (var e in PlayerData.Equipments) {
            if (e.Value != null) {
                MaxHealth += e.Value.AddHealth;
                MaxMana += e.Value.AddMana;
                MaxAD += e.Value.AddAD;
                MaxMD += e.Value.AddMD;
                MaxAttkSpd += e.Value.AddAttkSpd;
                MaxMoveSpd += e.Value.AddMoveSpd;
                MaxDefense += e.Value.AddDefense;
                MaxCritChance += e.Value.AddCritChance;
                MaxCritDmgBounus += e.Value.AddCritDmgBounus;
                MaxLPH += e.Value.AddLPH;
                MaxMPH += e.Value.AddMPH;
            }           
        }
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

    void BaseModelUpdate() {
        Animator BaseModelAnim = BaseModel.GetComponent<Animator>();
        if (CM != null) {
            BaseModelAnim.SetInteger("Direction", Direction);
            BaseModelAnim.speed = GetMovementAnimSpeed();
        }
    }

    void EquiPrefabsUpdate() {
        if (CM != null) {
            foreach(var e_prefab in EquipPrefabs.Values) {
                if(e_prefab!=null)
                    e_prefab.GetComponent<EquipmentController>().EquipUpdate(Direction, AttackVector);
            }
        }      
    }

    //-------helper
    void UpdateStats() {
        MaxHealth = PlayerData.BaseHealth;
        MaxMana = PlayerData.BaseMana;
        MaxAD = PlayerData.BaseAD;
        MaxMD = PlayerData.BaseMD;
        MaxAttkSpd = PlayerData.BaseAttkSpd;
        MaxMoveSpd = PlayerData.BaseMoveSpd;
        MaxDefense = PlayerData.BaseDefense;
        MaxCritChance = PlayerData.BaseCritChance;
        MaxCritDmgBounus = PlayerData.BaseCritDmgBounus;
        MaxLPH = PlayerData.BaseLPH;
        MaxMPH = PlayerData.BaseMPH;
        foreach (var e in PlayerData.Equipments) {
            if (e.Value != null) {
                MaxHealth += e.Value.AddHealth;
                MaxMana += e.Value.AddMana;
                MaxAD += e.Value.AddAD;
                MaxMD += e.Value.AddMD;
                MaxAttkSpd += e.Value.AddAttkSpd;
                MaxMoveSpd += e.Value.AddMoveSpd;
                MaxDefense += e.Value.AddDefense;
                MaxCritChance += e.Value.AddCritChance;
                MaxCritDmgBounus += e.Value.AddCritDmgBounus;
                MaxLPH += e.Value.AddLPH;
                MaxMPH += e.Value.AddMPH;
            }
        }
        if(CurrHealth>MaxHealth)
            CurrHealth = MaxHealth;
        if(CurrMana>MaxMana)
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
    void InstaniateEquipment() {
        if (PlayerData.Class == "Warrior") {
            BaseModel = Instantiate(Resources.Load("Red Ghost/Ghost/Red Ghost"), transform) as GameObject;
            BaseModel.name = "Red Ghost";
            BaseModel.transform.position = transform.position + BaseModel.transform.position;
        }
        else if(PlayerData.Class == "Mage") {

        }
        else if(PlayerData.Class == "Rogue") {

        }
        foreach(var e in PlayerData.Equipments) {
            if (e.Value != null) {
                GameObject equipPrefab = EquipmentController.ObtainPrefab(e.Value, transform);
                EquipPrefabs[e.Key] = equipPrefab;
            }
        }
    }

    void PickUpInUpdate() {
        if (PickedTarget != null && CM.AllowControlUpdate) {
            transform.Find("Indication Board/PickUpNotify").gameObject.SetActive(true);
            if (Input.GetKeyDown(CM.Interact) || Input.GetKeyDown(CM.J_A)) {
                if (InventoryIsFull()) {
                    Debug.Log("Your inventory is full!");
                } else {
                    int InventoryIndex = FirstAvailbleInventorySlot();
                    AddToInventory(InventoryIndex, PickedTarget.GetComponent<EquipmentController>().E);
                    Destroy(PickedTarget);
                    PickedTarget = null;
                }
            }
        } else
            transform.Find("Indication Board/PickUpNotify").gameObject.SetActive(false);
    }

    void CheckLevelUp() {
        if (PlayerData.lvl >= LvlExpModule.LvlCap)
            return;
        if(PlayerData.exp >= NextLevelExp) {
            PlayerData.lvl++;
            PlayerData.exp = 0;
            NextLevelExp = LvlExpModule.GetRequiredExp(PlayerData.lvl + 1);
            AudioSource.PlayClipAtPoint(lvlup, transform.position, GameManager.SFX_Volume);
            PlayerData.StatPoints++;
            PlayerData.SkillPoints++;            
        }     
    }

    void DieUpdate() {
        if (CurrHealth <= 0) {//Insert dead animation here
            //GetComponent<DropList>().SpawnLoots(); //Added for PVP later
            Alive = false;
            Destroy(gameObject);
        }
    }
}
