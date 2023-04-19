using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ReplaceCardWithOnUse : IStackActionCardUse
    {
        [field: SerializeField, HideLabel, Title("Replacement Card"), HorizontalGroup]
        private CardData ReplacementCard { get; set; }

        public void Initialize()
        {
            
        }

        public void OnCardUse(Card cardObject)
        {
            var actualTargetCard = cardObject.GetComponent<Card>();
            CardManager.Instance.RequestToReplaceCardData(actualTargetCard, ReplacementCard);
        }
    }
}