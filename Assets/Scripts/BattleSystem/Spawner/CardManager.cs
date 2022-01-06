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
    private readonly GameObject _cardPrefab;
    private readonly float _scaleFactor;
    private readonly float _separationFactor;

    public CardManager(ISpawner spawner,
                       BattleSystemUtils utils,
                       DeckManager deckManager,
                       GameObject playerHand,
                       GameObject cardPrefab,
                       float scaleFactor, float separationFactor)
    {
        _spawner = spawner;
        _utils = utils;
        _deckManager = deckManager;
        _playerHand = playerHand;
        _cardPrefab = cardPrefab;
        _scaleFactor = scaleFactor;
        _separationFactor = separationFactor;
    }

    public void Draw()
    {
        GameObject card = _spawner.SpawnCard(_deckManager.DrawCard());
        SetPositionToDeckLocation(card.transform, -1);
        InitialCardRescale(card.transform);

        List<GameObject> children = _utils.GetChildren(_playerHand);
        List<Vector3> updatedPositions = GenerateCardPositionsOnPlayerHand(children.Count);

        for (int i = 0; i < children.Count; i++)
        {
            MinionBehaviour minionBehaviour = children[i].GetComponent<MinionBehaviour>();

            minionBehaviour.SetInitialPosition(updatedPositions[i]);
            minionBehaviour.ReturnToIntiialPosition();
        }
    }

    public void InstantiateCards()
    {
        List<Vector3> targetPositions = GenerateCardPositionsOnPlayerHand(INITIAL_DRAW_COUNT);

        for (int i = 0; i < INITIAL_DRAW_COUNT; i++)
        {
            float zAxis = -i / 10f;

            GameObject card = _spawner.SpawnCard(_deckManager.DrawCard());
            SetPositionToDeckLocation(card.transform, -1);
            SetInitialPosition(card.transform, targetPositions[i]);
            InitialCardRescale(card.transform);

            MinionBehaviour minionBehaviour = card.GetComponent<MinionBehaviour>();
            minionBehaviour.SetInitialPosition(targetPositions[i]);
        }

    }
    private void SetPositionToDeckLocation(Transform cardTransform, float zAxis)
    {
        cardTransform.SetParent(_playerHand.transform);
        cardTransform.position = GetDeckLocation(zAxis);
    }

    private static Vector3 GetDeckLocation(float zAxis = 0f)
    {
        return new Vector3(-7, 0, zAxis);
    }

    private void SetInitialPosition(Transform cardTransform, Vector3 initialPosition)
    {
        cardTransform.SetParent(_playerHand.transform);
        cardTransform.localPosition = initialPosition;
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

        float xAxisOffset = GetXAxisOffset(0, _separationFactor * (length - 1));

        for (int i = 0; i < length; i++)
        {
            float compute = (_separationFactor * i) - xAxisOffset;
            output.Add(new Vector3(compute, 0, -i / 100f));
        }

        return output;
    }

    private float GetXAxisOffset(float leftmostCardXPosition, float rightmostCardXPosition)
    {
        float canvasWidth = GetCanvasWidth(_cardPrefab);

        float leftmostBorderXPosition = leftmostCardXPosition - canvasWidth;
        float rightmostBorderXPosition = rightmostCardXPosition + canvasWidth;

        return Math.Abs((leftmostBorderXPosition + rightmostBorderXPosition) / 2f);
    }

    private float GetCanvasWidth(GameObject obj) => obj.GetComponentInChildren<RectTransform>().rect.width;
}