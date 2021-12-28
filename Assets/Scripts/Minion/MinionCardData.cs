using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "Card Data/Minion")]
public class MinionCardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [Multiline(3)]
    [SerializeField] private string description;

    [SerializeField] private Sprite artwork;

    [SerializeField] int manaCost;
    [SerializeField] int attack;
    [SerializeField] int health;

    public string CardName => cardName;

    public string Description => description;

    public Sprite Artwork => artwork;

    public int ManaCost => manaCost;

    public int Attack => attack;

    public int Health => health;
}
