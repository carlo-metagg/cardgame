using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay
{
    private MinionCardData _cardData;

    private TextMeshProUGUI _cardName;
    private TextMeshProUGUI _description;

    private Image _artwork;

    private TextMeshProUGUI _attack;
    private TextMeshProUGUI _health;
    private TextMeshProUGUI _manaCost;
    private Vector3 _originalLocalScale;

    public CardDisplay(MinionCardData cardData,
                       TextMeshProUGUI cardName,
                       TextMeshProUGUI description,
                       Image artwork,
                       TextMeshProUGUI attack,
                       TextMeshProUGUI health,
                       TextMeshProUGUI manaCost,
                       Vector3 localScale)
    {
        _cardData = cardData;
        _cardName = cardName;
        _description = description;
        _artwork = artwork;
        _attack = attack;
        _health = health;
        _manaCost = manaCost;
        _originalLocalScale = localScale;
    }

    public Vector3 OriginalLocalScale { get => _originalLocalScale; }

    public void InitializeCard()
    {
        _cardName.text = _cardData.CardName;
        _description.text = _cardData.Description;

        _artwork.sprite = _cardData.Artwork;

        _attack.text = _cardData.Attack.ToString();
        _health.text = _cardData.Health.ToString();
        _manaCost.text = _cardData.ManaCost.ToString();
    }
}