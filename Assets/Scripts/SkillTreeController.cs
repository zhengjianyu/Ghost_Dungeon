using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTreeController : MonoBehaviour {
    public Skill[] SkillTree;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InstantiateSkills() {
        PlayerController PC = transform.parent.GetComponent<PlayerController>();
        for (int i = 0; i < PC.PlayerData.SkillTreelvls.Length; i++) {
            if (SkillTree[i] != null && PC.PlayerData.SkillTreelvls[i] != 0) {
                GameObject SkillObject = Instantiate(Resources.Load("SkillPrefabs/" + SkillTree[i].Name)) as GameObject;
                if (SkillObject.transform.GetComponent<Skill>().GetType().IsSubclassOf(typeof(ActiveSkill)))
                    SkillObject.transform.parent = PC.Actives;
                else if (SkillObject.transform.GetComponent<Skill>().GetType().IsSubclassOf(typeof(PassiveSkill)))
                    SkillObject.transform.parent = PC.Passives;
                SkillObject.name = SkillTree[i].Name;
                SkillObject.GetComponent<Skill>().InitSkill(PC.PlayerData.SkillTreelvls[i]);
            }
        }
    }
   
}
