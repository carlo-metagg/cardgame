using System.Collections.Generic;
using UnityEngine;

public class BattleSystemUtils
{
    public List<GameObject> GetChildren(GameObject obj)
    {
        List<GameObject> output = new List<GameObject>();

        foreach (Transform child in obj.transform)
        {
            output.Add(child.gameObject);
        }

        return output;
    }

    public RaycastHit2D GetNearestCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        return hit;
    }
}
