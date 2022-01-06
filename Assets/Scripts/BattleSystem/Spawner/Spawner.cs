using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject cardPrefab;

    public GameObject SpawnCard(MinionCardData entry)
    {
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<MinionBehaviour>().CardData = entry;

        return card;
    }
}
