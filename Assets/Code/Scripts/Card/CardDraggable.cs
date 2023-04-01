using System;
using UnityEngine;

namespace SolitaireSettlement
{
    public class CardDraggable : MonoBehaviour, IUIDraggable
    {
        [field: SerializeField] private Canvas CardCanvas { get; set; }

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
            transform.SetAsLastSibling();
            transform.position = CardCanvas.transform.TransformPoint(position);
        }
    }
}