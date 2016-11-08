using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;


public static class DataManager{
    static private string FilePath = "Save/username_save.txt";
    static private string DataSeperator = ",";
    static private string SlotSeperator = "------SlotSeperator------";
    static private string CategorySeperator = "---CategorySeperator---";
    static private string EquipmentSeperator = "-EquipSeperator-";
    static private int NumOfSlot = 6;
    static private CharacterDataStruct[] CharacterData = new CharacterDataStruct[NumOfSlot];

    public static void Save() {
        StreamWriter SaveStream = new StreamWriter(FilePath);
        for (int i = 0; i < NumOfSlot; i++) {
            WriteCharacter(SaveStream, i);
            WriteEquipments(SaveStream, i);
            WriteInventory(SaveStream, i);
            if (i != NumOfSlot - 1)
                SaveStream.Write(Environment.NewLine + SlotSeperator + Environment.NewLine);
        }

        SaveStream.Close();
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
            string BaseData = ThisCharacterData[0];
            string EquipData = ThisCharacterData[1];
            string InventoryData = ThisCharacterData[2];
            ReadCharacter(i, BaseData);
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
        CharacterData[slotIndex].SlotIndex = int.Parse(S_BaseData[0]);
        CharacterData[slotIndex].Name = S_BaseData[1];
        CharacterData[slotIndex].Class = S_BaseData[2];
        CharacterData[slotIndex].lvl = int.Parse(S_BaseData[3]);
        CharacterData[slotIndex].paragon_lvl = int.Parse(S_BaseData[4]);

        CharacterData[slotIndex].BaseHealth = float.Parse(S_BaseData[5]);
        CharacterData[slotIndex].BaseMana = float.Parse(S_BaseData[6]);
        CharacterData[slotIndex].BaseAD = float.Parse(S_BaseData[7]);
        CharacterData[slotIndex].BaseMD = float.Parse(S_BaseData[8]);
        CharacterData[slotIndex].BaseAttkSpd = float.Parse(S_BaseData[9]);
        CharacterData[slotIndex].BaseMoveSpd = float.Parse(S_BaseData[10]);
        CharacterData[slotIndex].BaseDefense = float.Parse(S_BaseData[11]);
        CharacterData[slotIndex].BaseCritChance = float.Parse(S_BaseData[12]);
        CharacterData[slotIndex].BaseCritDmgBounus = float.Parse(S_BaseData[13]);
        CharacterData[slotIndex].BaseLPH = float.Parse(S_BaseData[14]);
        CharacterData[slotIndex].BaseMPH = float.Parse(S_BaseData[15]);
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
        CharacterData[SlotIndex].Inventory = new Equipment[40];
        for(int i =0;i< CharacterData[SlotIndex].Inventory.Length; i++) {
            CharacterData[SlotIndex].Inventory[i] = ReadOnePieceOfEquip(S_EquipData[i]);
        }
    }

    private static Equipment ReadOnePieceOfEquip(string PieceEquipData) {
        if (PieceEquipData == "null")
            return null;
        //Equipment E = ScriptableObject.CreateInstance<Equipment>();
        Equipment E = new Equipment();
        string[] S_PieceEquipData = Regex.Split(PieceEquipData, DataSeperator);
        E.Rarity = int.Parse(S_PieceEquipData[0]);
        E.Name = S_PieceEquipData[1];
        E.Class = S_PieceEquipData[2];
        E.Type = S_PieceEquipData[3];
        E.LvlReq = int.Parse(S_PieceEquipData[4]);

        E.AddHealth = float.Parse(S_PieceEquipData[5]);
        E.AddMana = float.Parse(S_PieceEquipData[6]);
        E.AddAD = float.Parse(S_PieceEquipData[7]);
        E.AddMD = float.Parse(S_PieceEquipData[8]);
        E.AddAttkSpd = float.Parse(S_PieceEquipData[9]);
        E.AddMoveSpd = float.Parse(S_PieceEquipData[10]);
        E.AddDefense = float.Parse(S_PieceEquipData[11]);

        E.AddCritChance = float.Parse(S_PieceEquipData[12]);
        E.AddCritDmgBounus = float.Parse(S_PieceEquipData[13]);

        E.AddLPH = float.Parse(S_PieceEquipData[14]);
        E.AddMPH = float.Parse(S_PieceEquipData[15]);

        E.Reroll = int.Parse(S_PieceEquipData[16]);
        E.Reforged = int.Parse(S_PieceEquipData[17]);
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
                CharacterData[slotIndex].BaseMPH +

                Environment.NewLine + CategorySeperator + Environment.NewLine +
                //Equiped Items
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
            for(int i = 0; i < 40; i++) {
                SaveStream.Write("null");
                if (i < 39)
                    SaveStream.Write(Environment.NewLine+EquipmentSeperator + Environment.NewLine);
            }

            if (slotIndex != NumOfSlot - 1) {
                SaveStream.Write(Environment.NewLine+SlotSeperator + Environment.NewLine);
            }

        }
        SaveStream.Close();
    }
}
