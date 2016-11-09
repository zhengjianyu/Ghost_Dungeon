using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryButtonController : MonoBehaviour {
    PlayerController PC;
    GameObject EquipmentIcon;
    private int Slot = -999;

    GameObject ES;

    Text StatsText;
    public GameObject Stats;

    Equipment E = null;

    void Awake() {
        Slot = int.Parse(gameObject.name);
        PC = GameObject.Find("MainPlayer/PlayerController").transform.GetComponent<PlayerController>();
        ES = GameObject.Find("EventSystem");
        Stats = Instantiate(Stats,transform) as GameObject;
        Stats.transform.localPosition = new Vector3(0, 0, 0);
        Stats.transform.localScale = new Vector3(1.2f, 1.2f,0);
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        UpdateStats();
        UpdateSlot();
    }

    public void OnClickEquip() {
        if (PC.GetInventoryItem(Slot)!=null) {
            if (PC.Compatible(E)) {
                if (PC.GetEquippedItem(E.Type)!=null) {//Has Equipped Item
                    Equipment TakeOff = PC.GetEquippedItem(E.Type);
                    PC.UnEquip(TakeOff.Type);
                    PC.RemoveFromInventory(Slot, E);
                    PC.Equip(E);
                    PC.AddToInventory(Slot, TakeOff);                   
                } else {
                    PC.Equip(E);
                    PC.RemoveFromInventory(Slot, E);
                }
            } else {
                Debug.Log("You can't equip this item.");
            }
        }
    }

    public void UpdateSlot() {
        //Debug.Log(E.Name);
        if (PC.GetInventoryItem(Slot) == E) {
            return;
        } else {
            E = PC.GetInventoryItem(Slot);
            if (E != null) {
                if (EquipmentIcon != null)
                    DestroyObject(EquipmentIcon);
                EquipmentIcon = EquipmentController.ObtainInventoryIcon(E,transform);
            } else {
                DestroyObject(EquipmentIcon);
                EquipmentIcon = null;
            }
        }
    }

    void UpdateStats() {
        if (E!=null  && ES.GetComponent<UnityEngine.EventSystems.EventSystem>().currentSelectedGameObject == gameObject) {
            Stats.gameObject.SetActive(true);

            Vector2 Interval = new Vector2(0,-50f);
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
            if (E.Class == PC.PlayerData.Class || E.Class == "All")
                ClassText.color = MyColor.Green;
            else
                ClassText.color = MyColor.Red;
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
            if (E.LvlReq > PC.PlayerData.lvl)
                LvlReqText.color = MyColor.Red;
            else
                LvlReqText.color = MyColor.Green;
            LvlReqText.text = "Level Required: "+ E.LvlReq.ToString();
            LvlReq.localPosition = LastUpdatePosition + Interval;
            LastUpdatePosition = LvlReq.localPosition;
            LvlReq.gameObject.SetActive(true);

            Equipment _E = PC.GetEquippedItem(E.Type);
            if (_E != null) {
                if (E.AddHealth > 0 || _E.AddHealth>0) {
                    Transform AddHealth = Stats.transform.Find("AddHealth");
                    Text AddHealthText = AddHealth.GetComponent<Text>();
                    if (E.AddHealth > _E.AddHealth)
                        AddHealthText.color = MyColor.Green;
                    else if (E.AddHealth < _E.AddHealth)
                        AddHealthText.color = MyColor.Red;
                    else
                        AddHealthText.color = MyColor.White;
                    float delta = E.AddHealth - _E.AddHealth;
                    AddHealthText.text = "HP: +" + E.AddHealth + " <" + (delta < 0 ? "" : "+") + delta + ">";
                    AddHealth.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddHealth.localPosition;
                    AddHealth.gameObject.SetActive(true);
                } else {
                    Transform AddHealth = Stats.transform.Find("AddHealth");
                    AddHealth.gameObject.SetActive(false);
                }
                if (E.AddMana > 0 || _E.AddMana>0) {
                    Transform AddMana = Stats.transform.Find("AddMana");
                    Text AddManaText = AddMana.GetComponent<Text>();
                    if (E.AddMana > _E.AddMana)
                        AddManaText.color = MyColor.Green;
                    else if (E.AddMana < _E.AddMana)
                        AddManaText.color = MyColor.Red;
                    else
                        AddManaText.color = MyColor.White;
                    float delta = E.AddMana - _E.AddMana;
                    AddManaText.text = "MP: +" + E.AddMana + " <" + (delta < 0 ? "" : "+") + delta + ">";
                    AddMana.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddMana.localPosition;
                    AddMana.gameObject.SetActive(true);
                } else {
                    Transform AddMana = Stats.transform.Find("AddMana");
                    AddMana.gameObject.SetActive(false);
                }
                if (E.AddAD > 0 || _E.AddAD>0) {
                    Transform AddAD = Stats.transform.Find("AddAD");
                    Text AddADText = AddAD.GetComponent<Text>();
                    if (E.AddAD > _E.AddAD)
                        AddADText.color = MyColor.Green;
                    else if (E.AddAD < _E.AddAD)
                        AddADText.color = MyColor.Red;
                    else
                        AddADText.color = MyColor.White;
                    float delta = E.AddAD - _E.AddAD;
                    AddADText.text = "AD: +" + E.AddAD + " <" + (delta < 0 ? "" : "+") + delta + ">";
                    AddAD.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddAD.localPosition;
                    AddAD.gameObject.SetActive(true);
                } else {
                    Transform AddAD = Stats.transform.Find("AddAD");
                    AddAD.gameObject.SetActive(false);
                }
                if (E.AddMD > 0 || _E.AddMD>0) {
                    Transform AddMD = Stats.transform.Find("AddMD");
                    Text AddMDText = AddMD.GetComponent<Text>();
                    if (E.AddMD > _E.AddMD)
                        AddMDText.color = MyColor.Green;
                    else if (E.AddMD < _E.AddMD)
                        AddMDText.color = MyColor.Red;
                    else
                        AddMDText.color = MyColor.White;
                    float delta = E.AddMD - _E.AddMD;
                    AddMDText.text = "MD: +" + E.AddMD + " <" + (delta<0?"":"+")+delta + ">";
                    AddMD.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddMD.localPosition;
                    AddMD.gameObject.SetActive(true);
                } else {
                    Transform AddMD = Stats.transform.Find("AddMD");
                    AddMD.gameObject.SetActive(false);
                }
                if (E.AddAttkSpd > 0 || _E.AddAttkSpd>0) {
                    Transform AddAttkSpd = Stats.transform.Find("AddAttkSpd");
                    Text AddAttkSpdText = AddAttkSpd.GetComponent<Text>();
                    if (E.AddAttkSpd > _E.AddAttkSpd)
                        AddAttkSpdText.color = MyColor.Green;
                    else if (E.AddAttkSpd < _E.AddAttkSpd)
                        AddAttkSpdText.color = MyColor.Red;
                    else
                        AddAttkSpdText.color = MyColor.White;
                    float delta = E.AddAttkSpd - _E.AddAttkSpd;
                    AddAttkSpdText.text = "Attack Speed: +" + E.AddAttkSpd + "% <" + (delta < 0 ? "" : "+") + delta + "%>";
                    AddAttkSpd.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddAttkSpd.localPosition;
                    AddAttkSpd.gameObject.SetActive(true);
                } else {
                    Transform AddAttkSpd = Stats.transform.Find("AddAttkSpd");
                    AddAttkSpd.gameObject.SetActive(false);
                }
                if (E.AddMoveSpd > 0 || _E.AddMoveSpd>0) {
                    Transform AddMoveSpd = Stats.transform.Find("AddMoveSpd");
                    Text AddMoveSpdText = AddMoveSpd.GetComponent<Text>();
                    if (E.AddMoveSpd > _E.AddMoveSpd)
                        AddMoveSpdText.color = MyColor.Green;
                    else if (E.AddMoveSpd < _E.AddMoveSpd)
                        AddMoveSpdText.color = MyColor.Red;
                    else
                        AddMoveSpdText.color = MyColor.White;
                    float delta = E.AddMoveSpd - _E.AddMoveSpd;
                    AddMoveSpdText.text = "Move Speed: +" + E.AddMoveSpd + "% <" + (delta < 0 ? "" : "+") + delta + "%>";
                    AddMoveSpd.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddMoveSpd.localPosition;
                    AddMoveSpd.gameObject.SetActive(true);
                } else {
                    Transform AddMoveSpd = Stats.transform.Find("AddMoveSpd");
                    AddMoveSpd.gameObject.SetActive(false);
                }
                if (E.AddDefense > 0 || _E.AddDefense>0) {
                    Transform AddDefense = Stats.transform.Find("AddDefense");
                    Text AddDefenseText = AddDefense.GetComponent<Text>();
                    if (E.AddDefense > _E.AddDefense)
                        AddDefenseText.color = MyColor.Green;
                    else if (E.AddDefense < _E.AddDefense)
                        AddDefenseText.color = MyColor.Red;
                    else
                        AddDefenseText.color = MyColor.White;
                    float delta = E.AddDefense - _E.AddDefense;
                    AddDefenseText.text = "Defense: +" + E.AddDefense + "% <" + (delta < 0 ? "" : "+") + delta + "%>";
                    AddDefense.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddDefense.localPosition;
                    AddDefense.gameObject.SetActive(true);
                } else {
                    Transform AddDefense = Stats.transform.Find("AddDefense");
                    AddDefense.gameObject.SetActive(false);
                }
                if (E.AddCritChance > 0 || _E.AddCritChance>0) {
                    Transform AddCritChance = Stats.transform.Find("AddCritChance");
                    Text AddCritChanceText = AddCritChance.GetComponent<Text>();
                    if (E.AddCritChance > _E.AddCritChance)
                        AddCritChanceText.color = MyColor.Green;
                    else if (E.AddCritChance < _E.AddCritChance)
                        AddCritChanceText.color = MyColor.Red;
                    else
                        AddCritChanceText.color = MyColor.White;
                    float delta = E.AddCritChance - _E.AddCritChance;
                    AddCritChanceText.text = "Crit Chance: +" + E.AddCritChance + "% <" + (delta < 0 ? "" : "+") + delta + "%>";
                    AddCritChance.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddCritChance.localPosition;
                    AddCritChance.gameObject.SetActive(true);
                } else {
                    Transform AddCritChance = Stats.transform.Find("AddCritChance");
                    AddCritChance.gameObject.SetActive(false);
                }
                if (E.AddCritDmgBounus > 0 || _E.AddCritDmgBounus>0) {
                    Transform AddCritDmgBounus = Stats.transform.Find("AddCritDmgBounus");
                    Text AddCritDmgBounusText = AddCritDmgBounus.GetComponent<Text>();
                    if (E.AddCritDmgBounus > _E.AddCritDmgBounus)
                        AddCritDmgBounusText.color = MyColor.Green;
                    else if (E.AddCritDmgBounus < _E.AddCritDmgBounus)
                        AddCritDmgBounusText.color = MyColor.Red;
                    else
                        AddCritDmgBounusText.color = MyColor.White;
                    float delta = E.AddCritDmgBounus - _E.AddCritDmgBounus;
                    AddCritDmgBounusText.text = "Crit Damage: +" + E.AddCritDmgBounus + "% <" + (delta < 0 ? "" : "+") + delta + "%>";
                    AddCritDmgBounus.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddCritDmgBounus.localPosition;
                    AddCritDmgBounus.gameObject.SetActive(true);
                } else {
                    Transform AddCritDmgBounus = Stats.transform.Find("AddCritDmgBounus");
                    AddCritDmgBounus.gameObject.SetActive(false);
                }
                if (E.AddLPH > 0 || _E.AddLPH>0) {
                    Transform AddLPH = Stats.transform.Find("AddLPH");
                    Text AddLPHText = AddLPH.GetComponent<Text>();
                    if (E.AddLPH > _E.AddLPH)
                        AddLPHText.color = MyColor.Green;
                    else if (E.AddLPH < _E.AddLPH)
                        AddLPHText.color = MyColor.Red;
                    else
                        AddLPHText.color = MyColor.White;
                    AddLPHText.text = "Life/Hit: +" + E.AddLPH + " <" + (E.AddLPH - _E.AddLPH) + ">";
                    AddLPH.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddLPH.localPosition;
                    AddLPH.gameObject.SetActive(true);
                } else {
                    Transform AddLPH = Stats.transform.Find("AddLPH");
                    AddLPH.gameObject.SetActive(false);
                }
                if (E.AddMPH > 0 || _E.AddMPH>0) {
                    Transform AddMPH = Stats.transform.Find("AddMPH");
                    Text AddMPHText = AddMPH.GetComponent<Text>();
                    if (E.AddMPH > _E.AddMPH)
                        AddMPHText.color = MyColor.Green;
                    else if (E.AddMPH < _E.AddMPH)
                        AddMPHText.color = MyColor.Red;
                    else
                        AddMPHText.color = MyColor.White;
                    AddMPHText.text = "Mana/Hit: +" + E.AddMPH + " <" + (E.AddMPH - _E.AddMPH) + ">";
                    AddMPH.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddMPH.localPosition;
                    AddMPH.gameObject.SetActive(true);
                } else {
                    Transform AddMPH = Stats.transform.Find("AddMPH");
                    AddMPH.gameObject.SetActive(false);
                }
            } else {
                if (E.AddHealth > 0) {
                    Transform AddHealth = Stats.transform.Find("AddHealth");
                    Text AddHealthText = AddHealth.GetComponent<Text>();
                    AddHealthText.color = MyColor.Green;
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
                    AddManaText.color = MyColor.Green;
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
                    AddADText.color = MyColor.Green;
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
                    AddMDText.color = MyColor.Green;
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
                    AddAttkSpdText.color = MyColor.Green;
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
                    AddMoveSpdText.color = MyColor.Green;
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
                    AddDefenseText.color = MyColor.Green;
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
                    AddCritChanceText.color = MyColor.Green;
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
                    AddCritDmgBounusText.color = MyColor.Green;
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
                    AddLPHText.color = MyColor.Green;
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
                    AddMPHText.color = MyColor.Green;
                    AddMPHText.text = "Mana/Hit: +" + E.AddLPH;
                    AddMPH.localPosition = LastUpdatePosition + Interval;
                    LastUpdatePosition = AddMPH.localPosition;
                    AddMPH.gameObject.SetActive(true);
                }else {
                    Transform AddMPH = Stats.transform.Find("AddMPH");
                    AddMPH.gameObject.SetActive(false);
                }
            }
        } else {
            //StatsText.gameObject.SetActive(false);
            Stats.gameObject.SetActive(false);
        }
    }
}
