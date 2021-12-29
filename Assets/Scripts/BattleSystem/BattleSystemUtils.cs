using System;
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

    public float GetCanvasWidth(GameObject obj) => obj.GetComponentInChildren<RectTransform>().rect.width;

    public void CenterCards(List<GameObject> children)
    {
        float xAxisOffset = -GetXAxisOffset(children[0], children[children.Count - 1]);

        for (int i = 0; i < children.Count; i++)
        {
            GameObject child = children[i];

            child.transform.localPosition += new Vector3(xAxisOffset, 0f, -i / 10f);
            child.GetComponent<MinionBehaviour>().SetCurrentPositionAsInitialPosition();
        }
    }
    private float GetXAxisOffset(GameObject rightmostCard, GameObject leftmostCard)
    {
        float canvasWidth = GetCanvasWidth(rightmostCard);

        float leftmostBorderXPosition = leftmostCard.transform.localPosition.x - canvasWidth;
        float rightmostBorderXPosition = rightmostCard.transform.localPosition.x + canvasWidth;

        return Math.Abs((leftmostBorderXPosition + rightmostBorderXPosition) / 2f);
    }

    public RaycastHit2D GetNearestCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        return hit;
    }
}
