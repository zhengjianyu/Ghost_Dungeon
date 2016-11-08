using UnityEngine;
using System.Collections;

public class DropList : MonoBehaviour {
    public GameObject[] Drops;

    //void Start() {
    //    SpawnLoots();
    //}

    public void SpawnLoots() {
        foreach (var i in Drops) {
            if (i != null)
                i.GetComponent<EquipmentController>().InstantiateLoot(transform);
        }
    }
}
