using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AddCardsToDeckResult : AddCardResult
    {
        [field: SerializeField]
        public CardData[] ProducedCard { get; private set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var position = relatedCardStack.ElementAt(0).transform.position;
            for (var i = 0; i < ProducedCard.Length; i++)
            {
                var card = ProducedCard[i];
                CreateCardToDeck(card, position, i * DELAY_BETWEEN_CARD_SPAWNS);
            }
        }

        public override List<CardData> AddedCardData()
        {
            return ProducedCard.ToList();
        }
    }
}