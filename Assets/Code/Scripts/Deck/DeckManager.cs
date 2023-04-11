using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DeckManager : Singleton<DeckManager>
    {
        [field: ShowInInspector, ReadOnly]
        private List<CardRuntimeInfo> _cardsInDeck;

        private List<CardRuntimeInfo> CardsInDeck =>
            _cardsInDeck = CardManager.Instance.AllCardsInfo
                .Where(c => c.Location == CardRuntimeInfo.CardLocation.Deck)
                .ToList();

        private bool HasCardsInDeck => CardsInDeck.Count > 0;

        [field: SerializeField]
        public int CardsDrawnPerRound { get; private set; }

        [field: SerializeField]
        private float DurationBetweenCardDraw { get; set; } = 0.5f;

        [field: SerializeField]
        public GameObject DeckGameObject { get; set; }

        public Vector3 DeckPosition => DeckGameObject.transform.position;

        public void StartCoroutineDrawCards()
        {
            DrawCards();
        }

        private void DrawCards()
        {
            var i = 0;
            while (i < CardsDrawnPerRound)
            {
                if (CardsInDeck.Count <= 0)
                    break;

                var cardDataToDraw = CardsInDeck[0];
                cardDataToDraw.SetCardLocation(CardRuntimeInfo.CardLocation.Hand, true);
                i++;
            }
        }
    }
}