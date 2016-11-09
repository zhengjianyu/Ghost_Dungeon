using UnityEngine;
using System.Collections;

public class SaveLoadManager : MonoBehaviour {
    public int SlotIndexToLoad;

    public static SaveLoadManager instance;
    public static SaveLoadManager Instance { get { return instance; } }
    void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        }else {
            Destroy(gameObject);
        }
        DataManager.Load();
    }

    public void SaveCurrentPlayerInfo() {
        PlayerController PC = GameObject.Find("MainPlayer/PlayerController").GetComponent<PlayerController>();
        DataManager.SaveCharacter(PC.PlayerData);
        DataManager.Save();
    }

    public CharacterDataStruct LoadPlayerInfo(int SlotIndex) {
        return DataManager.LoadCharacter(SlotIndex);
    }
}
