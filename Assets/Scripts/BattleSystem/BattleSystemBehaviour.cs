using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystemBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject playerHand;


    private GameObject selectedCard;
    private MinionBehaviour cardBehaviour;
    private bool isDragging;

    void Awake()
    {
        isDragging = false;
        selectedCard = null;
        InstantiateCards();
        CenterCards();
    }

    private void CenterCards()
    {
        List<GameObject> children = BattleSystemUtils.GetChildren(playerHand);
        float xAxisOffset = -GetXAxisOffset(children[0], children[children.Count - 1]);

        for (int i = 0; i < children.Count; i++)
        {
            GameObject child = children[i];

            child.transform.localPosition += new Vector3(xAxisOffset, 0f, -i/10f);
            child.GetComponent<MinionBehaviour>().SetCurrentPositionAsInitialPosition();
        }
    }

    private float GetXAxisOffset(GameObject rightmostCard, GameObject leftmostCard)
    {
        float canvasWidth = GetCanvasWidth(cardPrefab);

        float leftmostBorderXPosition = leftmostCard.transform.localPosition.x - canvasWidth;
        float rightmostBorderXPosition = rightmostCard.transform.localPosition.x + canvasWidth;

        return Math.Abs((leftmostBorderXPosition + rightmostBorderXPosition) / 2f);
    }

    private float GetCanvasWidth(GameObject obj)
    {
        return obj.GetComponentInChildren<RectTransform>().rect.width;
    }

    private void InstantiateCards()
    {
        List<MinionCardData> entries = new List<MinionCardData>(Resources.LoadAll<MinionCardData>("Minions"));

        List<Vector3> positionOffsets = GeneratePositionOffset(entries.Count);

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject card = SpawnCard(entries[i]);
            SetInitialPosition(card.transform, positionOffsets[i]);
        }
    }

    private List<Vector3> GeneratePositionOffset(int length)
    {
        List<Vector3> output = new List<Vector3>();
        float separation = 0.05f;

        for (int i = 0; i < length; i++)
        {
            float compute = separation * i;
            output.Add(new Vector3(compute, 0, 0));
        }

        return output;
    }

    private void SetInitialPosition(Transform cardTransform, Vector3 positionOffset)
    {
        cardTransform.SetParent(playerHand.transform);
        cardTransform.localPosition = positionOffset;
    }
    private GameObject SpawnCard(MinionCardData entry)
    {
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<MinionBehaviour>().CardData = entry;

        return card;
    }

    private void Update()
    {
        if (isDragging)
        {
            cardBehaviour.Drag();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider)
            {
                GameObject selectedGameObject = hit.collider.gameObject;
                string cardName = selectedGameObject.GetComponent<MinionBehaviour>().CardData.CardName;

                Debug.Log(cardName);
                selectedCard = selectedGameObject;
                cardBehaviour = selectedCard.GetComponent<MinionBehaviour>();

                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            cardBehaviour.Release();

            selectedCard = null;
            cardBehaviour = null;
        }
    }
}
