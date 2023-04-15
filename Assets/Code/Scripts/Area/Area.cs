using System;
using System.Collections.Generic;
using System.Linq;
using Simplicity.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [RequireComponent(typeof(AreaVisual))]
    public class Area : MonoBehaviour
    {
        [field: SerializeField]
        private AreaData Data { get; set; }

        [field: SerializeField, ChildGameObjectsOnly]
        private GameObject ShownGameObject { get; set; }

        private List<Card> _cardObjectsInArea;

        [field: ShowInInspector]
        public bool ShouldRevealAfterPlanning { get; set; } = false;

        [field: ShowInInspector]
        public bool Revealed { get; set; } = false;

        private void OnEnable()
        {
            _cardObjectsInArea = ShownGameObject.transform.GetComponentsInChildren<Card>().ToList();
            var initialCardObjectCount = _cardObjectsInArea.Count;

            for (var i = 0; i < Data.MaxCardCount - initialCardObjectCount; i++)
            {
                var newCardObject = Instantiate(CardManager.Instance.CardPrefab, ShownGameObject.transform);
                _cardObjectsInArea.Add(newCardObject.GetComponent<Card>());
            }

            var areaSpawnedCards = Data.GetSpawnedCards(true);
            if (areaSpawnedCards.Count != _cardObjectsInArea.Count)
                Debug.LogError("Area Spawned Cards returned to many cards to fit in Area!");

            for (var i = 0; i < _cardObjectsInArea.Count; i++)
            {
                var card = _cardObjectsInArea[i];
                card.UpdateCardData(areaSpawnedCards[i]);
                card.Area = this;
            }
        }

        private void Update()
        {
            // This shouldn't be called every frame. Only should happen when dropping a card
            // Could use an event for it.
            ShouldRevealAfterPlanning = AnyPersonCardOverlapping();
        }

        public void OnRevealed()
        {
        }

        public bool AnyPersonCardOverlapping()
        {
            return GetOverlappingCards().Any(c => c.Info.Data.CardType == CardData.ECardType.Person);
        }

        private List<Card> GetOverlappingCards()
        {
            var gameBoardCards =
                CardManager.Instance.AllCardsInfo.Where(c =>
                    c.Location == CardRuntimeInfo.CardLocation.GameBoard && c.RelatedGameObject != null);
            var areaRectTransform = GetComponent<RectTransform>();

            return (from card in gameBoardCards
                let cardRectTransform = card.RelatedGameObject.GetComponent<RectTransform>()
                where areaRectTransform.Overlaps(cardRectTransform)
                select card.RelatedGameObject.GetComponent<Card>()).ToList();
        }
    }
}