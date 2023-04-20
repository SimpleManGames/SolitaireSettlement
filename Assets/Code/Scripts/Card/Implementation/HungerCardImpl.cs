using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class HungerCardImpl : ICardUniqueImpl
    {
        [field: SerializeField, Min(1)]
        public int HungerCap { get; private set; } = 5;

        [ShowInInspector]
        public int CurrentHunger { get; private set; } = 5;

        private Card _card;

        public void Initialize(Card cardObject)
        {
            _card = cardObject.GetComponent<Card>();
            CurrentHunger = HungerCap;
        }

        public void TurnProgress(Card card)
        {
            _card.Render.UpdateDynamicTextFields(card.Info.Data);
        }

        public void ModifyHungerBy(int amount)
        {
            CurrentHunger += amount;
            CurrentHunger = Mathf.Clamp(CurrentHunger, 0, HungerCap);
            _card.Render.UpdateDynamicTextFields(_card.Info.Data);
        }
    }
}