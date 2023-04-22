using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public abstract class AddCardResult : IStackActionResult
    {
        protected const float DELAY_BETWEEN_CARD_SPAWNS = 0.2f;

        [field: SerializeField, LabelText("Location of Added Card")]
        protected CardRuntimeInfo.CardLocation Location { get; set; } = CardRuntimeInfo.CardLocation.Deck;

        public virtual void OnResult(IEnumerable<Card> relatedCardStack)
        {
        }

        protected static void CreateCardTo(CardData card, CardRuntimeInfo.CardLocation location,
            Vector3 spawnPosition, float animationDelay = 0.0f)
        {
            if (card.CardType == CardData.ECardType.Person && !CardManager.Instance.CanAddToPopulation)
                return;

            if (location != CardRuntimeInfo.CardLocation.GameBoard)
            {
                CameraManager.Instance.GetPositionOnScreenSpaceCanvas(spawnPosition, out var pos);
                spawnPosition = pos;
            }

            CardManager.Instance.CreateNewCardRuntimeInfo(card, location, true, spawnPosition, animationDelay);
        }

        public abstract List<CardData> AddedCardData();
    }
}