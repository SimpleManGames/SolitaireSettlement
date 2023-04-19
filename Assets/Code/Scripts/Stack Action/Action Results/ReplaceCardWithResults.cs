using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ReplaceCardWithResults : IStackActionResult
    {
        [field: SerializeField, HideLabel, Title("Target Card"), HorizontalGroup]
        public CardData TargetCard { get; private set; }

        [field: SerializeField, HideLabel, Title("Replacement Card"), HorizontalGroup]
        public CardData ReplacementCard { get; private set; }

        public virtual void OnResult(IEnumerable<Card> relatedCardStack)
        {
            var actualTargetCard = relatedCardStack.First(c => c.InternalDataReference == TargetCard);
            CardManager.Instance.RequestToReplaceCardData(actualTargetCard, ReplacementCard);
        }
    }
}