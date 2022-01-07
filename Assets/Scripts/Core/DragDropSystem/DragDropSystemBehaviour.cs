using System.Collections;
using UnityEngine;

public class DragDropSystemBehaviour : MonoBehaviour
{
    [Header("Card Preview Parameters")]

    [SerializeField] 
    private ISpawner _spawner;

    [SerializeField]
    private float previewCardYOffset = 3f;

    [Range(0, 3)]
    [SerializeField]
    private float previewCardScaleFactor = 1.3f;

    [SerializeField]
    private float returnToHandDuration = 0.5f;

    private GameObject _cardObject;
    private MinionBehaviour _cardBehaviour;
    private DragState _dragState;
    private GameObject _previewCard;

    private void Awake()
    {
        _spawner = GetComponent<ISpawner>();

        ChangeState(DragState.Idle);
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
        _previewCard.transform.localScale *= previewCardScaleFactor;
    }

    private Vector3 GenerateTargetPosition()
    {
        Vector3 cardPosition = _cardObject.transform.position;
        Vector3 previewCardTargetPosition = new Vector3(cardPosition.x, cardPosition.y + previewCardYOffset, -1f);
        return previewCardTargetPosition;
    }

    private void DestroyCardPreview()
    {
        Destroy(_previewCard);
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

            StartCoroutine(ReturnToInitialPosition());
            _cardObject = null;
        }
    }
    private IEnumerator ReturnToInitialPosition()
    {
        Vector3 startingPos = _cardBehaviour.transform.position;
        Vector3 finalPos = _cardBehaviour.Minion.InitialPosition;
        float elapsedTime = 0;

        while (finalPos != _cardBehaviour.transform.position)
        {
            float lerpTravelPercentage = elapsedTime / returnToHandDuration;

            _cardBehaviour.transform.position = Vector3.Lerp(startingPos, finalPos, lerpTravelPercentage);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ChangeState(DragState.Idle);
    }

    private void ChangeState(DragState state)
    {
        _dragState = state;
    }
}
