using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class Card : MonoBehaviour
    {
        [field: SerializeField]
        public CardData Data { get; set; }

        [field: ShowInInspector]
        [field: ReadOnly]
        public CardStack Stack { get; set; }

        public bool IsDragging { get; set; }

        [ShowInInspector]
        public bool IsInStack => Stack != null;

        public bool IsOnBottom => Stack.BottomCard() == this;
        public bool IsOnTop => Stack.TopCard() == this;

        private void Awake()
        {
            GetComponent<CardRenderer>().UpdateCardVisuals(Data);
        }

        private void Update()
        {
            if (IsInStack && Stack.Cards.Count <= 1)
                Stack = null;

            if (IsInStack)
            {
                var index = Stack!.Cards.IndexOf(this);

                if (index - 1 < 0)
                    return;

                var previousCardPosition = Stack.Cards[index - 1].transform.position;
                transform.localPosition = previousCardPosition + new Vector3(0.0f, -2.0f, 0.0f);
            }

            if (Stack != null && Stack.HasCards)
            {
                var cards = Stack.Cards;
                foreach (var card in cards)
                {
                    card.transform.SetAsLastSibling();
                }
            }
        }
    }
}