using Simplicity.UI;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardDraggable : MonoBehaviour, IUIDrag
    {
        [field: SerializeField]
        private Canvas CardCanvas { get; set; }

        [field: SerializeField]
        private Card Card { get; set; }

        public bool CanBeDragged { get; set; } = true;

        public bool IsBeingDragged { get; set; } = false;

        private Vector3 _lastValidDragPosition = Vector3.zero;

        private Vector3 _offset = Vector3.zero;

        private void Awake()
        {
            CanBeDragged = Card.Info.Data == null || Card.Info.Data.FreeDragging;
            IsBeingDragged = false;

            CardCanvas = CardManager.Instance.GameAreaCanvas.GetComponent<Canvas>();
        }

        public void OnDragStart(Vector2 position)
        {
            if (!Card.Info.Data.FreeDragging)
                return;

            IsBeingDragged = true;

            if (Card.Info.Location != CardRuntimeInfo.CardLocation.Hand)
                _offset = transform.position - (Vector3)position;

            Card.Info.SetCardLocation(CardRuntimeInfo.CardLocation.GameBoard);
            transform.SetParent(CardCanvas.transform);
        }

        public void OnDrag(Vector2 position)
        {
            if (!CanBeDragged || !IsBeingDragged)
                return;

            // Simple way to draw the last card selected as the top one
            if (!Card.IsInStack)
                transform.SetAsLastSibling();

            _lastValidDragPosition = position;

            transform.position = CardCanvas.transform.TransformPoint(_lastValidDragPosition + _offset);
            if (!Card.CanLeaveArea)
            {
                var areaRectTransform = Card.Area.GetComponent<RectTransform>();
                var cardRectTransform = GetComponent<RectTransform>();

                cardRectTransform.ClampWithin(areaRectTransform);
            }
        }

        public void OnDragEnd()
        {
            IsBeingDragged = false;
        }
    }
}