using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMinionBehaviourScript : MonoBehaviour
{
    [SerializeField] private MinionCardData cardData;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Image artwork;

    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI manaCost;

    [SerializeField] private float lerpMultiplier = 20;
    [SerializeField] private float returnToHandDuration = 0.5f;

    private Vector3 initialPosition;
    private Vector3 delta;

    void Awake()
    {
        InitializeCard();

        initialPosition = transform.position;
    }

    private void InitializeCard()
    {
        cardName.text = cardData.CardName;
        description.text = cardData.Description;

        artwork.sprite = cardData.Artwork;

        attack.text = cardData.Attack.ToString();
        health.text = cardData.Health.ToString();
        manaCost.text = cardData.ManaCost.ToString();
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        print("mouse down!");
    }

    private void OnMouseDrag()
    {
        print("dragging!");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = Vector2.Lerp(transform.position, mousePosition, Time.deltaTime * lerpMultiplier);
    }

    private void OnMouseUp()
    {
        //transform.position = initialPosition;
        StartCoroutine(ReturnToInitial());
    }

    private IEnumerator ReturnToInitial()
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = initialPosition;
        float elapsedTime = 0;

        while (finalPos != transform.position)
        {
            float lerpTravelPercentage = elapsedTime / returnToHandDuration;
            transform.position = Vector3.Lerp(startingPos, finalPos, lerpTravelPercentage);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
