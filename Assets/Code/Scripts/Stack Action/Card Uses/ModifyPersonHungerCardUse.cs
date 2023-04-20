using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace SolitaireSettlement
{
    public class ModifyPersonHungerCardUse : IStackActionCardUse
    {
        [field: SerializeField]
        private int ModifyAmount { get; set; } = 1;

        [field: SerializeField]
        private List<StackActionData> IgnoreStarvingWhileDoingStackActions { get; set; }

        public void Initialize()
        {
        }

        public void OnCardUse(Card cardObject)
        {
            var card = cardObject.GetComponent<Card>();

            if (!card.Stack.HasCards)
                return;

            if (!IgnoreStarvingWhileDoingStackActions.IsNullOrEmpty())
                if (IgnoreStarvingWhileDoingStackActions.Contains(cardObject.InvolvedStackAction))
                    return;

            var cardImplWithinStack = card.Stack.Cards.SelectMany(c => c.Info.Data.OnTurnUpdate);
            var hungerCardImplWithinStack =
                cardImplWithinStack.Where(c => c is HungerCardImpl).Cast<HungerCardImpl>();

            foreach (var hungerImpl in hungerCardImplWithinStack)
                hungerImpl.ModifyHungerBy(ModifyAmount);
        }
    }
}