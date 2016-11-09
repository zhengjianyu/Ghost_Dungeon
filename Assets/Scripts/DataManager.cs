using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;


public static class DataManager {
    static private string FilePath = "Save/username_save.txt";
    static private string DataSeperator = ",";
    static private string SlotSeperator = "------SlotSeperator------";
    static private string CategorySeperator = "---CategorySeperator---";
    //static private string SkillPathSeperator = "-ActiveSkillSep-";
    static private string EquipmentSeperator = "-EquipSeperator-";

    static private int NumOfSlot = 6;
    static private CharacterDataStruct[] CharacterData = new CharacterDataStruct[NumOfSlot];

    static private int InventoryCapacity = 40;

    static private int SkillTreeSize = 18;

    static private int ActiveSkillSlots = 4;

    public static void Save() {
        StreamWriter SS = new StreamWriter(FilePath);
        for (int i = 0; i < NumOfSlot; i++) {
            WriteCharacter(SS, i);
            WriteSkillTreelvls(SS, i);
            WriteActiveSkills(SS, i);
            WriteEquipments(SS, i);
            WriteInventory(SS, i);
            if (i != NumOfSlot - 1)
                SS.Write(Environment.NewLine + SlotSeperator + Environment.NewLine);
        }

        SS.Close();
    }

    public static void Load() {
        if (!File.Exists(FilePath)) {//First Time Login
            InitSave();
            return;
        }
        StreamReader LoadStream = new StreamReader(FilePath);
        string SlotData = "";

        while (!LoadStream.EndOfStream) {
            SlotData += LoadStream.ReadLine();
        }
        string[] CharactersData = Regex.Split(SlotData, SlotSeperator);
        for (int i = 0; i < NumOfSlot; i++) {
            string[] ThisCharacterData = Regex.Split(CharactersData[i], CategorySeperator);
            int n = 0;
            string BaseData = ThisCharacterData[n++];
            string SkillTreelvlsData = ThisCharacterData[n++];
            string ActiveSkillsData = ThisCharacterData[n++];
            string EquipData = ThisCharacterData[n++];
            string InventoryData = ThisCharacterData[n++];
            ReadCharacter(i, BaseData);
            ReadSkillTreelvls(i, SkillTreelvlsData);
            ReadActiveSkills(i, ActiveSkillsData);
            ReadEquipments(i, EquipData);
            ReadInventory(i, InventoryData);
        }
        LoadStream.Close();
    }


    public static void SaveCharacter(CharacterDataStruct PlayerData) {
        CharacterData[PlayerData.SlotIndex] = PlayerData;
    }

    public static CharacterDataStruct LoadCharacter(int SlotIndex) {
        return CharacterData[SlotIndex];
    }



    //------------------Writing
    private static void WriteCharacter(StreamWriter SW, int SlotIndex) {
        SW.Write(
            CharacterData[SlotIndex].SlotIndex + DataSeperator +
            CharacterData[SlotIndex].Name + DataSeperator +
            CharacterData[SlotIndex].Class + DataSeperator +
            CharacterData[SlotIndex].lvl + DataSeperator +
            CharacterData[SlotIndex].paragon_lvl + DataSeperator +
            CharacterData[SlotIndex].exp + DataSeperator +

            CharacterData[SlotIndex].souls + DataSeperator +

            CharacterData[SlotIndex].StatPoints + DataSeperator +
            CharacterData[SlotIndex].SkillPoints + DataSeperator +

            CharacterData[SlotIndex].BaseHealth + DataSeperator +
            CharacterData[SlotIndex].BaseMana + DataSeperator +
            CharacterData[SlotIndex].BaseAD + DataSeperator +
            CharacterData[SlotIndex].BaseMD + DataSeperator +
            CharacterData[SlotIndex].BaseAttkSpd + DataSeperator +
            CharacterData[SlotIndex].BaseMoveSpd + DataSeperator +
            CharacterData[SlotIndex].BaseDefense + DataSeperator +
            CharacterData[SlotIndex].BaseCritChance + DataSeperator +
            CharacterData[SlotIndex].BaseCritDmgBounus + DataSeperator +
            CharacterData[SlotIndex].BaseLPH + DataSeperator +
            CharacterData[SlotIndex].BaseMPH +
            Environment.NewLine + CategorySeperator + Environment.NewLine);
    }

    private static void WriteSkillTreelvls(StreamWriter SW, int SlotIndex) {
        for(int s = 0; s < SkillTreeSize; s++) {
            if (s == SkillTreeSize - 1)
                SW.Write(CharacterData[SlotIndex].SkillTreelvls[s]);
            else
                SW.Write(CharacterData[SlotIndex].SkillTreelvls[s] + DataSeperator);
        }
        SW.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);
    }

    private static void WriteActiveSkills(StreamWriter SW, int SlotIndex) {
        for(int s = 0; s < ActiveSkillSlots; s++) {
            if (CharacterData[SlotIndex].ActiveSlotData[s]){
                if (s == ActiveSkillSlots - 1)
                    SW.Write(CharacterData[SlotIndex].ActiveSlotData[s].Name);
                else
                    SW.Write(CharacterData[SlotIndex].ActiveSlotData[s].Name + DataSeperator);
            } else {
                if (s == ActiveSkillSlots - 1)
                    SW.Write("null");
                else
                    SW.Write("null" + DataSeperator);
            }
        }
        SW.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);
    }

    private static void WriteEquipments(StreamWriter SW, int SlotIndex) {
        foreach (var e in CharacterData[SlotIndex].Equipments) {
            if (e.Value!=null) {
                SW.Write(
                    CharacterData[SlotIndex].Equipments[e.Key].Rarity + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].Name + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].Class + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].Type + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].LvlReq + DataSeperator +

                    CharacterData[SlotIndex].Equipments[e.Key].AddHealth + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddMana + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddAD + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddMD + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddAttkSpd + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddMoveSpd + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddDefense + DataSeperator +

                    CharacterData[SlotIndex].Equipments[e.Key].AddCritChance + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddCritDmgBounus + DataSeperator +

                    CharacterData[SlotIndex].Equipments[e.Key].AddLPH + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].AddMPH + DataSeperator +

                    CharacterData[SlotIndex].Equipments[e.Key].Reroll + DataSeperator +
                    CharacterData[SlotIndex].Equipments[e.Key].Reforged);
            } else {
                SW.Write("null");
            }
            if (e.Key !="Trinket")
                SW.Write(Environment.NewLine + EquipmentSeperator + Environment.NewLine);
        }
        SW.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);
    }

    private static void WriteInventory(StreamWriter SW, int SlotIndex) {
        for(int i =0;i< CharacterData[SlotIndex].Inventory.Length;i++) {
            if (CharacterData[SlotIndex].Inventory[i] != null) {
                SW.Write(
                    CharacterData[SlotIndex].Inventory[i].Rarity + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].Name + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].Class + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].Type + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].LvlReq + DataSeperator +

                    CharacterData[SlotIndex].Inventory[i].AddHealth + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddMana + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddAD + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddMD + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddAttkSpd + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddMoveSpd + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddDefense + DataSeperator +

                    CharacterData[SlotIndex].Inventory[i].AddCritChance + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddCritDmgBounus + DataSeperator +

                    CharacterData[SlotIndex].Inventory[i].AddLPH + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].AddMPH + DataSeperator +

                    CharacterData[SlotIndex].Inventory[i].Reroll + DataSeperator +
                    CharacterData[SlotIndex].Inventory[i].Reforged);
            } else {
                SW.Write("null");
            }
            if(i < CharacterData[SlotIndex].Inventory.Length - 1) {
                SW.Write(Environment.NewLine + EquipmentSeperator + Environment.NewLine);
            }
        }
        //SW.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);
    }



    //------------------Reading
    private static void ReadCharacter(int slotIndex, string BaseData) {
        string[] S_BaseData = Regex.Split(BaseData, DataSeperator);
        int i = 0;
        CharacterData[slotIndex].SlotIndex = int.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].Name = S_BaseData[i++];
        CharacterData[slotIndex].Class = S_BaseData[i++];
        CharacterData[slotIndex].lvl = int.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].paragon_lvl = int.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].exp = int.Parse(S_BaseData[i++]);

        CharacterData[slotIndex].souls = int.Parse(S_BaseData[i++]);

        CharacterData[slotIndex].StatPoints = int.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].SkillPoints = int.Parse(S_BaseData[i++]);

        CharacterData[slotIndex].BaseHealth = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseMana = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseAD = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseMD = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseAttkSpd = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseMoveSpd = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseDefense = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseCritChance = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseCritDmgBounus = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseLPH = float.Parse(S_BaseData[i++]);
        CharacterData[slotIndex].BaseMPH = float.Parse(S_BaseData[i++]);
    }

    private static void ReadSkillTreelvls(int SlotIndex, string SkillTreelvlsData) {
        string[] S_SkillTreelvlsData = Regex.Split(SkillTreelvlsData, DataSeperator);
        CharacterData[SlotIndex].SkillTreelvls = new int[SkillTreeSize];
        for (int s = 0; s < SkillTreeSize; s++) {
            CharacterData[SlotIndex].SkillTreelvls[s] = int.Parse(S_SkillTreelvlsData[s]);
        }
    }

    private static void ReadActiveSkills(int SlotIndex, string ActiveSkillsData) {
        string[] S_ActiveSkillsData = Regex.Split(ActiveSkillsData, DataSeperator);
        CharacterData[SlotIndex].ActiveSlotData = new SkillData[ActiveSkillSlots];
        for(int s = 0; s < ActiveSkillSlots; s++) {
            if (S_ActiveSkillsData[s] == "null")
                CharacterData[SlotIndex].ActiveSlotData[s] = null;
            else {
                SkillData SD = ScriptableObject.CreateInstance<SkillData>();
                SD.Name = S_ActiveSkillsData[s];
                CharacterData[SlotIndex].ActiveSlotData[s] = SD;
            }
        }
    }

    private static void ReadEquipments(int slotIndex, string EquipData) {
        string[] S_EquipData = Regex.Split(EquipData, EquipmentSeperator);
        CharacterData[slotIndex].Equipments = new Dictionary<string, Equipment>();
        for (int i = 0; i < S_EquipData.Length; i++) {
            switch (i) {
                case 0:
                    CharacterData[slotIndex].Equipments["Helmet"] = ReadOnePieceOfEquip(S_EquipData[i]);
                    break;
                case 1:
                    CharacterData[slotIndex].Equipments["Chest"] = ReadOnePieceOfEquip(S_EquipData[i]);
                    break;
                case 2:
                    CharacterData[slotIndex].Equipments["Shackle"] = ReadOnePieceOfEquip(S_EquipData[i]);
                    break;
                case 3:
                    CharacterData[slotIndex].Equipments["Weapon"] = ReadOnePieceOfEquip(S_EquipData[i]);
                    break;
                case 4:
                    CharacterData[slotIndex].Equipments["Trinket"] = ReadOnePieceOfEquip(S_EquipData[i]);
                    break;
            }
        }
    }

    static private void ReadInventory(int SlotIndex, string InventoryData) {
        string[] S_EquipData = Regex.Split(InventoryData, EquipmentSeperator);
        CharacterData[SlotIndex].Inventory = new Equipment[InventoryCapacity];
        for(int i =0;i< CharacterData[SlotIndex].Inventory.Length; i++) {
            CharacterData[SlotIndex].Inventory[i] = ReadOnePieceOfEquip(S_EquipData[i]);
        }
    }

    private static Equipment ReadOnePieceOfEquip(string PieceEquipData) {
        if (PieceEquipData == "null")
            return null;
        Equipment E = ScriptableObject.CreateInstance<Equipment>();
        string[] S_PieceEquipData = Regex.Split(PieceEquipData, DataSeperator);
        int i = 0;
        E.Rarity = int.Parse(S_PieceEquipData[i++]);
        E.Name = S_PieceEquipData[i++];
        E.Class = S_PieceEquipData[i++];
        E.Type = S_PieceEquipData[i++];
        E.LvlReq = int.Parse(S_PieceEquipData[i++]);

        E.AddHealth = float.Parse(S_PieceEquipData[i++]);
        E.AddMana = float.Parse(S_PieceEquipData[i++]);
        E.AddAD = float.Parse(S_PieceEquipData[i++]);
        E.AddMD = float.Parse(S_PieceEquipData[i++]);
        E.AddAttkSpd = float.Parse(S_PieceEquipData[i++]);
        E.AddMoveSpd = float.Parse(S_PieceEquipData[i++]);
        E.AddDefense = float.Parse(S_PieceEquipData[i++]);

        E.AddCritChance = float.Parse(S_PieceEquipData[i++]);
        E.AddCritDmgBounus = float.Parse(S_PieceEquipData[i++]);

        E.AddLPH = float.Parse(S_PieceEquipData[i++]);
        E.AddMPH = float.Parse(S_PieceEquipData[i++]);

        E.Reroll = int.Parse(S_PieceEquipData[i++]);
        E.Reforged = int.Parse(S_PieceEquipData[i++]);
        //Debug.Log(E.Name);
        return E;
    }

    //----------Save Init
    private static void InitSave() {//For server to init save file format
        StreamWriter SaveStream = new StreamWriter(FilePath);
        for (int slotIndex = 0; slotIndex < NumOfSlot; slotIndex++) {
            SaveStream.Write(
            //Base Stats
            slotIndex + DataSeperator +
            "null" + DataSeperator +
            "null" + DataSeperator +
            CharacterData[slotIndex].lvl + DataSeperator +
            CharacterData[slotIndex].paragon_lvl + DataSeperator +
            CharacterData[slotIndex].exp + DataSeperator +

            CharacterData[slotIndex].souls + DataSeperator +

            CharacterData[slotIndex].StatPoints + DataSeperator +
            CharacterData[slotIndex].SkillPoints + DataSeperator +

            CharacterData[slotIndex].BaseHealth + DataSeperator +
            CharacterData[slotIndex].BaseMana + DataSeperator +
            CharacterData[slotIndex].BaseAD + DataSeperator +
            CharacterData[slotIndex].BaseMD + DataSeperator +
            CharacterData[slotIndex].BaseAttkSpd + DataSeperator +
            CharacterData[slotIndex].BaseMoveSpd + DataSeperator +
            CharacterData[slotIndex].BaseDefense + DataSeperator +
            CharacterData[slotIndex].BaseCritChance + DataSeperator +
            CharacterData[slotIndex].BaseCritDmgBounus + DataSeperator +
            CharacterData[slotIndex].BaseLPH + DataSeperator +
            CharacterData[slotIndex].BaseMPH);

            SaveStream.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);

            //Skill Tree
            for (int s = 0; s < SkillTreeSize; s++) {
                if (s == SkillTreeSize - 1)
                    SaveStream.Write(0);
                else
                    SaveStream.Write(0 + DataSeperator);
            }
            SaveStream.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);

            //Active Skills
            for(int s = 0; s < ActiveSkillSlots; s++) {
                if (s == ActiveSkillSlots - 1)
                    SaveStream.Write("null");
                else
                    SaveStream.Write("null" + DataSeperator);
            }
            SaveStream.Write(Environment.NewLine + CategorySeperator + Environment.NewLine);

            //Equiped Items
            SaveStream.Write(
                "null" +
                Environment.NewLine + EquipmentSeperator + Environment.NewLine +
                "null" +
                Environment.NewLine + EquipmentSeperator + Environment.NewLine +
                "null" +
                Environment.NewLine + EquipmentSeperator + Environment.NewLine +
                "null" +
                Environment.NewLine + EquipmentSeperator + Environment.NewLine +
                "null" +

                Environment.NewLine + CategorySeperator + Environment.NewLine
            );

            //Inventory
            for(int i = 0; i < InventoryCapacity; i++) {
                SaveStream.Write("null");
                if (i < InventoryCapacity-1)
                    SaveStream.Write(Environment.NewLine+EquipmentSeperator + Environment.NewLine);
            }

            if (slotIndex != NumOfSlot - 1) {
                SaveStream.Write(Environment.NewLine+SlotSeperator + Environment.NewLine);
            }

        }
        SaveStream.Close();
    }
}
