using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class StackActionManager : SerializedMonoBehaviour
    {
        [field: SerializeField]
        private HashSet<StackActionData> StackActions { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardStack> CurrentStacks { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<StackActionData> CurrentPossibleStackActions { get; set; }

        private void Awake()
        {
            CurrentStacks = new List<CardStack>();
            CurrentPossibleStackActions = new List<StackActionData>();
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
            foreach (var action in from stack in CurrentStacks // Gather all the CardData into list
                     select stack.Cards.Select(c => c.Data).ToList()
                     into stackCardData
                     from action in StackActions // Gather all the actions that match what the stack has
                     let containsAllCards = action.NeededCardsInStack.All(stackCardData.Contains)
                     where containsAllCards
                     select action)
            {
                CurrentPossibleStackActions.Add(action);
            }
        }
    }
}