using System;
using System.Collections.Generic;
using System.Linq;
using Simplicity.Utility;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
                CompareCurrentStackToKnownStackActionsForPossibilities(currentStackData, currentStack);
            }
        }

        private void CompareCurrentStackToKnownStackActionsForPossibilities(List<CardData> currentStackData,
            CardStack currentStack)
        {
            foreach (var stackAction in StackActions)
            {
                if (!CheckForSimilarCards(stackAction, currentStackData))
                    continue;

                if (!CheckForFullMatching(stackAction, currentStackData))
                    continue;

                CreateAndAddRelevantStackActionInfo(stackAction, currentStack);
            }
        }

        private void CreateAndAddRelevantStackActionInfo(StackActionData stackAction, CardStack currentStack)
        {
            var stackInfo = new RelevantStackActionInfo()
            {
                stackActionData = stackAction,
                involvedCards = currentStack.Cards
            };

            currentStack.Cards.ForEach(c => c.InvolvedStackAction = stackAction);
            CurrentPossibleStackActions.Add(stackInfo);
        }

        public static bool CheckForSimilarCards(StackActionData stackAction, List<CardData> currentStackData)
        {
            if (stackAction == null)
            {
                Debug.LogError($"StackAction was null during StackActionManager:CheckForSimilarCards");
                throw new NullReferenceException();
            }

            if (stackAction.NeededCardsInStack == null)
            {
                Debug.LogError(
                    $"StackAction:NeededCardsInStack for {stackAction.name} was null during StackActionManager:CheckForSimilarCards");
                throw new NullReferenceException();
            }

            if (currentStackData == null)
            {
                Debug.LogError($"CurrentStackData was null during StackActionManager:CheckForSimilarCards");
                throw new NullReferenceException();
            }

            return stackAction.NeededCardsInStack.Select(n => n.Card).All(currentStackData.Contains);
        }

        public static bool CheckForFullMatching(StackActionData stackAction, List<CardData> currentStackData)
        {
            if (stackAction.NeededCardsInStack.Any(c => c.AnyAmount))
                return true;

            if (currentStackData.Count != stackAction.NeededCardsInStack.Sum(c => c.Count))
                return false;

            foreach (var need in stackAction.NeededCardsInStack)
            {
                var currentStackDataMatchCount = currentStackData.Count(c => c == need.Card);
                var stackActionMatchCount = stackAction.NeededCardsInStack.First(c => c.Card == need.Card).Count;

                if (currentStackDataMatchCount != stackActionMatchCount)
                    return false;
            }

            return true;
        }

        public void PreformPossibleStackActions()
        {
            foreach (var stackActionInfo in CurrentPossibleStackActions)
            {
                var involvedCards = stackActionInfo.involvedCards;
                if (!stackActionInfo.stackActionData.Results.IsNullOrEmpty())
                    foreach (var result in stackActionInfo.stackActionData.Results)
                        result.OnResult(involvedCards);

                foreach (var card in involvedCards.Where(card => card.Info.Data.CardUse != null))
                {
                    foreach (var cardUse in card.Info.Data.CardUse)
                        cardUse.OnCardUse(card);
                }
            }

            CurrentPossibleStackActions.Clear();
        }

        [Button]
        private void GatherStackActionAssetsIntoList()
        {
            StackActions = AssetParsingUtility.FindAssetsByType<StackActionData, HashSet<StackActionData>>();
        }
    }
}