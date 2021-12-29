using TMPro;
using UnityEngine.UI;

public class CardDisplay
{
    private MinionCardData cardData;

    private TextMeshProUGUI cardName;
    private TextMeshProUGUI description;

    private Image artwork;

    private TextMeshProUGUI attack;
    private TextMeshProUGUI health;
    private TextMeshProUGUI manaCost;

    public CardDisplay(MinionCardData cardData,
                       TextMeshProUGUI cardName,
                       TextMeshProUGUI description,
                       Image artwork,
                       TextMeshProUGUI attack,
                       TextMeshProUGUI health,
                       TextMeshProUGUI manaCost)
    {
        this.cardData = cardData;
        this.cardName = cardName;
        this.description = description;
        this.artwork = artwork;
        this.attack = attack;
        this.health = health;
        this.manaCost = manaCost;
    }

    public void InitializeCard()
    {
        cardName.text = cardData.CardName;
        description.text = cardData.Description;

        artwork.sprite = cardData.Artwork;

        attack.text = cardData.Attack.ToString();
        health.text = cardData.Health.ToString();
        manaCost.text = cardData.ManaCost.ToString();
    }
}