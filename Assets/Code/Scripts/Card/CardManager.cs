using System.Collections.Generic;
using System.Linq;
using Simplicity.Singleton;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardManager : Singleton<CardManager>
    {
        [field: Title("Data")]
        [field: SerializeField, Sirenix.OdinInspector.ReadOnly]
        public List<CardRuntimeInfo> AllCardsInfo { get; private set; }

        [field: Title("Scene References")]
        [field: SerializeField]
        public GameObject GameAreaCanvas { get; private set; }

        [field: SerializeField]
        public GameObject ScreenSpaceCanvas { get; private set; }

        [field: SerializeField]
        public Camera GameCamera { get; private set; }

        [field: Title("Prefabs")]
        [field: SerializeField, AssetsOnly]
        public GameObject CardPrefab { get; private set; }

        [field: SerializeField, AssetsOnly]
        public CardData EmptyPlotCard { get; private set; }

        [field: SerializeField, AssetsOnly]
        public CardData PersonCard { get; private set; }

        [field: Title("Info")]
        [field: SerializeField]
        public int BasePopulateCap { get; private set; }

        [field: ShowInInspector, ReadOnly]
        public int PopulationCap { get; private set; }
        
        [ShowInInspector]
        public int PersonCount => Instance.AllCardsInfo.Count(c => c.Data.CardType == CardData.ECardType.Person);

        public bool CanAddToPopulation => PersonCount < CardManager.Instance.PopulationCap;

        [field: SerializeField]
        private float DurationBetweenCardDiscard { get; set; } = 0.5f;

        private readonly Queue<GameObject> _toBeDestroyed = new();
        private readonly Queue<KeyValuePair<CardData, Card>> _toBeReplaced = new();

        private void Start()
        {
            GatherCardReferences();
        }

        private void LateUpdate()
        {
            while (_toBeDestroyed.TryDequeue(out var destroy) && destroy != null)
                DeleteCardObject(destroy);

            while (_toBeReplaced.TryDequeue(out var replace) && replace.Value != null)
                ReplaceCardDatFromRequested(replace.Key, replace.Value);

            for (var i = AllCardsInfo.Count - 1; i >= 0; i--)
            {
                var card = AllCardsInfo[i];
                if (card.Location == CardRuntimeInfo.CardLocation.Delete)
                    AllCardsInfo.Remove(card);
            }

            DeterminePopulationCap();
        }

        public void AddCardFromAreaReveal(List<Card> areaCards)
        {
            AllCardsInfo.AddRange(areaCards.Select(c => c.Info));
        }

        private void GatherCardReferences()
        {
            AllCardsInfo = FindObjectsOfType<Card>()
                .Select(c => c.GetComponent<Card>().Info)
                .ToList();
        }

        private void DeterminePopulationCap()
        {
            var capBonusFromCards = 0;

            var validCardBonuses = AllCardsInfo?.Where(w => !w.Data.CardBonuses.IsNullOrEmpty()).ToList();
            if (validCardBonuses != null)
            {
                var cardBonuses = validCardBonuses.SelectMany(c => c.Data.CardBonuses)
                    .Where(cardBonus => cardBonus is GlobalPopulateBonus)
                    .Cast<GlobalPopulateBonus>().ToList();

                capBonusFromCards = cardBonuses.Sum(b => b.BonusToPopulateCap);
            }

            PopulationCap = BasePopulateCap + capBonusFromCards;
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
            AllCardsInfo.Remove(card.Info);
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

        public void ProgressCardData()
        {
            var possibleCardProgress = AllCardsInfo.Where(r => r.Data.OnTurnUpdate != null);
            var correctLocationCardProgress =
                possibleCardProgress.Where(r => r.Location == CardRuntimeInfo.CardLocation.GameBoard);

            foreach (var card in correctLocationCardProgress)
            {
                var cardObject = card.RelatedGameObject.GetComponent<Card>();
                foreach (var progress in card.Data.OnTurnUpdate)
                    progress.TurnProgress(cardObject);
            }
        }
    }
}