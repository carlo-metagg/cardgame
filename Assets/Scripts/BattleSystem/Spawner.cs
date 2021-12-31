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
        List<Vector3> positionOffsets = GeneratePositionOffset(entries.Count);

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject card = SpawnCard(entries[i]);
            SetInitialPosition(card.transform, positionOffsets[i]);
            InitialCardRescale(card.transform);
        }
    }

    private void InitialCardRescale(Transform cardTransform)
    {
        Vector3 originalScale = cardTransform.localScale;

        cardTransform.localScale *= scaleFactor;
        cardTransform.localScale = new Vector3(cardTransform.localScale.x, cardTransform.localScale.y, originalScale.z);
    }

    private void SetInitialPosition(Transform cardTransform, Vector3 positionOffset)
    {
        cardTransform.SetParent(playerHand.transform);
        cardTransform.localPosition = positionOffset;
    }

    public GameObject SpawnCard(MinionCardData entry)
    {
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<MinionBehaviour>().CardData = entry;

        return card;
    }

    private List<Vector3> GeneratePositionOffset(int length)
    {
        List<Vector3> output = new List<Vector3>();

        for (int i = 0; i < length; i++)
        {
            float compute = separationFactor * i;
            output.Add(new Vector3(compute, 0, 0));
        }

        return output;
    }
}
