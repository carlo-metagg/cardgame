using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionBehaviour : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private MinionCardData cardData;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Image artwork;

    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI manaCost;

    [Header("Lerping parameters")]
    [SerializeField] private float dragLerpMultiplier = 20;
    [SerializeField] private float returnToHandDuration = 0.5f;

    private Minion minion;
    private CardDisplay cardDisplay;

    private void Awake()
    {
        minion = new Minion(transform, dragLerpMultiplier, returnToHandDuration);
    }

    void Start()
    {
        cardDisplay = new CardDisplay(cardData,
                                      cardName,
                                      description,
                                      artwork,
                                      attack,
                                      health,
                                      manaCost,
                                      transform.localScale);

        cardDisplay.InitializeCard();
    }

    public MinionCardData CardData { get => cardData;  set => cardData = value; }

    public void SetCurrentPositionAsInitialPosition() => minion.SetCurrentPositionAsInitialPosition();

    public void Drag() => minion.DragToPosition(GetTargetPosition());

    private Vector3 GetTargetPosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = new Vector3(mousePosition.x, mousePosition.y, -1);

        return targetPosition;
    }

    public void Release(Action actionAfterCoroutine)
    {
        StartCoroutine(minion.ReturnToInitialPosition(actionAfterCoroutine));
    }

    public Vector3 GetOriginalLocalScale() => cardDisplay.OriginalLocalScale;
}