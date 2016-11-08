using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static float SFX_Volume = 1;

    public static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    ControllerManager CM;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        if (ControllerManager.Instance) {
            CM = ControllerManager.Instance;
        } else
            CM = FindObjectOfType<ControllerManager>();
    }

    void OnLevelWasLoaded() {
        
    }

    void Update() {

    }

    public void LoadSelectionScene() {
        Application.LoadLevel("Selection");
    }

    public void Exit() {
        Application.Quit();
    }

    public void LoadMenuScene() {
        Application.LoadLevel("Menu");
    }

}
