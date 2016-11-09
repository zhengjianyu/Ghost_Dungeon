using UnityEngine;
using System.Collections;

public class ActiveSkill : Skill {
    [HideInInspector]
    public float CD;
    protected override void Awake() {
        base.Awake();
    }

    public override void InitSkill(int lvl) {
        base.InitSkill(lvl);
    }

    protected override void Start () {
        base.Start();
	}

    protected override void Update () {
        base.Update();
	}

    public virtual void Active() {

    }
}
