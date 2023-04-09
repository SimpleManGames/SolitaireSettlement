using System.Collections.Generic;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckManager : Singleton<DeckManager>
    {
        [field: SerializeField]
        private Canvas CardCanvas { get; set; }

        [field: SerializeField]
        private DeckShownCardsVisual Visuals { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardData> CardsInDeck { get; set; }

        [field: ShowInInspector, ReadOnly]
        private List<CardData> ShownCards { get; set; }

        [ShowInInspector, ReadOnly]
        public List<CardData> CurrentlyVisibleShownCards =>
            new()
            {
                ShownCards?.Count > 2 ? ShownCards[^3] : null,
                ShownCards?.Count > 1 ? ShownCards[^2] : null,
                ShownCards?.Count > 0 ? ShownCards[^1] : null
            };

        [field: SerializeField]
        private int AmountOfCardsDrawn { get; set; } = 3;

        public bool HasCardsInDeck => CardsInDeck?.Count > 0;
        public bool IsShowingCards => CurrentlyVisibleShownCards?.Count > 0;

        public override void Awake()
        {
            base.Awake();

            CardsInDeck = new List<CardData>();
            ShownCards = new List<CardData>();
        }

        private void OnDestroy()
        {
            CardsInDeck = new List<CardData>();
            ShownCards = new List<CardData>();
        }

        public void AddCardToDeck(CardData cardData)
        {
            CardsInDeck.Add(cardData);
        }

        public void MoveCardTopCardToGame()
        {
            Visuals.MoveTopShownCardToGame();
            ShownCards.Remove(ShownCards[^1]);
        }

        public void InteractWithDeck()
        {
            if (HasCardsInDeck)
            {
                DrawFromDeck();
                return;
            }

            RecycleShownCards();
        }

        private void RecycleShownCards()
        {
            CardData topCard;

            while ((topCard = GetTopCardFromList(ShownCards)) != null)
            {
                CardsInDeck.Add(topCard);
                ShownCards.Remove(topCard);
            }
        }

        private void DrawFromDeck()
        {
            CardData topCard;
            var count = 0;

            while ((topCard = GetTopCardFromList(CardsInDeck)) != null && count++ < AmountOfCardsDrawn)
            {
                ShownCards.Add(topCard);
                CardsInDeck.Remove(topCard);
            }
        }

        private CardData GetTopCardFromList(List<CardData> cards)
        {
            return cards.Count > 0 ? cards[^1] : null;
        }
    }
}