using UnityEngine;

public interface ISpawner
{
    void InstantiateCards();
    GameObject SpawnCard(MinionCardData entry);
}