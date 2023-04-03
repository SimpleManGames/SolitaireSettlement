using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace SolitaireSettlement
{
    public class StackActionManager : SerializedMonoBehaviour
    {
        protected struct RelevantStackActionInfo
        {
            public StackActionData stackActionData;
            public List<Card> involvedCards;
        }

        [field: SerializeField]
        private HashSet<StackActionData> StackActions { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardStack> CurrentStacks { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<RelevantStackActionInfo> CurrentPossibleStackActions { get; set; }

        private void Awake()
        {
            CurrentStacks = new List<CardStack>();
            CurrentPossibleStackActions = new List<RelevantStackActionInfo>();
        }

        private void Update()
        {
            GatherCurrentStacks();
            CheckForPossibleStackActions();
        }

        private void GatherCurrentStacks()
        {
            CurrentPossibleStackActions.Clear();
            var cardObjects = GameObject.FindGameObjectsWithTag("Card");
            var cardComponents = cardObjects.Select(c => c.GetComponent<Card>());
            var uniqueStacks = new List<CardStack>();
            foreach (var card in cardComponents)
            {
                if (card.Stack?.HasCards != true)
                    continue;

                if (uniqueStacks.Contains(card.Stack))
                    continue;

                uniqueStacks.Add(card.Stack);
            }

            CurrentStacks = uniqueStacks;
        }

        private void CheckForPossibleStackActions()
        {
            foreach (var currentStack in CurrentStacks)
            {
                var currentStackData = currentStack.Cards.Select(c => c.Data);

                // Select the stackAction that contains all the cards in currentStack
                foreach (var stackAction in from stackAction in StackActions
                         let containsAllCards = stackAction.NeededCardsInStack.All(stackActionCard =>
                             currentStackData.Contains(stackActionCard))
                         where containsAllCards
                         select stackAction)
                {
                    CurrentPossibleStackActions.Add(new RelevantStackActionInfo()
                    {
                        stackActionData = stackAction,
                        involvedCards = currentStack.Cards
                    });
                }
            }
        }

        public void PreformPossibleStackActions()
        {
            foreach (var stackActionInfo in CurrentPossibleStackActions)
                stackActionInfo.stackActionData.Result.Result(stackActionInfo.involvedCards);
        }
    }
}