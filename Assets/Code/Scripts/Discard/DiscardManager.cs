using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Simplicity.Utility.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DiscardManager : Singleton<DiscardManager>
    {
        [field: ShowInInspector, ReadOnly]
        private List<CardRuntimeInfo> _cardsInDiscard;

        public List<CardRuntimeInfo> CardsInDiscard =>
            _cardsInDiscard = CardManager.Instance.AllCardsInfo
                .Where(c => c.Location == CardRuntimeInfo.CardLocation.Discard)
                .ToList();

        [field: SerializeField]
        public GameObject DiscardCardLocation { get; private set; }

        public Vector3 DiscardCardPosition => DiscardCardLocation.transform.position;

        public void ShuffleDiscardIntoDeck()
        {
            CardsInDiscard.FisherYatesShuffle();
            var shuffledDiscardDeck = CardsInDiscard;
            foreach (var card in shuffledDiscardDeck)
            {
                card.SetCardLocation(CardRuntimeInfo.CardLocation.Deck);
            }
        }
    }
}