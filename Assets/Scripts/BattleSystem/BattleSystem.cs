using UnityEngine;

public class BattleSystem
{
    private readonly ISpawner _spawner;

    public BattleSystem(ISpawner spawner)
    {
        _spawner = spawner;
    }

    public void PreparePlayArea()
    {
        _spawner.InstantiateCards();
    }
}