using UnityEngine;

public class DragDropSystem
{
    private MinionBehaviour _cardBehaviour;
    private bool isDragging;

    public void GrabCard(GameObject selectedGameObject)
    {
        MinionBehaviour minionBehaviour = selectedGameObject.GetComponent<MinionBehaviour>();

        if (minionBehaviour)
        {
            string cardName = minionBehaviour.CardData.CardName;

            _cardBehaviour = minionBehaviour;

            Debug.Log(cardName);
            isDragging = true;
        }
    }

    public void DragListener()
    {
        if (_cardBehaviour && isDragging)
        {
            _cardBehaviour.Drag();
        }
    }

    public void ReleaseCard()
    {
        if (_cardBehaviour)
        {
            isDragging = false;
            _cardBehaviour.Release();

            _cardBehaviour = null;
        }
    }
}
