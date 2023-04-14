using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AddCardToDeckResult : AddCardResult
    {
        [field: SerializeField]
        private CardData ProducedCard { get; set; }

        public override void OnResult(IEnumerable<Card> relatedCardStack)
        {
            CreateCardToDeck(ProducedCard, relatedCardStack.ElementAt(0).transform.position);
        }
    }
}