using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerHand;

    [Header("Card Manager parameters")]
    [Range(0, 1)]
    [SerializeField] private float scaleFactor = 0.75f;
    [SerializeField] private float separationFactor = 0.05f;

    private List<MinionCardData> initialCardsDrawn;
    private ISpawner spawner;
    private BattleSystemUtils utils;
    private DeckManager deckManager;
    private CardManager cardManager;
    private BattleSystem battleSystem;
    //private DragDropSystemBehaviour dragDrop;
    //private bool isCursorOnCard;

    void Awake()
    {
        initialCardsDrawn = new List<MinionCardData>(Resources.LoadAll<MinionCardData>("Minions"));

        spawner = GetComponent<ISpawner>();
        utils = new BattleSystemUtils();
        deckManager = new DeckManager("Minions");
        cardManager = new CardManager(spawner,
                                      utils,
                                      deckManager,
                                      playerHand,
                                      cardPrefab,
                                      scaleFactor,
                                      separationFactor);

        battleSystem = new BattleSystem(spawner, cardManager);
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

public class DeckManager
{
    private readonly int DECK_SIZE = 20;
    List<MinionCardData> allCardData;
    Stack<MinionCardData> _deck;

    public DeckManager(string path)
    {
        allCardData = new List<MinionCardData>(Resources.LoadAll<MinionCardData>(path));
        _deck = GenerateRandomDeck();
    }

    private Stack<MinionCardData> GenerateRandomDeck()
    {
        Stack<MinionCardData> output = new Stack<MinionCardData>();

        for (int i = 0; i < DECK_SIZE; i++)
        {
            output.Push(allCardData[UnityEngine.Random.Range(0, allCardData.Count - 1)]);
        }

        return output;
    }

    public MinionCardData DrawCard()
    {
        return _deck.Pop();
    }
}
