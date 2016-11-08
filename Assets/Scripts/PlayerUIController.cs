using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    ControllerManager CM;

    PlayerController PC;
    Transform HealthMask;
    Transform ManaMask;

    MenuController MC;
    CharacterSheetController CSC;

    GameObject ES;

	void Awake () {
        if (ControllerManager.Instance)
            CM = ControllerManager.Instance;
        else
            CM = FindObjectOfType<ControllerManager>();

        PC = transform.parent.Find("PlayerController").GetComponent<PlayerController>();
        HealthMask = transform.Find("Health Bar/Mask");
        ManaMask = transform.Find("Mana Bar/Mask");

        MC = transform.Find("Menu").GetComponent<MenuController>();
        CSC = transform.Find("CharacterSheet").GetComponent<CharacterSheetController>();
    }

    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
        PopUpUIUpdate();
        UpdateHealthManaBar();
    }

    private void PopUpUIUpdate() {
        if (Input.GetKeyDown(CM.Menu) || Input.GetKeyDown(CM.J_Start)) {
            CM.MoveVector = Vector2.zero;
            CM.AttackVector = Vector2.zero;
            if (CSC.IsOn())
                CSC.TurnOff();
            else
                MC.Toggle();

        } else if (Input.GetKeyDown(CM.CharacterSheet) || Input.GetKeyDown(CM.J_X)) {
            CM.MoveVector = Vector2.zero;
            CM.AttackVector = Vector2.zero;
            if (MC.IsOn())
                MC.TurnOff();
            CSC.Toggle();
        } else if (Input.GetKeyDown(CM.J_B)) {
            if (CSC.IsOn())
                CSC.TurnOff();
            if (MC.IsOn())
                MC.TurnOff();
        }
        if (MC.IsOn() || CSC.IsOn())
            CM.AllowControlUpdate = false;
        else
            CM.AllowControlUpdate = true;
    }

    void UpdateHealthManaBar() {
        if(PC.CurrHealth/PC.MaxHealth>=0)
            HealthMask.transform.localScale = new Vector2(PC.CurrHealth / PC.MaxHealth, 1);
        else
            HealthMask.transform.localScale = new Vector2(0, 1);
        if (PC.CurrMana / PC.MaxMana >= 0)
            ManaMask.transform.localScale = new Vector2(PC.CurrMana / PC.MaxMana, 1);
        else
            ManaMask.transform.localScale = new Vector2(0, 1);
    }

}
