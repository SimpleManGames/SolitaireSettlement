using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AddCardToResult : AddCardResult
    {
        [field: SerializeField, HideLabel, Title("Card")]
        public CardData ProducedCard { get; private set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var firstCard = relatedCardStack.ElementAt(0);
            CreateCardTo(ProducedCard, Location, firstCard.transform.position);
        }

        public override List<CardData> AddedCardData()
        {
            return new List<CardData>
            {
                ProducedCard
            };
        }
    }
}