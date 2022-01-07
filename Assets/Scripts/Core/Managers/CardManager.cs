using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Managers
{
    public class CardManager
    {
        private readonly int INITIAL_DRAW_COUNT = 3;

        private readonly ISpawner _spawner;
        private readonly IBattleSystemUtils _utils;
        private readonly IDeckManager _deckManager;
        private readonly GameObject _playerHand;
        private readonly float _scaleFactor;
        private readonly float _separationFactor;

        private CardManagerState _state;

        public CardManager(ISpawner spawner,
                           IBattleSystemUtils utils,
                           IDeckManager deckManager,
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

        private void ChangeState(CardManagerState state) => _state = state;

        public void Draw()
        {
            if (CardManagerState.Idle != _state) return;

            InstantiateCard();

            List<GameObject> children = _utils.GetChildren(_playerHand);
            List<Vector3> updatedPositions = GenerateCardPositionsOnPlayerHand(children.Count);

            for (int i = 0; i < children.Count; i++)
            {
                ChangeState(CardManagerState.Drawing);
                IMinionBehaviour minionBehaviour = _utils.GetMinionBehaviour(children[i]);

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

                IMinionBehaviour minionBehaviour = _utils.GetMinionBehaviour(card);

                minionBehaviour.SetInitialPosition(targetPositions[i]);
                minionBehaviour.LerpToInitialPosition(() => ChangeState(CardManagerState.Idle));
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
                float compute = _separationFactor * i - xAxisOffset;
                output.Add(new Vector3(compute, 0, -i / 100f));
            }

            return output;
        }

        private float GetXAxisOffset(float leftmostCardXPosition, float rightmostCardXPosition) => Math.Abs((leftmostCardXPosition + rightmostCardXPosition) / 2f);
    }
}