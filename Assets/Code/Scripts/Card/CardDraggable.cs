using UnityEngine;

namespace SolitaireSettlement
{
    public class CardDraggable : MonoBehaviour, IUIDraggable
    {
        [field: SerializeField]
        private Canvas CardCanvas { get; set; }

        [field: SerializeField]
        private Card Card { get; set; }

        public bool CanBeDragged { get; set; } = true;

        private void Awake()
        {
            if (CardCanvas == null)
                CardCanvas = GetComponentInParent<Canvas>();

            CanBeDragged = true;
        }

        public void OnDrag(Vector2 position)
        {
            if (!CanBeDragged)
                return;

            // Simple way to draw the last card selected as the top one
            if (!Card.IsInStack)
                transform.SetAsLastSibling();

            var vec3Position = (Vector3)position;
            // vec3Position += -Vector3.forward;
            transform.position = CardCanvas.transform.TransformPoint(vec3Position);
        }

        public void OnDragCancel()
        {
            // transform.position += Vector3.forward;
        }
    }
}