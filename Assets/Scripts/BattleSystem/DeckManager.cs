using System.Collections.Generic;
using UnityEngine;

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
            output.Push(allCardData[UnityEngine.Random.Range(0, allCardData.Count)]);
        }

        return output;
    }

    public MinionCardData DrawCard()
    {
        return _deck.Pop();
    }
}
