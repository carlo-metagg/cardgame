using UnityEngine;

public class BattleSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject playerHand;

    [Header("Card Preview Parameters")]
    [SerializeField] private float previewCardYOffset = 3f;
    [Range(0, 3)]
    [SerializeField] private float previewCardScaleFactor = 1.3f;

    private ISpawner spawner;
    private BattleSystemUtils utils;
    private BattleSystem battleSystem;
    private DragDropSystem dragDrop;
    private bool isCursorOnCard;

    void Awake()
    {
        spawner = GetComponent<ISpawner>();
        utils = new BattleSystemUtils();
        battleSystem = new BattleSystem(playerHand, spawner, utils);
        dragDrop = new DragDropSystem(spawner, previewCardScaleFactor, previewCardYOffset);
        isCursorOnCard = false;

        battleSystem.PreparePlayArea();
    }

    private void Update()
    {
        dragDrop.DragListener();

        RaycastHit2D hit = utils.GetNearestCollider();

        if (hit.collider)
        {
            GameObject nearestObject = hit.collider.gameObject;
            bool isCard = nearestObject.GetComponent<MinionBehaviour>();
            isCursorOnCard = true;

            if (isCard)
            {
                dragDrop.CardObject = nearestObject;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    dragDrop.GrabCard();
                }
                
                if (!Input.GetKeyDown(KeyCode.Mouse0))
                {
                    dragDrop.Hover();
                }
            }
        }
        else if (isCursorOnCard)
        {
            dragDrop.ExitHover();
            isCursorOnCard = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragDrop.ReleaseCard();
        }
    }
}
