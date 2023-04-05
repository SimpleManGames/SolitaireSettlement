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

        [field: SerializeField]
        private float VerticalOffset { get; set; } = -2.0f;

        [field: SerializeField]
        private float OrderingOffset { get; set; } = -0.1f;

        [field: ShowInInspector]
        [field: ReadOnly]
        public bool IsInDeck { get; set; }

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
                transform.localPosition = previousCardPosition + new Vector3(0.0f, VerticalOffset, OrderingOffset);
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

        public bool IsValidPlacement(ICardPlaceable placeable)
        {
            var cardObject = placeable as Card;
            return CanPlaceCardOnTarget(this, cardObject) && !IsInDeck;
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