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
        public bool Discovered { get; set; }

        [field: ShowInInspector]
        public bool ShouldRevealAfterPlanning { get; set; } = false;

        [field: ShowInInspector]
        public bool Revealed { get; set; } = false;

        public int Index { get; set; }

        private AreaVisual _areaVisual;

        private List<Card> _overlappingPersons = new();

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
            _cardObjectsInArea = ShownGameObject.transform.GetComponentsInChildren<Card>().ToList();
            var initialCardObjectCount = _cardObjectsInArea.Count;

            if (Data == null) return;

            for (var i = 0; i < Data.MaxCardCount - initialCardObjectCount; i++)
            {
                var newCardObject = Instantiate(CardManager.Instance.CardPrefab, ShownGameObject.transform);
                _cardObjectsInArea.Add(newCardObject.GetComponent<Card>());
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

            if (areaSpawnedCards.Count > _cardObjectsInArea.Count)
                Debug.LogError($"AreaData, {Data.Name}, Spawned Cards returned to many cards to fit in Area!");

            var areaRectTransform = GetComponent<RectTransform>();

            for (var i = 0; i < areaSpawnedCards.Count; i++)
            {
                var card = _cardObjectsInArea[i];
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

            ExpendPersonHungerWhenDiscovering();

            AddPersonToAreaIfNeeded();

            DiscoverNeighborAreas();

            Revealed = true;
            ShouldRevealAfterPlanning = false;
        }

        private void ExpendPersonHungerWhenDiscovering()
        {
            var areaRevealedCount = AreaManager.Instance.GeneratedAreaComponents.Count(a => a.Revealed);
            // First one should be free
            if (areaRevealedCount == 0)
                return;

            GatherPersonOverlaps();

            if (!_overlappingPersons.Any())
                return;
            
            // Expend hunger for the first detected person in the area
            var personCard = _overlappingPersons.First();
            var hungerCardUse = personCard.Info.Data.OnTurnUpdate
                .Where(c => c is HungerCardImpl)
                .Cast<HungerCardImpl>().First();

            if (hungerCardUse == null)
                return;

            hungerCardUse.ModifyHungerBy(-AreaManager.Instance.AreaDiscoverHungerCost);
            personCard.Render.UpdateDynamicTextFields(personCard.Info.Data);
        }

        private void AddPersonToAreaIfNeeded()
        {
            var personCount =
                CardManager.Instance.AllCardsInfo.Count(c => c.Data.CardType == CardData.ECardType.Person);

            if (personCount <= 0)
                return;

            var areaCount = AreaManager.Instance.GeneratedAreaComponents.Count(a => a.Revealed);

            if (areaCount <= 0)
                return;

            if (areaCount / personCount >= AreaManager.Instance.AreaPersonRatio)
            {
                CardManager.Instance.CreateNewCardRuntimeInfo(CardManager.Instance.PersonCard,
                    CardRuntimeInfo.CardLocation.GameBoard, false, transform.position, 0);
            }
        }

        private void DiscoverNeighborAreas()
        {
            var objs = new Area[AreaManager.Instance.AreaCountWidth, AreaManager.Instance.AreaCountHeight];
            for (int y = 0; y < AreaManager.Instance.AreaCountHeight; y++)
            {
                for (int x = 0; x < AreaManager.Instance.AreaCountHeight; x++)
                {
                    objs[x, y] =
                        AreaManager.Instance.GeneratedAreaComponents[y * AreaManager.Instance.AreaCountWidth + x];
                }
            }

            var arrayX = Index % AreaManager.Instance.AreaCountWidth;
            var arrayY = Index / AreaManager.Instance.AreaCountWidth;

            if (arrayY + 1 < AreaManager.Instance.AreaCountHeight)
            {
                objs[arrayX, arrayY + 1].Discovered = true;
                objs[arrayX, arrayY + 1].gameObject.SetActive(true);
            }

            if (arrayY - 1 >= 0)
            {
                objs[arrayX, arrayY - 1].Discovered = true;
                objs[arrayX, arrayY - 1].gameObject.SetActive(true);
            }

            if (arrayX + 1 < AreaManager.Instance.AreaCountWidth)
            {
                objs[arrayX + 1, arrayY].Discovered = true;
                objs[arrayX + 1, arrayY].gameObject.SetActive(true);
            }

            if (arrayX - 1 >= 0)
            {
                objs[arrayX - 1, arrayY].Discovered = true;
                objs[arrayX - 1, arrayY].gameObject.SetActive(true);
            }
        }

        public bool AnyPersonCardOverlapping()
        {
            GatherPersonOverlaps();
            return _overlappingPersons.Any();
        }

        private void GatherPersonOverlaps()
        {
            _overlappingPersons = GetOverlappingCards()
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