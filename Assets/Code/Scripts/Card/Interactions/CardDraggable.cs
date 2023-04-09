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

        public bool IsBeDragging { get; set; } = false;

        private void Awake()
        {
            CanBeDragged = true;
            IsBeDragging = false;
        }

        public void OnDragStart()
        {
            IsBeDragging = true;
            transform.localScale = Vector3.one;
            transform.rotation = CardCanvas.transform.rotation;
        }

        public void OnDrag(Vector2 position)
        {
            if (!CanBeDragged)
                return;

            // Simple way to draw the last card selected as the top one
            if (!Card.IsInStack)
                transform.SetAsLastSibling();

            var vec3Position = (Vector3)position;
            transform.SetParent(CardCanvas.transform);
            transform.position = CardCanvas.transform.TransformPoint(vec3Position);
        }

        public void OnDragEnd()
        {
            IsBeDragging = false;
        }
    }
}