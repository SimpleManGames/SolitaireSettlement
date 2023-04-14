using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AddCardToDeckResult : AddCardResult
    {
        [field: SerializeField, HideLabel, Title("Card")]
        public CardData ProducedCard { get; private set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            CreateCardToDeck(ProducedCard, relatedCardStack.ElementAt(0).transform.position);
        }

        public override List<CardData> AddedCardData()
        {
            return new List<CardData>()
            {
                ProducedCard
            };
        }
    }
}