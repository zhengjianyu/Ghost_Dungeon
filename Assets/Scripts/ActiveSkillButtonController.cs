using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveSkillButtonController : MonoBehaviour {
    PlayerController PC;
    private int Slot = -999;
    float CD = 0;

    ControllerManager CM;

    private ActiveSkill Skill;

    private KeyCode K_Key;
    private string J_Key;

    private Image CD_Mask;
    private Transform BG;

    void Awake() {
    }
	// Use this for initialization
	void Start () {
        if (ControllerManager.Instance) {
            CM = ControllerManager.Instance;
        } else
            CM = FindObjectOfType<ControllerManager>();
        Slot = int.Parse(gameObject.name);
        CD_Mask = transform.Find("CD_Mask").GetComponent<Image>();
        BG = transform.parent;
        GetComponent<Button>().onClick.AddListener(ActiveSkill);
        switch (Slot) {
            case 0:
                K_Key = CM.Skill0;
                J_Key = CM.J_LB;
                break;
            case 1:
                K_Key = CM.Skill1;
                J_Key = CM.J_RB;
                break;
            case 2:
                K_Key = CM.Skill2;
                J_Key = CM.J_LTRT;
                break;
            case 3:
                K_Key = CM.Skill3;
                J_Key = CM.J_LTRT;
                break;
        }
        PC = GameObject.Find("MainPlayer/PlayerController").transform.GetComponent<PlayerController>();
        Skill = PC.GetActiveSlotSkillTransform(Slot);
    }
	
	// Update is called once per frame
	void Update () {
        var pointer = new PointerEventData(EventSystem.current);
        if (J_Key == CM.J_LTRT && K_Key == CM.Skill2) {
            if (Input.GetKeyDown(K_Key) || Input.GetAxisRaw(J_Key) < 0)
                ExecuteEvents.Execute(this.gameObject, pointer, ExecuteEvents.submitHandler);
        } else if (J_Key == CM.J_LTRT && K_Key == CM.Skill3) {
            if (Input.GetKeyDown(K_Key) || Input.GetAxisRaw(J_Key) > 0)
                ExecuteEvents.Execute(this.gameObject, pointer, ExecuteEvents.submitHandler);
        }
        else if(Input.GetKeyDown(K_Key)|| Input.GetKeyDown(J_Key)) {
            ExecuteEvents.Execute(this.gameObject, pointer, ExecuteEvents.submitHandler);
        }
        if(Skill)
            UpdateCDMask();
    }

    void ActiveSkill() {
        if (PC.Stunned)
            return;
        if (Skill && CD == 0) {
            Skill.Active();
            CD = Skill.CD;
        }
    }

    void UpdateCDMask() {
        if (CD - Time.deltaTime <= 0 && CD!=0) {
            BG.GetComponent<Animator>().Play("bg_blank");
        }

        if (CD > 0) {
            CD -= Time.deltaTime;
        } else
            CD = 0;
        CD_Mask.fillAmount = CD/Skill.CD;
    }
}
