using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ReplaceCardWithResults : IStackActionResult
    {
        [field: SerializeField, HideLabel, Title("Target Card"), HorizontalGroup]
        private CardData TargetCard { get; set; }

        [field: SerializeField, HideLabel, Title("Replacement Card"), HorizontalGroup]
        private CardData ReplacementCard { get; set; }

        public virtual void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var actualTargetCard = relatedCardStack.First(c => c.Info.Data == TargetCard);
            actualTargetCard.UpdateCardData(ReplacementCard);
        }
    }
}