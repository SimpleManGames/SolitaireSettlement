using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class AmountOfUsesConsume : IStackActionConsume
    {
        [field: SerializeField, MinValue(1)]
        private int AmountOfUses { get; set; } = 1;

        public void OnConsume(Card cardObject)
        {
            AmountOfUses--;
            if (AmountOfUses == 0)
            {
                Debug.Log("Used up all Consume");
            }
        }
    }
}