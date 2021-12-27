using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerHand;

    void Awake()
    {
        InstantiateCards();
        CenterCards();
    }

    private void CenterCards()
    {
        List<GameObject> children = GetChildren(playerHand);
        float xAxisOffset = GetXAxisOffset(children[0], children[children.Count - 1]);

        print(xAxisOffset);

        foreach (GameObject child in children)
        {
            child.transform.localPosition += new Vector3(xAxisOffset, 0f, 0f);
            child.GetComponent<MinionBehaviour>().SetCurrentPositionAsInitialPosition();
        }
    }

    private float GetXAxisOffset(GameObject rightmostCard, GameObject leftmostCard)
    {
        float canvasWidth = GetCanvasWidth(cardPrefab);

        float leftmostBorderXPosition = leftmostCard.transform.localPosition.x - canvasWidth;
        float rightmostBorderXPosition = rightmostCard.transform.localPosition.x + canvasWidth;

        return Math.Abs((leftmostBorderXPosition + rightmostBorderXPosition) / 2f);
    }

    private float GetCanvasWidth(GameObject obj)
    {
        return obj.GetComponentInChildren<RectTransform>().rect.width;
    }

    //put in utils static class
    private List<GameObject> GetChildren(GameObject obj)
    {
        List<GameObject> output = new List<GameObject>();

        foreach (Transform child in obj.transform)
        {
            output.Add(child.gameObject);
        }

        return output;
    }

    private void InstantiateCards()
    {
        List<MinionCardData> entries = new List<MinionCardData>(Resources.LoadAll<MinionCardData>("Minions"));

        List<Vector3> positionOffsets = GeneratePositionOffset(entries.Count);

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject card = SpawnCard(entries[i]);
            SetInitialPosition(card.transform, positionOffsets[i]);
            //print(card.transform.localScale);
        }
    }

    private List<Vector3> GeneratePositionOffset(int length)
    {
        List<Vector3> output = new List<Vector3>();
        float separation = -0.05f;

        for (int i = 0; i < length; i++)
        {
            float compute = separation * i;
            output.Add(new Vector3(compute, 0, 0));
        }

        return output;
    }

    private void SetInitialPosition(Transform cardTransform, Vector3 positionOffset)
    {
        cardTransform.SetParent(playerHand.transform);
        cardTransform.localPosition = positionOffset;
    }

    //TODO: how to implem scaling
    private GameObject SpawnCard(MinionCardData entry)
    {
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<MinionBehaviour>().CardData = entry;

        return card;
    }
}
