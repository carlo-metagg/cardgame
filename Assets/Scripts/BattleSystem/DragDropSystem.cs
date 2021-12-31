using UnityEngine;

public class DragDropSystem
{
    private ISpawner _spawner;
    private readonly float _previewCardScaleFactor;
    private readonly float _previewCardYOffset;

    private GameObject _cardObject;
    private MinionBehaviour _cardBehaviour;
    private DragState _dragState;
    private GameObject _previewCard;

    public DragDropSystem(ISpawner spawner, float previewCardScaleFactor, float previewCardYOffset)
    {
        _spawner = spawner;
        _previewCardScaleFactor = previewCardScaleFactor;
        _previewCardYOffset = previewCardYOffset;

        ChangeState(DragState.Idle);
        _previewCard = null;
    }

    public GameObject CardObject { 
        get => _cardObject;
        set 
        {
            if(DragState.Idle == _dragState)
            {
                _cardObject = value;
                _cardBehaviour = _cardObject.GetComponent<MinionBehaviour>();
            }
        } 
    }

    public void DragListener()
    {
        if (_cardObject && DragState.Dragging == _dragState)
        {
            _cardBehaviour.Drag();
        }
    }

    public void GrabCard()
    {
        ExitHover();

        ChangeState(DragState.Dragging);
    }

    public void Hover()
    {
        if (DragState.Idle == _dragState)
        {
            if (_previewCard && IsCardBeingHoveredDifferent())
            {
                DestroyCardPreview();
            }

            if (!_previewCard)
            {
                SpawnAndPositionPreviewCard();
            }
        }
    }

    private bool IsCardBeingHoveredDifferent()
    {
        return _cardObject.gameObject.GetInstanceID() != _previewCard.GetInstanceID();
    }

    private void SpawnAndPositionPreviewCard()
    {
        _previewCard = _spawner.SpawnCard(_cardBehaviour.CardData);
        _previewCard.GetComponent<Collider2D>().enabled = false;

        _previewCard.transform.position = GenerateTargetPosition();

        _previewCard.transform.localScale *= _previewCardScaleFactor;
    }

    private Vector3 GenerateTargetPosition()
    {
        Vector3 cardPosition = _cardObject.transform.position;
        Vector3 previewCardTargetPosition = new Vector3(cardPosition.x, cardPosition.y + _previewCardYOffset, -1f);
        return previewCardTargetPosition;
    }

    private void DestroyCardPreview()
    {
        Object.Destroy(_previewCard);
        _previewCard = null;
    }

    public void ExitHover()
    {
        if (_previewCard)
        {
            DestroyCardPreview();
        }
    }

    public void ReleaseCard()
    {
        if (_cardObject)
        {
            ChangeState(DragState.Returning);

            _cardBehaviour.Release(() => ChangeState(DragState.Idle));
            _cardObject = null;
        }
    }

    private void ChangeState(DragState state)
    {
        _dragState = state;
    }
}

public enum DragState
{ 
    Idle,
    Dragging,
    Returning
}