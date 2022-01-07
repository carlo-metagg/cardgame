using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    private readonly int INITIAL_DRAW_COUNT = 3;

    private readonly BattleSystemUtils _utils;
    private ISpawner _spawner;
    private readonly DeckManager _deckManager;
    private GameObject _playerHand;
    private readonly float _scaleFactor;
    private readonly float _separationFactor;

    private CardManagerState _state;

    public CardManager(ISpawner spawner,
                       BattleSystemUtils utils,
                       DeckManager deckManager,
                       GameObject playerHand,
                       float scaleFactor,
                       float separationFactor)
    {
        _spawner = spawner;
        _utils = utils;
        _deckManager = deckManager;
        _playerHand = playerHand;
        _scaleFactor = scaleFactor;
        _separationFactor = separationFactor;

        ChangeState(CardManagerState.Starting);
    }

    private void ChangeState(CardManagerState state)
    {
        _state = state;
    }

    public void Draw()
    {
        if (CardManagerState.Idle != _state) return;

        InstantiateCard();

        List<GameObject> children = _utils.GetChildren(_playerHand);
        List<Vector3> updatedPositions = GenerateCardPositionsOnPlayerHand(children.Count);

        for (int i = 0; i < children.Count; i++)
        {
            ChangeState(CardManagerState.Drawing);
            MinionBehaviour minionBehaviour = children[i].GetComponent<MinionBehaviour>();

            minionBehaviour.SetInitialPosition(updatedPositions[i]);
            minionBehaviour.LerpToInitialPosition(() => ChangeState(CardManagerState.Idle));
        }
    }

    public void InstantiateCards()
    {
        List<Vector3> targetPositions = GenerateCardPositionsOnPlayerHand(INITIAL_DRAW_COUNT);

        for (int i = 0; i < INITIAL_DRAW_COUNT; i++)
        {
            ChangeState(CardManagerState.Starting);
            GameObject card = InstantiateCard();

            //SetInitialPosition(card.transform, targetPositions[i]);
            card.GetComponent<MinionBehaviour>().SetInitialPosition(targetPositions[i]);
            card.GetComponent<MinionBehaviour>().LerpToInitialPosition(() => ChangeState(CardManagerState.Idle));
        }

    }

    private GameObject InstantiateCard()
    {
        GameObject card = _spawner.SpawnCard(_deckManager.DrawCard());
        SetPositionToDeckLocation(card.transform);
        InitialCardRescale(card.transform);
        return card;
    }

    private void SetPositionToDeckLocation(Transform cardTransform)
    {
        cardTransform.SetParent(_playerHand.transform);
        cardTransform.position = new Vector3(-7, 0, -1);
    }

    private void SetInitialPosition(Transform cardTransform, Vector3 targetPosition)
    {
        cardTransform.SetParent(_playerHand.transform);
        cardTransform.localPosition = targetPosition;
    }

    private void InitialCardRescale(Transform cardTransform)
    {
        Vector3 originalScale = cardTransform.localScale;

        cardTransform.localScale *= _scaleFactor;
        cardTransform.localScale = new Vector3(cardTransform.localScale.x, cardTransform.localScale.y, originalScale.z);
    }

    private List<Vector3> GenerateCardPositionsOnPlayerHand(int length)
    {
        List<Vector3> output = new List<Vector3>();
        int leftmostCardXPosition = 0;
        float rightmostCardXPosition = _separationFactor * (length - 1);

        float xAxisOffset = GetXAxisOffset(leftmostCardXPosition, rightmostCardXPosition);

        for (int i = 0; i < length; i++)
        {
            float compute = (_separationFactor * i) - xAxisOffset;
            output.Add(new Vector3(compute, 0, -i / 100f));
        }

        return output;
    }

    private float GetXAxisOffset(float leftmostCardXPosition, float rightmostCardXPosition) => Math.Abs((leftmostCardXPosition  + rightmostCardXPosition) / 2f);
}

public enum CardManagerState
{
    Starting,
    Idle,
    Drawing
}