using System.Collections.Generic;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardStack
    {
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public List<Card> Cards { get; private set; }

        public bool HasCards => Cards.Count > 0;

        private readonly List<Card> _splitCards;

        public CardStack()
        {
            Cards = new List<Card>();
            _splitCards = new List<Card>();
        }

        public void StartStack(Vector3 position)
        {
            Position = position;
        }

        public void Clear()
        {
            Cards.Clear();
        }

        public Card BottomCard()
        {
            return HasCards ? Cards[0] : null;
        }

        public Card TopCard()
        {
            return HasCards ? Cards[^1] : null;
        }

        public bool CanAddCard(Card card)
        {
            return Card.CanPlaceCardOnTarget(TopCard(), card);
        }

        public void AddCard(Card card)
        {
            if (card == null)
                return;

            if (HasCards && !CanAddCard(card))
                return;

            card.Stack = this;
            Cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (card == null)
                return;

            if (Cards.Remove(card))
                card.Stack = null;
        }

        public void AddCards(IList<Card> cards)
        {
            if (cards == null)
                return;

            for (var i = 0; i < cards.Count; i++)
                AddCard(cards[i]);
        }

        public void RemoveCards(IList<Card> cards)
        {
            if (cards == null)
                return;

            for (var i = 0; i < cards.Count; i++)
                RemoveCard(cards[i]);
        }

        public IList<Card> SplitAt(Card card)
        {
            var index = Cards.IndexOf(card);

            if (index < 0 || index >= Cards.Count)
                return null;

            _splitCards.Clear();

            for (int i = index; i < Cards.Count; i++)
                _splitCards.Add(Cards[i]);

            return _splitCards;
        }
    }
}