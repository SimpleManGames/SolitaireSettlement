using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Simplicity.Utility.Collections;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace SolitaireSettlement
{
    public class DiscardManager : Singleton<DiscardManager>
    {
        [field: ShowInInspector, ReadOnly]
        private List<CardRuntimeInfo> _cardsInDiscard;

        public int DiscardCardCount => _cardsInDiscard?.Count ?? 0;
        public bool HasCardsInDiscard => _cardsInDiscard?.Count > 0;

        [field: SerializeField]
        public GameObject DiscardCardLocation { get; private set; }

        public Vector3 DiscardCardPosition => DiscardCardLocation.transform.position;

        public void GatherCardsInDiscard()
        {
            _cardsInDiscard = CardManager.Instance.AllCardsInfo
                .Where(c => c.Location == CardRuntimeInfo.CardLocation.Discard)
                .ToList();
        }

        public void SendCardsFromDiscardIntoDeck()
        {
            if (_cardsInDiscard.IsNullOrEmpty())
                return;

            foreach (var card in _cardsInDiscard)
                card.SetCardLocation(CardRuntimeInfo.CardLocation.Deck);
        }
    }
}