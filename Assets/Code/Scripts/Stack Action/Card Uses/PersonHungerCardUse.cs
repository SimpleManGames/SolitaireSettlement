using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class PersonHungerCardUse : IStackActionCardUse
    {
        [field: SerializeField, Min(1)]
        public int HungerCap { get; private set; } = 5;

        [ShowInInspector]
        public int CurrentHunger { get; private set; } = 5;

        public void Initialize()
        {
            CurrentHunger = HungerCap;
        }

        public void OnCardUse(Card cardObject)
        {
            var card = cardObject.GetComponent<Card>();

            if (card.Stack.HasCards)
                if (card.Stack.Cards.Any(c => c.Info.Data.CardType == CardData.ECardType.Food))
                    return;

            CurrentHunger--;
            CurrentHunger = Mathf.Clamp(CurrentHunger, 0, HungerCap);
            card.Render.UpdateDynamicTextFields(card.Info.Data);
        }

        public void ModifyHungerBy(int amount)
        {
            CurrentHunger += amount;
            CurrentHunger = Mathf.Clamp(CurrentHunger, 0, HungerCap);
        }
    }
}