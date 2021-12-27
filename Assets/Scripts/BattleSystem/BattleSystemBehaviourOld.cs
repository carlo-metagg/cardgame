using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemBehaviourOld : MonoBehaviour
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
        card.GetComponent<MinionBehaviourOld>().CardData = entry;

        Transform cardTransform = card.transform;
        cardTransform.SetParent(playerHand.transform);
    }

    void Update()
    {
        
    }
}
