using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AmountOfCardUses : IStackActionCardUse
    {
        [field: SerializeField, MinValue(1)]
        private int AmountOfUses { get; set; } = 1;

        public void OnCardUse(Card cardObject)
        {
            AmountOfUses--;
            if (AmountOfUses == 0)
                CardManager.Instance.RequestToDeleteCardObject(cardObject.gameObject);
        }
    }
}