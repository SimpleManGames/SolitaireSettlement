using UnityEngine;

namespace SolitaireSettlement
{
    public class StockpileCardImpl : ICardUniqueImpl
    {
        [field: SerializeField, Min(0)]
        public int AmountOfCardsStored { get; private set; }
        
        public void Initialize(Card cardObject)
        {
        }

        public void TurnProgress(Card card)
        {
        }
    }
}