using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    ControllerManager CM;

    PlayerController PC;
    Transform HealthMask;
    Transform ManaMask;
    Transform ExpMask;

    MenuController MC;
    CharacterSheetController CSC;

    GameObject ES;

	void Awake () {
        if (ControllerManager.Instance)
            CM = ControllerManager.Instance;
        else
            CM = FindObjectOfType<ControllerManager>();

        PC = transform.parent.Find("PlayerController").GetComponent<PlayerController>();
        HealthMask = transform.Find("Action Bar/Health Orb");
        ManaMask = transform.Find("Action Bar/Mana Orb");
        ExpMask = transform.Find("Action Bar/XP mask");

        MC = transform.Find("Menu").GetComponent<MenuController>();
        CSC = transform.Find("CharacterSheet").GetComponent<CharacterSheetController>();
    }

    void Start() {
    }
	
	// Update is called once per frame
	void Update () {
        PopUpUIUpdate();
        UpdateExpBar();
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

    public void UpdateHealthManaBar() {
        if(PC.CurrHealth/PC.MaxHealth>=0)
            HealthMask.transform.localScale = new Vector2(1, PC.CurrHealth / PC.MaxHealth);
        else
            HealthMask.transform.localScale = new Vector2(1, 0);
        if (PC.CurrMana / PC.MaxMana >= 0)
            ManaMask.transform.localScale = new Vector2( 1, PC.CurrMana / PC.MaxMana);
        else
            ManaMask.transform.localScale = new Vector2(1,0);
    }

    public void UpdateExpBar() {
        ExpMask.GetComponent<Image>().fillAmount = ((float)PC.PlayerData.exp / (float)PC.GetNextLvlExp());
    }

}
