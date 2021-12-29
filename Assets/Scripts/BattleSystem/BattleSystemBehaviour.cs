using UnityEngine;

public class BattleSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject playerHand;

    private ISpawner spawner;
    private BattleSystemUtils utils;
    private BattleSystem battleSystem;
    private DragDropSystem dragDrop;

    void Awake()
    {
        spawner = GetComponent<ISpawner>();
        utils = new BattleSystemUtils();
        battleSystem = new BattleSystem(playerHand, spawner, utils);
        dragDrop = new DragDropSystem();

        battleSystem.PreparePlayArea();
    }

    private void Update()
    {
        dragDrop.DragListener();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = utils.GetNearestCollider();

            if (hit.collider)
            {
                GameObject selectedGameObject = hit.collider.gameObject;
                dragDrop.GrabCard(selectedGameObject);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragDrop.ReleaseCard();
        }
    }
}
