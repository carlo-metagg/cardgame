using Core;
using Core.Managers;
using UnityEngine;

public class BattleSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerHand;

    [Header("Card Manager parameters")]
    [Range(0, 1)]
    [SerializeField] private float scaleFactor = 0.75f;
    [SerializeField] private float separationFactor = 0.05f;

    private ISpawner spawner;
    private IBattleSystemUtils utils;
    private IDeckManager deckManager;
    private CardManager cardManager;
    private BattleSystem battleSystem;
    //private DragDropSystemBehaviour dragDrop;
    //private bool isCursorOnCard;

    void Awake()
    {
        spawner = GetComponent<ISpawner>();
        utils = new BattleSystemUtils();
        deckManager = new DeckManager("Minions");
        cardManager = new CardManager(spawner,
                                      utils,
                                      deckManager,
                                      playerHand,
                                      scaleFactor,
                                      separationFactor);

        battleSystem = new BattleSystem(cardManager);
        //dragDrop = GetComponent<DragDropSystemBehaviour>();
        //isCursorOnCard = false;

        battleSystem.PreparePlayArea();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cardManager.Draw();
        }

        #region Drag and Drop System version 1
        //dragDrop.DragListener();

        //RaycastHit2D hit = utils.GetNearestCollider();

        //if (hit.collider)
        //{
        //    GameObject nearestObject = hit.collider.gameObject;
        //    bool isCard = nearestObject.GetComponent<MinionBehaviour>();
        //    isCursorOnCard = true;

        //    if (isCard)
        //    {
        //        dragDrop.CardObject = nearestObject;

        //        if (Input.GetKeyDown(KeyCode.Mouse0))
        //        {
        //            dragDrop.GrabCard();
        //        }

        //        if (!Input.GetKeyDown(KeyCode.Mouse0))
        //        {
        //            dragDrop.Hover();
        //        }
        //    }
        //}
        //else if (isCursorOnCard)
        //{
        //    dragDrop.ExitHover();
        //    isCursorOnCard = false;
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    dragDrop.ReleaseCard();
        //}
        #endregion
    }
}