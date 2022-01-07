using System;
using UnityEngine;

public interface IMinionBehaviour
{
    MinionCardData CardData { get; set; }
    Minion Minion { get; set; }

    void Drag();
    Vector3 GetOriginalLocalScale();
    void LerpToInitialPosition(Action action = null);
    void LerpToPosition(Vector3 initialPosition, Vector3 targetPosition, Action action = null);
    void SetCurrentPositionAsInitialPosition();
    void SetInitialPosition(Vector3 position);
}