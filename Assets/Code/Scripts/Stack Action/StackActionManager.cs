using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class StackActionManager : SerializedMonoBehaviour
    {
        private struct RelevantStackActionInfo
        {
            public StackActionData stackActionData;
            public List<Card> involvedCards;
        }

        [field: SerializeField]
        private HashSet<StackActionData> StackActions { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardStack> CurrentStacks { get; set; }

        [field: ShowInInspector, ReadOnly]
        private HashSet<RelevantStackActionInfo> CurrentPossibleStackActions { get; set; }

        private void Awake()
        {
            CurrentStacks = new List<CardStack>();
            CurrentPossibleStackActions = new HashSet<RelevantStackActionInfo>();
        }

        public void GatherCurrentStacks()
        {
            CurrentStacks.Clear();
            var cardObjects = FindObjectsOfType<Card>();
            var cardComponents = cardObjects.Select(c => c.GetComponent<Card>());
            var uniqueStacks = new List<CardStack>();
            foreach (var card in cardComponents)
            {
                if (card == null)
                    continue;

                if (card.Stack?.HasCards != true)
                    continue;

                if (uniqueStacks.Contains(card.Stack))
                    continue;

                uniqueStacks.Add(card.Stack);
            }

            CurrentStacks = uniqueStacks;
        }

        public void CheckForPossibleStackActions()
        {
            CurrentPossibleStackActions.Clear();
            foreach (var currentStack in CurrentStacks)
            {
                // We use the InternalDataReference here since the Cloned version has different values and the compare wouldn't work
                var currentStackData = currentStack.Cards.Select(c => c.InternalDataReference).ToList();

                foreach (var stackAction in StackActions)
                {
                    var containsAllCards = stackAction.NeededCardsInStack.All(stackActionCard =>
                        currentStackData.Contains(stackActionCard));

                    if (!containsAllCards)
                        continue;

                    var fullMatched = true;
                    foreach (var card in stackAction.NeededCardsInStack)
                    {
                        var currentStackDataMatchCount = currentStackData.Count(c => c == card);
                        var stackActionMatchCount = stackAction.NeededCardsInStack.Count(c => c == card);

                        if (currentStackDataMatchCount != stackActionMatchCount)
                            fullMatched = false;
                    }

                    if (!fullMatched)
                        continue;

                    var stackInfo = new RelevantStackActionInfo()
                    {
                        stackActionData = stackAction,
                        involvedCards = currentStack.Cards
                    };

                    CurrentPossibleStackActions.Add(stackInfo);
                }

                // Select the stackAction that contains all the cards in currentStack
                // foreach (var stackAction in from stackAction in StackActions.Where(s =>
                //              s.NeededCardsInStack.Count == currentStackData.Count())
                //          let containsAllCards = stackAction.NeededCardsInStack.All(stackActionCard =>
                //              currentStackData.Contains(stackActionCard))
                //          where containsAllCards
                //          select stackAction)
                // {
                // }
            }
        }

        public void PreformPossibleStackActions()
        {
            foreach (var stackActionInfo in CurrentPossibleStackActions)
            {
                stackActionInfo.stackActionData.Result.OnResult(stackActionInfo.involvedCards);
                foreach (var card in stackActionInfo.involvedCards.Where(card => card.Info.Data.CardUse != null))
                    card.Info.Data.CardUse.OnCardUse(card);
            }

            CurrentPossibleStackActions.Clear();
        }
    }
}