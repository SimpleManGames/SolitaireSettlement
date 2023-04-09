using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AddCardToDeckResult : IStackActionResult
    {
        [field: SerializeField, Required]
        private CardData ProducedCard { get; set; }

        public void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var stack = relatedCardStack.Aggregate("Stack ", (s, card) => s += $": {card.Data.Name}");
            Debug.Log($"Produced card:{ProducedCard.Name} with {stack}");
            DeckManager.Instance.AddCardToDeck(ProducedCard);
        }
    }
}