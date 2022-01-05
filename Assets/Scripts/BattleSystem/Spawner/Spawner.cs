using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerHand;

    [Range(0, 1)]
    [SerializeField] private float scaleFactor = 0.75f;
    [SerializeField] private float separationFactor = 0.05f;

    public  void InstantiateCards()
    {
        List<MinionCardData> entries = new List<MinionCardData>(Resources.LoadAll<MinionCardData>("Minions"));
        List<Vector3> targetPositions = GenerateCardPositionsOnPlayerHand(entries.Count);

        for (int i = 0; i < entries.Count; i++)
        {
            float zAxis = -i / 10f;

            GameObject card = SpawnCard(entries[i]);
            SetInitialPosition(card.transform, zAxis);
            SetInitialPosition(card.transform, targetPositions[i]);
            InitialCardRescale(card.transform);

            card.GetComponent<MinionBehaviour>().SetInitialPosition(targetPositions[i]);
        }
    }

    private void InitialCardRescale(Transform cardTransform)
    {
        Vector3 originalScale = cardTransform.localScale;

        cardTransform.localScale *= scaleFactor;
        cardTransform.localScale = new Vector3(cardTransform.localScale.x, cardTransform.localScale.y, originalScale.z);
    }

    private void SetInitialPosition(Transform cardTransform, float zAxis)
    {
        cardTransform.SetParent(playerHand.transform);
        cardTransform.position = new Vector3(-7, 0, zAxis);
    }

    private void SetInitialPosition(Transform cardTransform, Vector3 initialPosition)
    {
        cardTransform.SetParent(playerHand.transform);
        cardTransform.localPosition = initialPosition;
    }

    public GameObject SpawnCard(MinionCardData entry)
    {
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<MinionBehaviour>().CardData = entry;

        return card;
    }

    private List<Vector3> GenerateCardPositionsOnPlayerHand(int length)
    {
        List<Vector3> output = new List<Vector3>();

        float xAxisOffset = GetXAxisOffset(0, separationFactor * (length - 1));

        for (int i = 0; i < length; i++)
        {
            float compute = (separationFactor * i) - xAxisOffset;
            output.Add(new Vector3(compute, 0, -i / 10f));
        }

        return output;
    }

    private float GetXAxisOffset(float leftmostCardXPosition, float rightmostCardXPosition)
    {
        float canvasWidth = GetCanvasWidth(cardPrefab);

        float leftmostBorderXPosition = leftmostCardXPosition - canvasWidth;
        float rightmostBorderXPosition = rightmostCardXPosition + canvasWidth;

        return Math.Abs((leftmostBorderXPosition + rightmostBorderXPosition) / 2f);
    }

    public float GetCanvasWidth(GameObject obj) => obj.GetComponentInChildren<RectTransform>().rect.width;
}
