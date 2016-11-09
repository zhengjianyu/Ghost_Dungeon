using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour {
    public string Name;
    public string Class;
    public string Type;
    public int Rarity;

    public Vector2 AddHealth;
    public Vector2 AddMana;
    public Vector2 AddAD;
    public Vector2 AddMD;
    public Vector2 AddAttkSpd;
    public Vector2 AddMoveSpd;
    public Vector2 AddDefense;

    public Vector2 AddCritChance;//Percentage
    public Vector2 AddCritDmgBounus;//Percentage

    public Vector2 AddLPH;
    public Vector2 AddMPH;

    [HideInInspector]
    public Equipment E;

    private Animator Anim;
    // Use this for initialization
    void Awake() {
        InstantiateEquipmentData();
        Anim = GetComponent<Animator>();
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        IgnoreCollisionUpdate();
    }

    void IgnoreCollisionUpdate() {
        List<GameObject> IgnoreList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        IgnoreList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (var so in GameObject.FindGameObjectsWithTag("Skill")) {
            if (so.GetComponent<Collider2D>() != null)
                IgnoreList.Add(so);
        }
        foreach (var o in IgnoreList)
            Physics2D.IgnoreCollision(o.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    public void EquipUpdate(int Direction, Vector2 AttackVector) {
        if (E.Type == "Trinket") {//No update for trinket for now
            return;
        }
        PlayerController PC = GameObject.Find("MainPlayer/PlayerController").GetComponent<PlayerController>();
        Anim.SetInteger("Direction", Direction);
        if (PC.Attacking) {
            Anim.speed = PC.GetAttackAnimSpeed();
        } else {
            Anim.speed = PC.GetMovementAnimSpeed();
        }
        if (E.Type == "Weapon") {//Weapon animation speed is controlled by AttackCollider
            if (AttackVector != Vector2.zero) {
                Anim.SetBool("IsAttacking",true);
                Anim.SetInteger("Direction", Direction);
            } else {
                Anim.SetBool("IsAttacking", false);
            }
            if (Direction == 3)
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            else
                GetComponent<SpriteRenderer>().sortingOrder = 2;
        } else {//Helmet,Chest,Shackle
            Anim.speed = PC.GetMovementAnimSpeed();
        }
    }

    public void InstantiateLoot(Transform T) {
        InstantiateEquipmentData();
        GameObject Loot = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name), T.position,T.rotation) as GameObject;
        Loot.name = E.Name;
        GameObject[] PlayerList = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject player in PlayerList) {
            Physics2D.IgnoreCollision(player.transform.GetComponent<Collider2D>(), Loot.transform.GetComponent<Collider2D>());
        }
        foreach (GameObject enemy in EnemyList) {
            Physics2D.IgnoreCollision(enemy.transform.GetComponent<Collider2D>(), Loot.transform.GetComponent<Collider2D>());
        }
        Destroy(Loot.transform.Find("Icon").gameObject);
        Loot.transform.Find("LootBox").gameObject.SetActive(true);
        Loot.GetComponent<EquipmentController>().LootRandomlize();
        Loot.GetComponent<SpriteRenderer>().sortingOrder = 0; //Subject to change
        Loot.GetComponent<BoxCollider2D>().enabled = true;
        Text E_NameText = Loot.transform.Find("LootBox/UI/Name").GetComponent<Text>();
        switch (E.Rarity) {
            case 0: //Common
                E_NameText.color = MyColor.White;
                break;
            case 1: //UnCommon
                E_NameText.color = MyColor.Cyan;
                break;
            case 2: //Perfect
                E_NameText.color = MyColor.Yellow;
                break;
            case 3: //Legendary
                E_NameText.color = MyColor.Orange;
                break;
        }
        E_NameText.text = E.Name;
    }

    static public GameObject ObtainPrefab(Equipment E,Transform parent) {
        GameObject equipPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name)) as GameObject;
        equipPrefab.GetComponent<EquipmentController>().E = E;
        Destroy(equipPrefab.GetComponent<Rigidbody2D>());
        Destroy(equipPrefab.GetComponent< BoxCollider2D>());
        equipPrefab.name = E.Name;
        equipPrefab.transform.position = parent.transform.position + equipPrefab.transform.position;
        Destroy(equipPrefab.transform.Find("LootBox").gameObject);
        Destroy(equipPrefab.transform.Find("Icon").gameObject);
        equipPrefab.transform.parent = parent;
        return equipPrefab;
    }

    static public GameObject ObtainEquippedIcon(Equipment E, Transform parent) {
        GameObject equipPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name)) as GameObject;
        GameObject equipIcon = equipPrefab.transform.Find("Icon").gameObject;
        equipIcon.SetActive(true);
        equipIcon.name = E.Name;
        equipIcon.transform.position = parent.transform.position + equipIcon.transform.position;
        equipIcon.transform.parent = parent;
        Destroy(equipPrefab);
        return equipIcon;
    }

    static public GameObject ObtainInventoryIcon(Equipment E, Transform parent) {
        GameObject equipPrefab = Instantiate(Resources.Load("EquipmentPrefabs/" + E.Name)) as GameObject;
        GameObject equipIcon = equipPrefab.transform.Find("Icon").gameObject;
        equipIcon.SetActive(true);
        equipIcon.name = E.Name;
        equipIcon.transform.position = parent.transform.position + equipIcon.transform.position;
        equipIcon.transform.parent = parent;
        equipIcon.transform.localScale = new Vector2(500, 500);
        Destroy(equipPrefab);
        return equipIcon;
    }

    private void InstantiateEquipmentData() {//These 4 field are must have
        E = ScriptableObject.CreateInstance<Equipment>();
        E.Rarity = Rarity;
        E.Name = Name;
        E.Class = Class;
        E.Type = Type;

    }

    //Could be public in future
    void LootRandomlize() {
        List<Vector2> RandomStats = new List<Vector2>() {AddHealth,AddMana,AddAD,AddMD,AddAttkSpd,AddMoveSpd,AddDefense,AddCritChance,AddCritDmgBounus,AddLPH,AddMPH};
        for(int i = 0; i < RandomStats.Count; i++) {
            if (RandomStats[i] != Vector2.zero) {
                switch (i) {
                    case 0:
                        E.AddHealth = (int)Random.Range(RandomStats[i].x, RandomStats[i].y+1);
                        break;
                    case 1:
                        E.AddMana = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 2:
                        E.AddAD = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 3:
                        E.AddMD = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 4:
                        E.AddAttkSpd = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 5:
                        E.AddMoveSpd = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 6:
                        E.AddDefense = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 7:
                        E.AddCritChance = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 8:
                        E.AddCritDmgBounus = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 9:
                        E.AddLPH = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                    case 10:
                        E.AddMPH = (int)Random.Range(RandomStats[i].x, RandomStats[i].y + 1);
                        break;
                }
            }
        }
    }

}
