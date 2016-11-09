using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquippedButtonController : MonoBehaviour {
    PlayerController PC;
    GameObject EquipmentIcon;
    private string Slot;

    GameObject ES;

    public GameObject Stats;

    Equipment E = null;

    // Use this for initialization
    void Awake() {
        Slot = gameObject.name;
        PC = GameObject.Find("MainPlayer/PlayerController").transform.GetComponent<PlayerController>();
        ES = GameObject.Find("EventSystem");
        Stats = Instantiate(Stats, transform) as GameObject;
        Stats.transform.localPosition = new Vector3(0, 0, 0);
        Stats.transform.localScale = new Vector3(1.2f, 1.2f, 0);
    }

    void Start() {
    }

    // Update is called once per frame
    void Update() {
        UpdateStats();
        UpdateSlot();
    }

    public void OnClickUnEquip() {
        if (PC.GetEquippedItem(Slot) != null) {
            if (PC.InventoryIsFull()) {//Inventory Full
                Debug.Log("Your inventory is full!");
                return;
            } else {
                int SlotIndex = PC.FirstAvailbleInventorySlot();
                PC.AddToInventory(SlotIndex, E);
                PC.UnEquip(Slot);
            }
        }
    }
    public void UpdateSlot() {
        if (PC.GetEquippedItem(Slot) == E) {
            return;
        } else {
            E = PC.GetEquippedItem(Slot);
            if (E != null) {
                if (EquipmentIcon != null)
                    DestroyObject(EquipmentIcon);
                EquipmentIcon = EquipmentController.ObtainEquippedIcon(E, transform);
            } else {
                DestroyObject(EquipmentIcon);
                EquipmentIcon = null;
            }
        }
    }



    void UpdateStats() {
        if (E != null && ES.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject == gameObject) {
            Stats.gameObject.SetActive(true);

            Vector2 Interval = new Vector2(0, -50f);
            Vector2 LastUpdatePosition;

            Transform Name = Stats.transform.Find("Name");
            Text NameText = Name.GetComponent<Text>();
            switch (E.Rarity) {
                case 0: //Common
                    NameText.color = MyColor.White;
                    break;
                case 1: //UnCommon
                    NameText.color = MyColor.Cyan;
                    break;
                case 2: //Perfect
                    NameText.color = MyColor.Yellow;
                    break;
                case 3: //Legendary
                    NameText.color = MyColor.Orange;
                    break;
            }
            NameText.text = E.Name;
            Name.gameObject.SetActive(true);

            Transform Class = Stats.transform.Find("Class");
            Text ClassText = Class.GetComponent<Text>();
            ClassText.color = MyColor.White;
            ClassText.text = E.Class;
            LastUpdatePosition = Class.localPosition;
            Class.gameObject.SetActive(true);

            Transform Type = Stats.transform.Find("Type");
            Text TypeText = Type.GetComponent<Text>();
            TypeText.color = MyColor.Blue;
            TypeText.text = E.Type;
            Type.localPosition = LastUpdatePosition + Interval;
            LastUpdatePosition = Type.localPosition;
            Type.gameObject.SetActive(true);

            Transform LvlReq = Stats.transform.Find("LvlReq");
            Text LvlReqText = LvlReq.GetComponent<Text>();
            LvlReqText.color = MyColor.White;
            LvlReqText.text = "Level Required: " + E.LvlReq.ToString();
            LvlReq.localPosition = LastUpdatePosition + Interval;
            LastUpdatePosition = LvlReq.localPosition;
            LvlReq.gameObject.SetActive(true);

            if (E.AddHealth > 0) {
                Transform AddHealth = Stats.transform.Find("AddHealth");
                Text AddHealthText = AddHealth.GetComponent<Text>();
                AddHealthText.color = MyColor.White;
                AddHealthText.text = "HP: +" + E.AddHealth;
                AddHealth.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddHealth.localPosition;
                AddHealth.gameObject.SetActive(true);
            } else {
                Transform AddHealth = Stats.transform.Find("AddHealth");
                AddHealth.gameObject.SetActive(false);
            }
            if (E.AddMana > 0) {
                Transform AddMana = Stats.transform.Find("AddMana");
                Text AddManaText = AddMana.GetComponent<Text>();
                AddManaText.color = MyColor.White;
                AddManaText.text = "MP: +" + E.AddMana;
                AddMana.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddMana.localPosition;
                AddMana.gameObject.SetActive(true);
            } else {
                Transform AddMana = Stats.transform.Find("AddMana");
                AddMana.gameObject.SetActive(false);
            }
            if (E.AddAD > 0) {
                Transform AddAD = Stats.transform.Find("AddAD");
                Text AddADText = AddAD.GetComponent<Text>();
                AddADText.color = MyColor.White;
                AddADText.text = "AD: +" + E.AddAD;
                AddAD.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddAD.localPosition;
                AddAD.gameObject.SetActive(true);
            } else {
                Transform AddAD = Stats.transform.Find("AddAD");
                AddAD.gameObject.SetActive(false);
            }
            if (E.AddMD > 0) {
                Transform AddMD = Stats.transform.Find("AddMD");
                Text AddMDText = AddMD.GetComponent<Text>();
                AddMDText.color = MyColor.White;
                AddMDText.text = "MD: +" + E.AddMD;
                AddMD.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddMD.localPosition;
                AddMD.gameObject.SetActive(true);
            } else {
                Transform AddMD = Stats.transform.Find("AddMD");
                AddMD.gameObject.SetActive(false);
            }
            if (E.AddAttkSpd > 0) {
                Transform AddAttkSpd = Stats.transform.Find("AddAttkSpd");
                Text AddAttkSpdText = AddAttkSpd.GetComponent<Text>();
                AddAttkSpdText.color = MyColor.White;
                AddAttkSpdText.text = "Attack Speed: +" + E.AddAttkSpd + "%";
                AddAttkSpd.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddAttkSpd.localPosition;
                AddAttkSpd.gameObject.SetActive(true);
            } else {
                Transform AddAttkSpd = Stats.transform.Find("AddAttkSpd");
                AddAttkSpd.gameObject.SetActive(false);
            }
            if (E.AddMoveSpd > 0) {
                Transform AddMoveSpd = Stats.transform.Find("AddMoveSpd");
                Text AddMoveSpdText = AddMoveSpd.GetComponent<Text>();
                AddMoveSpdText.color = MyColor.White;
                AddMoveSpdText.text = "Move Speed: +" + E.AddMoveSpd + "%";
                AddMoveSpd.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddMoveSpd.localPosition;
                AddMoveSpd.gameObject.SetActive(true);
            } else {
                Transform AddMoveSpd = Stats.transform.Find("AddMoveSpd");
                AddMoveSpd.gameObject.SetActive(false);
            }
            if (E.AddDefense > 0) {
                Transform AddDefense = Stats.transform.Find("AddDefense");
                Text AddDefenseText = AddDefense.GetComponent<Text>();
                AddDefenseText.color = MyColor.White;
                AddDefenseText.text = "Defense: +" + E.AddDefense + "%";
                AddDefense.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddDefense.localPosition;
                AddDefense.gameObject.SetActive(true);
            } else {
                Transform AddDefense = Stats.transform.Find("AddDefense");
                AddDefense.gameObject.SetActive(false);
            }
            if (E.AddCritChance > 0) {
                Transform AddCritChance = Stats.transform.Find("AddCritChance");
                Text AddCritChanceText = AddCritChance.GetComponent<Text>();
                AddCritChanceText.color = MyColor.White;
                AddCritChanceText.text = "Crit Chance: +" + E.AddCritChance + "%";
                AddCritChance.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddCritChance.localPosition;
                AddCritChance.gameObject.SetActive(true);
            } else {
                Transform AddCritChance = Stats.transform.Find("AddCritChance");
                AddCritChance.gameObject.SetActive(false);
            }
            if (E.AddCritDmgBounus > 0) {
                Transform AddCritDmgBounus = Stats.transform.Find("AddCritDmgBounus");
                Text AddCritDmgBounusText = AddCritDmgBounus.GetComponent<Text>();
                AddCritDmgBounusText.color = MyColor.White;
                AddCritDmgBounusText.text = "Crit Damage: +" + E.AddCritDmgBounus + "%";
                AddCritDmgBounus.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddCritDmgBounus.localPosition;
                AddCritDmgBounus.gameObject.SetActive(true);
            } else {
                Transform AddCritDmgBounus = Stats.transform.Find("AddCritDmgBounus");
                AddCritDmgBounus.gameObject.SetActive(false);
            }
            if (E.AddLPH > 0) {
                Transform AddLPH = Stats.transform.Find("AddLPH");
                Text AddLPHText = AddLPH.GetComponent<Text>();
                AddLPHText.color = MyColor.White;
                AddLPHText.text = "Life/Hit: +" + E.AddLPH;
                AddLPH.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddLPH.localPosition;
                AddLPH.gameObject.SetActive(true);
            } else {
                Transform AddLPH = Stats.transform.Find("AddLPH");
                AddLPH.gameObject.SetActive(false);
            }
            if (E.AddMPH > 0) {
                Transform AddMPH = Stats.transform.Find("AddMPH");
                Text AddMPHText = AddMPH.GetComponent<Text>();
                AddMPHText.color = MyColor.White;
                AddMPHText.text = "Mana/Hit: +" + E.AddLPH;
                AddMPH.localPosition = LastUpdatePosition + Interval;
                LastUpdatePosition = AddMPH.localPosition;
                AddMPH.gameObject.SetActive(true);
            } else {
                Transform AddMPH = Stats.transform.Find("AddMPH");
                AddMPH.gameObject.SetActive(false);
            }
        } else {
            //StatsText.gameObject.SetActive(false);
            Stats.gameObject.SetActive(false);
        }
    }
}
