using UnityEngine;
using System.Collections;

[System.Serializable]
public class Equipment :ScriptableObject{
    public int Rarity;//0 common and so on
    public string Name;
    public string Class;//For non-trinket equipment only
    public string Type;
    public int LvlReq;

    public float AddHealth;
    public float AddMana;
    public float AddAD;
    public float AddMD;
    public float AddAttkSpd;
    public float AddMoveSpd;
    public float AddDefense;

    public float AddCritChance; //Percantage
    public float AddCritDmgBounus; //Percantage

    public float AddLPH;
    public float AddMPH;

    public int Reroll; //NumofTime been rerolled
    public int Reforged; //NumofTime been reforged

}
