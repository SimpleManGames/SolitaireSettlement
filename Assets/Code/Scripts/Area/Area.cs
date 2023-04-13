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

        private Card[] _cardObjectsInArea;

        [field: ShowInInspector]
        public bool ShouldRevealAfterPlanning { get; set; } = false;

        [field: ShowInInspector]
        public bool Revealed { get; set; } = false;

        private void Update()
        {
            ShouldRevealAfterPlanning = AnyPersonCardOverlapping();
        }

        public void OnRevealed()
        {
            _cardObjectsInArea = new Card[Data.MaxCardCount];
            _cardObjectsInArea = GetOverlappingCards()
                .Where(c => c.Info.Data.CardType == CardData.ECardType.Gathering)
                .ToArray();

            foreach (var card in _cardObjectsInArea)
                card.Area = this;
        }

        public bool AnyPersonCardOverlapping()
        {
            return GetOverlappingCards().Any(c => c.Info.Data.CardType == CardData.ECardType.Person);
        }

        private List<Card> GetOverlappingCards()
        {
            var gameBoardCards =
                CardManager.Instance.AllCardsInfo.Where(c => c.Location == CardRuntimeInfo.CardLocation.GameBoard);
            var areaRectTransform = GetComponent<RectTransform>();

            return (from card in gameBoardCards
                let cardRectTransform = card.RelatedGameObject.GetComponent<RectTransform>()
                where areaRectTransform.Overlaps(cardRectTransform)
                select card.RelatedGameObject.GetComponent<Card>()).ToList();
        }
    }
}