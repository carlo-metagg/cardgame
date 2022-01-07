using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion
{
    private readonly ISpawner _spawner;
    private readonly Transform _transform;
    private readonly float _dragLerpMultiplier;
    private readonly float _returnToHandDuration;
    private readonly float _previewCardScaleFactor;
    private readonly float _previewCardYOffset;
    private GameObject _previewCard;

    private Vector3 _initialPosition;

    public Minion(ISpawner spawner,
                  Transform transform,
                  float dragLerpMultiplier,
                  float returnToHandDuration,
                  float previewCardScaleFactor,
                  float previewCardYOffset)
    {
        _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
        _transform = transform ?? throw new ArgumentNullException(nameof(transform));
        _dragLerpMultiplier = dragLerpMultiplier;
        _returnToHandDuration = returnToHandDuration;
        _previewCardScaleFactor = previewCardScaleFactor;
        _previewCardYOffset = previewCardYOffset;
    }

    public void DragToPosition(Vector3 targetPosition) => _transform.position = Vector3.Lerp(_transform.position, targetPosition, Time.deltaTime * _dragLerpMultiplier);

    public IEnumerator LerpToInitialPosition(List<Action> actions)
    {
        Vector3 startingPos = _transform.localPosition;
        Vector3 finalPos = _initialPosition;
        float elapsedTime = 0;

        while (finalPos != _transform.localPosition)
        {
            float lerpTravelPercentage = elapsedTime / _returnToHandDuration;

            _transform.localPosition = Vector3.Lerp(startingPos, finalPos, lerpTravelPercentage);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Action action in actions)
        {
            action();
        }
    }

    public IEnumerator LerpToPosition(Vector3 intialPosition, Vector3 targetPosition, float duration, System.Collections.Generic.List<Action> actions)
    {
        float elapsedTime = 0;

        while (targetPosition != _transform.localPosition)
        {
            float lerpTravelPercentage = elapsedTime / duration;

            _transform.localPosition = Vector3.Lerp(intialPosition, targetPosition, lerpTravelPercentage);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Action action in actions)
        {
            action();
        }
    }

    public void Hover(MinionCardData cardData)
    {

        _previewCard = _spawner.SpawnCard(cardData);
        _previewCard.GetComponent<Collider2D>().enabled = false;

        _previewCard.transform.position = GenerateTargetPosition();
        _previewCard.transform.localScale = new Vector3(1f, 1f, 1f);
        _previewCard.transform.localScale *= _previewCardScaleFactor;
    }

    public void DestroyPreviewCard()
    {
        if (_previewCard)
        {
            UnityEngine.Object.Destroy(_previewCard);
        }
    }

    private Vector3 GenerateTargetPosition()
    {
        Vector3 cardPosition = _transform.position;
        Vector3 previewCardTargetPosition = new Vector3(cardPosition.x, cardPosition.y + _previewCardYOffset, -1f);
        return previewCardTargetPosition;
    }

    public Vector3 InitialPosition { get => _initialPosition; set => _initialPosition = value; }
    public void SetInitialPosition(Vector3 position) => _initialPosition = position;
    public void SetCurrentPositionAsInitialPosition() => _initialPosition = _transform.position;
}
