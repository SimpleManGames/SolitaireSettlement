using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardManager : Singleton<CardManager>
    {
        [field: SerializeField, ReadOnly]
        public List<CardRuntimeInfo> AllCardsInfo { get; private set; }

        [field: SerializeField]
        public GameObject GameAreaCanvas { get; private set; }

        [field: SerializeField]
        public GameObject ScreenSpaceCanvas { get; private set; }

        [field: SerializeField]
        public Camera GameCamera { get; private set; }

        [field: SerializeField, AssetsOnly]
        public GameObject CardPrefab { get; private set; }

        [field: SerializeField, AssetsOnly]
        public CardData EmptyPlotCard { get; private set; }

        [field: SerializeField]
        private float DurationBetweenCardDiscard { get; set; } = 0.5f;

        private readonly Queue<GameObject> _toBeDestroyed = new();
        private readonly Queue<KeyValuePair<CardData, Card>> _toBeReplaced = new();

        private void Start()
        {
            // Grab the initial cards
            AllCardsInfo = GameObject.FindObjectsOfType<Card>()
                .Select(c => c.GetComponent<Card>().Info)
                .ToList();
        }

        private void LateUpdate()
        {
            while (_toBeDestroyed.TryDequeue(out var destroy) && destroy != null)
                DeleteCardObject(destroy);

            while (_toBeReplaced.TryDequeue(out var replace) && replace.Value != null)
                ReplaceCardDatFromRequested(replace.Key, replace.Value);
        }

        public void CreateNewCardRuntimeInfo(CardData data, CardRuntimeInfo.CardLocation location,
            bool animate = false, Vector3 initialPosition = default, float animateDelay = 0.0f)
        {
            var runtimeInfo = new CardRuntimeInfo(data, location, animate, initialPosition, animateDelay);
            AllCardsInfo.Add(runtimeInfo);
        }

        public void RequestToDeleteCardObject(GameObject cardObject)
        {
            _toBeDestroyed.Enqueue(cardObject);
        }

        private void DeleteCardObject(GameObject cardObject)
        {
            var card = cardObject.GetComponent<Card>();
            card.Stack?.RemoveCard(card);
            card.Info.SetCardLocation(CardRuntimeInfo.CardLocation.Delete);
            Destroy(cardObject);
        }

        public void SendLeftOverCardsToDiscard()
        {
            DiscardCards();
        }

        private void DiscardCards()
        {
            var leftOverCardsOnBoard = AllCardsInfo.Where(c =>
                    c.Location == CardRuntimeInfo.CardLocation.GameBoard &&
                    c.Data.CardType == CardData.ECardType.Resource)
                .ToList();

            for (var i = leftOverCardsOnBoard.Count - 1; i >= 0; i--)
            {
                var relatedCardComponent = leftOverCardsOnBoard[i].RelatedGameObject.GetComponent<Card>();
                relatedCardComponent.Stack?.RemoveCard(relatedCardComponent);

                var position =
                    GameCamera.WorldToScreenPoint(leftOverCardsOnBoard[i].RelatedGameObject.transform.position);

                leftOverCardsOnBoard[i].SetPosition(position);
                leftOverCardsOnBoard[i].SetCardLocation(CardRuntimeInfo.CardLocation.Discard, true);
            }
        }

        public void RequestToReplaceCardData(Card actualTargetCard, CardData replacementCard)
        {
            _toBeReplaced.Enqueue(new KeyValuePair<CardData, Card>(replacementCard, actualTargetCard));
        }

        private void ReplaceCardDatFromRequested(CardData replacementData, Card card)
        {
            card.UpdateCardData(replacementData);
        }
    }
}