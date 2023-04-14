using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ReplaceCardWithOnUse : IStackActionCardUse
    {
        [field: SerializeField, HideLabel, Title("Replacement Card"), HorizontalGroup]
        private CardData ReplacementCard { get; set; }
        
        public void OnCardUse(Card cardObject)
        {
            cardObject.UpdateCardData(ReplacementCard);
        }
    }
}