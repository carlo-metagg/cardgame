using System;
using System.Collections;
using UnityEngine;

public class Minion
{
    private readonly Transform _transform;
    private readonly float _dragLerpMultiplier;
    private readonly float _returnToHandDuration;

    private Vector3 initialPosition;

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

    public IEnumerator ReturnToInitialPosition(Action endOfCoroutineAction)
    {
        Vector3 startingPos = _transform.position;
        Vector3 finalPos = initialPosition;
        float elapsedTime = 0;

        while (finalPos != _transform.position)
        {
            float lerpTravelPercentage = elapsedTime / _returnToHandDuration;

            _transform.position = Vector3.Lerp(startingPos, finalPos, lerpTravelPercentage);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        endOfCoroutineAction();
    }

    public void SetCurrentPositionAsInitialPosition() => initialPosition = _transform.position;
}
