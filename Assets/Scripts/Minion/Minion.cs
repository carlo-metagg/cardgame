using System;
using System.Collections;
using UnityEngine;

public class Minion
{
    private readonly Transform _transform;
    private readonly float _dragLerpMultiplier;
    private readonly float _returnToHandDuration;

    private Vector3 _initialPosition;

    public Minion(Transform transform, float dragLerpMultiplier, float returnToHandDuration)
    {
        _transform = transform;
        _dragLerpMultiplier = dragLerpMultiplier;
        _returnToHandDuration = returnToHandDuration;
    }

    public void DragToPosition(Vector3 targetPosition)
    {
        _transform.position = Vector3.Lerp(_transform.position, targetPosition, Time.deltaTime * _dragLerpMultiplier);
    }

    public Vector3 InitialPosition { get => _initialPosition; set => _initialPosition = value; }
    public void SetInitialPosition(Vector3 position) => _initialPosition = position;
    public void SetCurrentPositionAsInitialPosition() => _initialPosition = _transform.position;
}
