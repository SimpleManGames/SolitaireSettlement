using System.Collections.Generic;
using UnityEngine;

namespace SolitaireSettlement
{
    public abstract class AddCardResult : IStackActionResult
    {
        protected const float DELAY_BETWEEN_CARD_SPAWNS = 0.2f;

        public virtual void OnResult(IEnumerable<Card> relatedCardStack)
        {
        }

        protected static void CreateCardToDeck(CardData card, Vector3 spawnPosition, float animationDelay = 0.0f)
        {
            CameraManager.Instance.GetPositionOnScreenSpaceCanvas(spawnPosition, out var pos);
            CardManager.Instance.CreateNewCardRuntimeInfo(card, CardRuntimeInfo.CardLocation.Deck,
                true, pos, animationDelay);
        }

        public abstract List<CardData> AddedCardData();
    }
}