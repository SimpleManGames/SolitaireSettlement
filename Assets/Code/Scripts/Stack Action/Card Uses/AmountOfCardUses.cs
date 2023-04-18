using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AmountOfCardUses : IStackActionCardUse
    {
        [field: SerializeField, MinValue(1), HideLabel, HorizontalGroup, InlineProperty, Title("Amount of Uses")]
        private int AmountOfUses { get; set; } = 1;

        [field: SerializeField, HideLabel, HorizontalGroup, InlineProperty, Title("Result")]
        private IStackActionCardUse ResultAfterUses { get; set; }

        public void Initialize()
        {
            
        }

        public void OnCardUse(Card cardObject)
        {
            AmountOfUses--;
            if (AmountOfUses == 0)
                ResultAfterUses.OnCardUse(cardObject);
        }
    }
}