using System.Linq;
using Simplicity.Utility.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SolitaireSettlement
{
    public class Card : MonoBehaviour, ICardPlaceable
    {
        [field: SerializeField,
                Tooltip("ScriptableObject References that this Card pulls info from.\n " +
                        "We create a copy of this during runtime to modify values on it without effecting the actual asset.")]
        public CardData InternalDataReference { get; private set; }

        [field: SerializeField]
        public CardRuntimeInfo Info { get; private set; }

        [field: SerializeField]
        private IUIDrag Draggable { get; set; }

        [field: SerializeField]
        public CardRenderer Render { get; private set; }

        public bool IsInHand => Info.Location == CardRuntimeInfo.CardLocation.Hand;

        public StackActionData InvolvedStackAction { get; set; }
        
        [field: ShowInInspector]
        [field: ReadOnly]
        public CardStack Stack { get; set; }

        [ShowInInspector]
        public bool IsInStack => Stack != null;

        public bool IsOnBottom => Stack.BottomCard() == this;
        public bool IsOnTop => Stack.TopCard() == this;

        [field: ShowInInspector]
        [field: ReadOnly]
        public Area Area { get; set; }

        public bool IsInArea => Area != null;

        public bool CanLeaveArea => Info.Data.CardType != CardData.ECardType.Building &&
                                    Info.Data.CardType != CardData.ECardType.Gathering;

        private void Awake()
        {
            UpdateCardData(InternalDataReference);
            Draggable = GetComponent<CardDraggable>();
            Render = GetComponent<CardRenderer>();
        }

        private void Start()
        {
            Info.SetRelatedGameObject(gameObject);

            Info.Data.CardUse?.ForEach(u => u.Initialize());
            Info.Data.OnTurnUpdate?.ForEach(t => t.Initialize(this));
        }

        private void Update()
        {
            if (IsInStack && Stack.Cards.Count <= 1)
                Stack = null;

            if (IsInStack && !Draggable.IsBeingDragged)
            {
                var index = Stack!.Cards.IndexOf(this);

                if (index - 1 < 0)
                    return;

                if (Stack.Cards[index - 1] == null)
                    return;

                var previousCardPosition = Stack.Cards[index - 1].transform.position;
                transform.position = previousCardPosition + new Vector3(0.0f, -2.0f, 0.0f);
            }

            if (Draggable.IsBeingDragged && IsInArea && CanLeaveArea)
            {
                Area.ShouldRevealAfterPlanning = false;
                Area = null;
            }

            if (Stack == null || !Stack.HasCards)
                return;

            var cards = Stack.Cards;
            foreach (var card in cards)
                card.transform.SetAsLastSibling();
        }

        public void UpdateCardInfo(CardRuntimeInfo info)
        {
            if (info == null)
                return;

            Info = info;
            Info.SetRelatedGameObject(gameObject);
        }

        public void UpdateCardData(CardData data)
        {
            if (Info.Data != null)
            {
                Info.Data = null;
            }

            if (data == null)
                return;

            InternalDataReference = data;

            Info.Data = Instantiate(InternalDataReference);

            Info.Data.CardUse?.ForEach(u => u.Initialize());
            Info.Data.OnTurnUpdate?.ForEach(t => t.Initialize(this));
            GetComponent<CardRenderer>().UpdateCardVisuals(Info.Data);
        }

        public bool OnRemoved()
        {
            return true;
        }

        public bool OnPlaced(GameObject target, GameObject place)
        {
            var placeOntoCard = target.GetComponent<Card>();
            var draggingCard = place.GetComponent<Card>();

            if (placeOntoCard != null)
                return OnPlacedCard(placeOntoCard, draggingCard);

            var placeOntoArea = target.GetComponent<Area>();

            return placeOntoArea != null && OnPlacedArea(placeOntoArea, draggingCard);
        }

        private bool OnPlacedCard(Card placed, Card dragging)
        {
            if (placed.Stack != null && placed.Stack.Cards.Contains(dragging))
                return false;

            // if (!placeOntoCard.IsValidPlacement(draggingCard))
            //     return false;

            UpdateStackInfoForDragObject(dragging);
            DetermineStackInteractions(placed, dragging);

            // if (draggingCard.GetComponent<Card>().IsInHand)
            //     DeckManager.Instance.MoveCardTopCardToGame();

            return true;
        }

        private bool OnPlacedArea(Area placed, Card dragging)
        {
            placed.ShouldRevealAfterPlanning = true;
            dragging.Area = placed;
            return true;
        }

        public bool IsValidPlacement(ICardPlaceable placeable)
        {
            var cardObject = placeable as Card;
            return CanPlaceCardOnTarget(this, cardObject) && !IsInHand;
        }

        private void UpdateStackInfoForDragObject(Card cardObject)
        {
            if (!cardObject.IsInStack)
                return;

            if (cardObject.IsOnTop)
            {
                cardObject.Stack.RemoveCard(cardObject);
            }
            else if (!cardObject.IsOnBottom)
            {
                var cards = cardObject.Stack.SplitAt(cardObject);
                cardObject.Stack.RemoveCards(cards);
                cardObject.Stack = new CardStack();
                cardObject.Stack.AddCards(cards);
            }
        }

        private void DetermineStackInteractions(Card placeOntoCard, Card draggingCard)
        {
            if (placeOntoCard.Stack == null) // New Stack
            {
                // New stack is being made, so we need to add both of them to it
                placeOntoCard.Stack = new CardStack();
                placeOntoCard.Stack.AddCard(placeOntoCard);
            }

            if (draggingCard.Stack != null && draggingCard.Stack.HasCards)
                AddDraggingCardsStackCards(placeOntoCard, draggingCard);
            else
                AddSingleCardToStack(placeOntoCard, draggingCard);
        }

        private static void AddSingleCardToStack(Card placeOntoCard, Card draggingCard)
        {
            placeOntoCard.Stack.AddCard(draggingCard);
        }

        private void AddDraggingCardsStackCards(Card placeOntoCard, Card draggingCard)
        {
            placeOntoCard.Stack.AddCards(draggingCard.Stack.Cards);
        }

        public static bool CanPlaceCardOnTarget(Card target, Card placing)
        {
            var validCardsOnStack = target.IsInStack
                ? target.Stack.Cards.SelectMany(c =>
                    c.InternalDataReference.ValidStackableCards)
                : target.InternalDataReference.ValidStackableCards;

            return validCardsOnStack.Any(c => c == placing.InternalDataReference);
        }
    }
}