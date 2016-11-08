using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
    public float DetectionRange = 1;
    public float AttackDistance = 0.3f;

    List<GameObject> TargetList;
    GameObject Target;
    [HideInInspector]
    public Vector2 MoveVector;
    [HideInInspector]
    public Vector2 AttackVector;
    [HideInInspector]
    public int Direction;
    // Use this for initialization
    void Awake() {
        TargetList = new List<GameObject>();
    }

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SearchTarget();
        LockOnClosestTarget();
        DiscardTarget();

        AttackVectorUpdate();
        MoveVectorUpdate();
        DirectionUpdate();
    }

    void SearchTarget() {
        if (Target == null) {
            foreach (var Player in GameObject.FindGameObjectsWithTag("Player")) {
                if (Vector2.Distance(transform.position, Player.transform.position) <= DetectionRange && !TargetList.Contains(Player)) {
                    TargetList.Add(Player);
                }
            }
        }
    }

    void DiscardTarget() {
        if (TargetList.Count == 0)
            return;
        foreach (var Player in GameObject.FindGameObjectsWithTag("Player")) {
            if (Vector2.Distance(transform.position, Player.transform.position) > DetectionRange) {
                TargetList.Remove(Player);
                if (Target == Player)
                    Target = null;
            }
        }
    }

    void LockOnClosestTarget() {
        if (TargetList.Count == 0)
            return;
        TargetList.Sort(delegate (GameObject a, GameObject b) {
            return Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position));
        });
        Target = TargetList[0];
    }

    //-------private
    void MoveVectorUpdate() {
        if (Target != null) {
            float dist = Vector2.Distance(Target.transform.position, transform.position);
            if (dist > AttackDistance) {
                MoveVector = (Vector2)Vector3.Normalize(Target.transform.position - transform.position);
                AttackVector = Vector2.zero;
            } else {
                MoveVector = Vector2.zero;
            }
        } else {
            MoveVector = Vector2.zero;
        }
    }

    void AttackVectorUpdate() {
        if (Target != null) {
            float dist = Vector2.Distance(Target.transform.position, transform.position);
            if (dist <= AttackDistance) {
                AttackVector = (Vector2)Vector3.Normalize(Target.transform.position - transform.position);
                MoveVector = Vector2.zero;
            } else {
                AttackVector = Vector2.zero;
            }
        } else {
            AttackVector = Vector2.zero;
        }
    }

    void DirectionUpdate() {
        if (MoveVector != Vector2.zero && AttackVector == Vector2.zero) {
            if (Vector2.Angle(MoveVector, Vector2.right) <= 45) {//Right
                Direction = 2;
            } else if (Vector2.Angle(MoveVector, Vector2.up) <= 45) {//Up
                Direction = 3;
            } else if (Vector2.Angle(MoveVector, Vector2.left) <= 45) {//Left
                Direction = 1;
            } else if (Vector2.Angle(MoveVector, Vector2.down) <= 45) {//Down
                Direction = 0;
            }
        } else if (AttackVector != Vector2.zero) {
            if (Vector2.Angle(AttackVector, Vector2.right) <= 45) {//Right
                Direction = 2;
            } else if (Vector2.Angle(AttackVector, Vector2.up) <= 45) {//Up
                Direction = 3;
            } else if (Vector2.Angle(AttackVector, Vector2.left) <= 45) {//Left
                Direction = 1;
            } else if (Vector2.Angle(AttackVector, Vector2.down) <= 45) {//Down
                Direction = 0;
            }
        }
    }
}
