using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ChanceToAddCardResult : ChanceAddCardResult
    {
        [field: SerializeField]
        public CardChance CardChances { get; private set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            ProcessCardChances(CardChances, relatedCardStack.ElementAt(0).transform.position);
        }

        public override List<CardData> AddedCardData()
        {
            return new List<CardData>()
            {
                CardChances.ProducedCard
            };
        }
    }
}