using UnityEngine;

public class BattleSystem
{
    private GameObject _playerHand;
    private readonly ISpawner _spawner;
    private readonly BattleSystemUtils _utils;

    public BattleSystem(GameObject playerHand, ISpawner spawner, BattleSystemUtils utils)
    {
        _playerHand = playerHand;
        _spawner = spawner;
        _utils = utils;
    }

    public void PreparePlayArea()
    {
        _spawner.InstantiateCards();
        _utils.CenterCards(_utils.GetChildren(_playerHand));
    }
}