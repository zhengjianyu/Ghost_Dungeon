using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {
    [HideInInspector]
    public SkillData SkillData; //For runtime data fetching

    //For designing purpose only
    public string Name;
    public string Description;

    protected virtual void Awake() {
        SkillData = ScriptableObject.CreateInstance<SkillData>();
        SkillData.Name = Name;
        SkillData.Description = Description;
    }

    public virtual void InitSkill(int lvl) {
        //SkillData = ScriptableObject.CreateInstance<SkillData>();
        //SkillData.Name = Name;
        //SkillData.Description = Description;
        SkillData.lvl = lvl;
    }

    // Use this for initialization
    protected virtual void Start () {
	    
	}

    // Update is called once per frame
    protected virtual void Update () {
	
	}
}
