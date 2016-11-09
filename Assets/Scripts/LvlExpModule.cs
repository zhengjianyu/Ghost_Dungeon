using UnityEngine;
using System.Collections;

public static class LvlExpModule{
    static public int LvlCap = 50;

    static public int GetRequiredExp(int NextLvl) {
        float e = 0;
        for (int i = 1; i < NextLvl; i++)
            e += Mathf.Floor(i + 300 * Mathf.Pow(2, (i / 7)));
        return (int)e;
        //return (int)Mathf.Floor(e / 4);
    }
}
