using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinionCardDisplay : MonoBehaviour
{
    [SerializeField] private MinionCardData cardData;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private Image artwork;

    [SerializeField] private TextMeshProUGUI attack;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI manaCost;

    void Start()
    {
        cardName.text = cardData.CardName;
        description.text = cardData.Description;

        artwork.sprite = cardData.Artwork;

        attack.text = cardData.Attack.ToString();
        health.text = cardData.Health.ToString();
        manaCost.text = cardData.ManaCost.ToString();
    }

    public MinionCardData CardData { set => cardData = value;  }
}
