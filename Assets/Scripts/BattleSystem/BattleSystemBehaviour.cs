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
    }

    private void InstantiateCards()
    {
        MinionCardData[] entries = Resources.LoadAll<MinionCardData>("Minions");

        foreach (MinionCardData entry in entries)
        {
            SpawnCard(entry);
        }
    }

    private void SpawnCard(MinionCardData entry)
    {
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<MinionCardDisplay>().CardData = entry;

        Transform cardTransform = card.transform;
        cardTransform.SetParent(playerHand.transform);
    }

    void Update()
    {
        
    }
}
