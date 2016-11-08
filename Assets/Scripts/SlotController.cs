using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SlotController : MonoBehaviour {
    int SlotIndex;

    Text Name;
    Text LvlClass;

    GameObject PlayButtonObject;
    GameObject DeleteButtonObject;
    GameObject CreateButtonObject;

    SaveLoadManager SLM;
    CharacterDataStruct PlayerData;

    private GameObject BaseModel;

    void Awake() {
        SlotIndex = (int)(transform.name[4])-48;
        Name = transform.Find("NameText").GetComponent<Text>();
        LvlClass = transform.Find("LvlClassText").GetComponent<Text>();

        PlayButtonObject = transform.Find("PlayButton").gameObject;
        DeleteButtonObject = transform.Find("DeleteButton").gameObject;
        CreateButtonObject = transform.Find("CreateButton").gameObject;
        if (SaveLoadManager.Instance)
            SLM = SaveLoadManager.Instance;
        else
            SLM = FindObjectOfType<SaveLoadManager>();
        PlayerData = SLM.LoadPlayerInfo(SlotIndex);
    }
            
    // Use this for initialization
	void Start () {
        LoadSlotData();
        if(PlayerData.Name != "null")
            InstaniateEquipment();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void PlayButtonOnClick() {
        SLM.SlotIndexToLoad = SlotIndex;
        Application.LoadLevel("Developing");
    }

    void LoadSlotData() {
        if(PlayerData.Name == "null") {
            Name.text = LvlClass.text = "";
            PlayButtonObject.SetActive(false);
            DeleteButtonObject.SetActive(false);
            CreateButtonObject.SetActive(true);
        }else {
            Name.text = PlayerData.Name;
            LvlClass.text = "lvl " + PlayerData.lvl + " " + PlayerData.Class;
            CreateButtonObject.SetActive(false);
        }
            
    }






























    void InstaniateEquipment() {
        if (PlayerData.Class == "Warrior") {
            BaseModel = Instantiate(Resources.Load("Red Ghost/Ghost/Red Ghost"), transform) as GameObject;
            BaseModel.name = "Red Ghost";
            BaseModel.transform.position = transform.position + BaseModel.transform.position;
        } else if (PlayerData.Class == "Mage") {

        } else if (PlayerData.Class == "Rogue") {

        }
        foreach (var e in PlayerData.Equipments) {
            if (e.Value != null) {
                GameObject equipPrefab = EquipmentController.ObtainPrefab(e.Value, transform);
            }
        }
    }
}