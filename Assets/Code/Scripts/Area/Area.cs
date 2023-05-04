using System.Collections.Generic;
using System.Linq;
using Simplicity.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    [RequireComponent(typeof(AreaVisual))]
    public class Area : SerializedMonoBehaviour
    {
        [field: SerializeField]
        public AreaData Data { get; private set; }

        [field: SerializeField, ChildGameObjectsOnly]
        private GameObject ShownGameObject { get; set; }

        public List<Card> CardObjectsInArea { get; private set; }

        [field: ShowInInspector]
        public bool Discovered { get; set; }

        [field: ShowInInspector]
        public bool ShouldRevealAfterPlanning { get; set; } = false;

        [field: ShowInInspector]
        public bool Revealed { get; set; } = false;

        [field: SerializeField]
        private List<IAreaReveal> OnAreaRevealedActions { get; set; } = new();

        public int Index { get; set; }

        private AreaVisual _areaVisual;

        public List<Card> OverlappingPersons { get; private set; } = new();

        private void OnEnable()
        {
            _areaVisual = GetComponent<AreaVisual>();
        }

        #region Setup

        public void SetAreaData(AreaData data)
        {
            Data = data;
            SetupAreaData();
        }

        private void SetupAreaData()
        {
            CreateInitialCardsBasedOnAreaData();

            DetermineSpawnedCardsBasedOnAreaData();

            ApplyAreaDataVisuals();
        }

        private void CreateInitialCardsBasedOnAreaData()
        {
            CardObjectsInArea = ShownGameObject.transform.GetComponentsInChildren<Card>().ToList();
            var initialCardObjectCount = CardObjectsInArea.Count;

            if (Data == null) return;

            for (var i = 0; i < Data.MaxCardCount - initialCardObjectCount; i++)
            {
                var newCardObject = Instantiate(CardManager.Instance.CardPrefab, ShownGameObject.transform);
                CardObjectsInArea.Add(newCardObject.GetComponent<Card>());
            }
        }

        private void DetermineSpawnedCardsBasedOnAreaData()
        {
            var areaSpawnedCards = Data.GetSpawnedCards(true);
            if (areaSpawnedCards.Count == 0)
            {
                Debug.LogError($"AreaData, {Data.Name}, Spawned Cards failed to determine what cards to spawn!");
                return;
            }

            if (areaSpawnedCards.Count > CardObjectsInArea.Count)
                Debug.LogError($"AreaData, {Data.Name}, Spawned Cards returned to many cards to fit in Area!");

            var areaRectTransform = GetComponent<RectTransform>();

            for (var i = 0; i < areaSpawnedCards.Count; i++)
            {
                var card = CardObjectsInArea[i];
                card.UpdateCardData(areaSpawnedCards[i]);
                card.Area = this;
                var rectTransform = card.GetComponent<RectTransform>();
                card.transform.position = rectTransform.RandomPointWithinRectBounds();

                rectTransform.ClampWithin(areaRectTransform);
            }
        }

        private void ApplyAreaDataVisuals()
        {
            _areaVisual.SetAreaDataColor(Data.Color);
        }

        #endregion

        private void Update()
        {
            // This shouldn't be called every frame. Only should happen when dropping a card
            // Could use an event for it.
            ShouldRevealAfterPlanning = AnyPersonCardOverlapping();
        }

        public void OnRevealed()
        {
            if (Revealed)
                return;

            foreach (var revealImpl in OnAreaRevealedActions)
                revealImpl.OnAreaReveal(this);

            Revealed = true;
            ShouldRevealAfterPlanning = false;
        }

        public bool AnyPersonCardOverlapping()
        {
            GatherPersonOverlaps();
            return OverlappingPersons.Any();
        }

        private void GatherPersonOverlaps()
        {
            OverlappingPersons = GetOverlappingCards()
                .Where(c => c.Info.Data.CardType == CardData.ECardType.Person && !c.IsInStack).ToList();
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