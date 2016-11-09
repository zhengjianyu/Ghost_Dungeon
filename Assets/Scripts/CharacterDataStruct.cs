using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct CharacterDataStruct{
    public int SlotIndex;
    public string Name;
    public string Class;
    public int lvl;
    public int paragon_lvl;
    public int exp;

    public int souls;

    public int StatPoints;
    public int SkillPoints;

    public float BaseHealth;
    public float BaseMana;
    public float BaseAD;
    public float BaseMD;
    public float BaseAttkSpd;
    public float BaseMoveSpd;
    public float BaseDefense;
    public float BaseCritChance;
    public float BaseCritDmgBounus;
    public float BaseLPH;
    public float BaseMPH;

    public int[] SkillTreelvls;

    public SkillData[] ActiveSlotData;

    public Dictionary<string, Equipment> Equipments;

    public Equipment[] Inventory;
}
