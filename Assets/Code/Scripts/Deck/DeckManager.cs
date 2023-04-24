using System;
using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Simplicity.Utility.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SolitaireSettlement
{
    public class DeckManager : Singleton<DeckManager>
    {
        [field: ShowInInspector, ReadOnly]
        private List<CardRuntimeInfo> _cardsInDeck;

        public int CardsInDeck => _cardsInDeck.Count;
        public bool HasCardsInDeck => CardsInDeck > 0;

        public bool HasEnoughCardsForFullDraw => _cardsInDeck.Count > CardsDrawnPerRound;

        [field: SerializeField]
        public int CardsDrawnPerRound { get; private set; }

        [field: SerializeField]
        private float DurationBetweenCardDraw { get; set; } = 0.5f;

        [field: SerializeField]
        public GameObject DeckGameObject { get; set; }

        public Vector3 DeckPosition => DeckGameObject.transform.position;

        private void Update()
        {
            // UpdateCardsInDeck();
        }

        public void UpdateCardsInDeck()
        {
            _cardsInDeck = CardManager.Instance.AllCardsInfo
                .Where(c => c.Location == CardRuntimeInfo.CardLocation.Deck)
                .ToList();
        }

        public void DrawCards()
        {
            var i = 0;
            while (i < CardsDrawnPerRound)
            {
                if (_cardsInDeck.Count <= 0)
                    break;

                var cardDataToDraw = _cardsInDeck[Random.Range(0, _cardsInDeck.Count)];
                cardDataToDraw.SetCardLocation(CardRuntimeInfo.CardLocation.Hand, true, i * DurationBetweenCardDraw);
                i++;
                _cardsInDeck.Remove(cardDataToDraw);
            }
        }
    }
}