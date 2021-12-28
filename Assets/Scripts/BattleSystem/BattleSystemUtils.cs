using System.Collections.Generic;
using UnityEngine;

public class BattleSystemUtils
{
    public static List<GameObject> GetChildren(GameObject obj)
    {
        List<GameObject> output = new List<GameObject>();

        foreach (Transform child in obj.transform)
        {
            output.Add(child.gameObject);
        }

        return output;
    }
}
