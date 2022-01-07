using Core.Managers;

namespace Core
{
    public class BattleSystem
    {
        private readonly CardManager _cardManager;

        public BattleSystem(CardManager cardManager)
        {
            _cardManager = cardManager;
        }

        public void PreparePlayArea()
        {
            _cardManager.InstantiateCards();
        }

        public void Draw()
        {
            _cardManager.Draw();
        }
    }
}