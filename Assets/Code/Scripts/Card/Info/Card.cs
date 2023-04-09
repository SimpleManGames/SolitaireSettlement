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

        [field: ShowInInspector, ReadOnly, InlineEditor]
        public CardData Data { get; set; }

        [field: SerializeField]
        private IUIDrag Draggable { get; set; }

        [field: ShowInInspector]
        [field: ReadOnly]
        public bool IsInHand { get; set; }

        [field: ShowInInspector]
        [field: ReadOnly]
        public CardStack Stack { get; set; }

        [ShowInInspector]
        public bool IsInStack => Stack != null;

        public bool IsOnBottom => Stack.BottomCard() == this;
        public bool IsOnTop => Stack.TopCard() == this;

        private void Awake()
        {
            UpdateCardData(InternalDataReference);
            Draggable = GetComponent<CardDraggable>();
        }

        private void Update()
        {
            if (IsInStack && Stack.Cards.Count <= 1)
                Stack = null;

            if (IsInStack && !Draggable.IsBeDragging)
            {
                var index = Stack!.Cards.IndexOf(this);

                if (index - 1 < 0)
                    return;

                var previousCardPosition = Stack.Cards[index - 1].transform.position;
                transform.position = previousCardPosition + new Vector3(0.0f, -2.0f, 0.0f);
            }

            if (Stack != null && Stack.HasCards)
            {
                var cards = Stack.Cards;
                foreach (var card in cards)
                    card.transform.SetAsLastSibling();
            }
        }

        public void UpdateCardData(CardData data)
        {
            if (Data != null)
                Destroy(Data);

            if (data == null)
                return;

            InternalDataReference = data;

            Data = Instantiate(InternalDataReference);
            GetComponent<CardRenderer>().UpdateCardVisuals(Data);
        }

        public bool OnPlaced(GameObject target, GameObject place)
        {
            var placeOntoCard = target.GetComponent<Card>();
            var draggingCard = place.GetComponent<Card>();

            if (placeOntoCard.Stack != null && placeOntoCard.Stack.Cards.Contains(draggingCard))
                return false;

            // if (!placeOntoCard.IsValidPlacement(draggingCard))
            //     return false;

            UpdateStackInfoForDragObject(draggingCard);
            DetermineStackInteractions(placeOntoCard, draggingCard);

            // if (draggingCard.GetComponent<Card>().IsInHand)
            //     DeckManager.Instance.MoveCardTopCardToGame();

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
            switch (target.Data.CardType)
            {
                case CardData.ECardType.Resource:
                    return placing.Data.CardType == CardData.ECardType.Resource;
                case CardData.ECardType.Building:
                    return placing.Data.CardType == CardData.ECardType.Person;
                case CardData.ECardType.Person:
                    return placing.Data.CardType == CardData.ECardType.Resource;
                case CardData.ECardType.Gathering:
                    return placing.Data.CardType == CardData.ECardType.Person;
                default:
                    return false;
            }
        }
    }
}