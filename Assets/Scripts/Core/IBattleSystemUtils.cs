using System.Collections.Generic;
using UnityEngine;

public interface IBattleSystemUtils
{
    List<GameObject> GetChildren(GameObject obj);
    IMinionBehaviour GetMinionBehaviour(GameObject obj);
    RaycastHit2D GetNearestCollider();
}