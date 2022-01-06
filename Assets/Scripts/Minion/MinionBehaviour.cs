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
    [SerializeField] private float returnToHandDuration = 0.1f;

    [Header("Card Preview parameters")]
    [SerializeField] private float previewCardYOffset = 3f;
    [SerializeField] private float previewCardScaleFactor = 1.3f;

    private Minion _minion;
    private ISpawner _spawner;
    private CardDisplay _cardDisplay;
    private GameObject _previewCard;
    private DragState _dragState;

    private void Awake()
    {
        _spawner = GetComponent<ISpawner>();
        _minion = new Minion(_spawner,
                             transform,
                             dragLerpMultiplier,
                             returnToHandDuration,
                             previewCardScaleFactor,
                             previewCardYOffset);

        ChangeState(DragState.Idle);
    }

    void Start()
    {
        _cardDisplay = new CardDisplay(cardData,
                                      cardName,
                                      description,
                                      artwork,
                                      attack,
                                      health,
                                      manaCost,
                                      transform.localScale);
    }

    private void OnMouseEnter()
    {
        //todo: issue where other cards still spawn preview cards on hover while a card is being dragged.
        //might be due to the fact this function is only called once
        if (DragState.Idle != _dragState || Input.GetMouseButtonDown(0)) return;

        _minion.Hover(cardData);
    }

    private void OnMouseExit()
    {
        _minion.DestroyPreviewCard();
    }

    private void OnMouseDown()
    {
        ChangeState(DragState.Dragging);
    }

    private void OnMouseDrag()
    {
        _minion.DestroyPreviewCard();
        _minion.DragToPosition(GetTargetPosition());
    }

    private void OnMouseUp()
    {
        ChangeState(DragState.Returning);
        ReturnToIntiialPosition();
    }

    public void ReturnToIntiialPosition()
    {
        StartCoroutine(_minion.ReturnToInitialPosition(() => ChangeState(DragState.Idle)));
    }

    public MinionCardData CardData { get => cardData;  set => cardData = value; }

    public Minion Minion { get => _minion; set => _minion = value; }

    public void SetCurrentPositionAsInitialPosition() => _minion.SetCurrentPositionAsInitialPosition();
    
    public void SetInitialPosition(Vector3 position) => _minion.SetInitialPosition(position);

    public void Drag() => _minion.DragToPosition(GetTargetPosition());

    private Vector3 GetTargetPosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPosition = new Vector3(mousePosition.x, mousePosition.y, -1);

        return targetPosition;
    }

    public Vector3 GetOriginalLocalScale() => _cardDisplay.OriginalLocalScale;

    private void ChangeState(DragState dragState)
    {
        Debug.Log($"ChangeState() {_dragState} -> {dragState}");
        _dragState = dragState;
    }
}