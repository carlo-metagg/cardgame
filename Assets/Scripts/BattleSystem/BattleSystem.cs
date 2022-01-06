using UnityEngine;

public class BattleSystem
{
    private readonly ISpawner _spawner;
    private readonly CardManager _cardManager;

    public BattleSystem(ISpawner spawner, CardManager cardManager)
    {
        _spawner = spawner;
        _cardManager = cardManager;
    }

    public void PreparePlayArea()
    {
        _cardManager.InstantiateCards();
    }

    public void Draw()
    {
        _cardManager.Draw();
    }
}