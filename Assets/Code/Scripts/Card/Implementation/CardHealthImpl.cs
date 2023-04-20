using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardHealthImpl : ICardUniqueImpl
    {
        [field: SerializeField]
        public int MaxHealth { get; private set; }

        [field: ShowInInspector]
        public int CurrentHealth { get; private set; }

        public virtual void Initialize(Card cardObject)
        {
            CurrentHealth = MaxHealth;
        }

        public virtual void TurnProgress(Card card)
        {
            if (CheckHealth())
                return;

            CardManager.Instance.RequestToDeleteCardObject(card.gameObject);
        }

        public void ModifyHealth(int amount)
        {
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }

        private bool CheckHealth()
        {
            return CurrentHealth > 0;
        }
    }
}